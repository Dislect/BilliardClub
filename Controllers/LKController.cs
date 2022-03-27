using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.HangfireService;
using BilliardClub.Models;
using BilliardClub.Models.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.Controllers
{
    [Authorize]
    public class LKController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly Cart _cart;

        public LKController(Context context, UserManager<User> userManager, Cart cart)
        {
            _context = context;
            _userManager = userManager;
            _cart = cart;
        }

        [HttpGet]
        public ActionResult Info()
        {
            return View(_cart);
        }

        #region Order

        [HttpGet]
        public IActionResult _Order()
        {
            return PartialView(_cart);
        }

        [HttpPost]
        public IActionResult DeleteTableInCart(int id)
        {
            if (_cart.CartPoolTables.Exists(x => x.PoolTable != null && x.PoolTable.id == id))
            {
                var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
                _cart.DeleteTableInCart(table);
                return Ok();
            }
            return BadRequest("Стол не находится в корзине");
        }

        [HttpPost]
        public void DeleteProductInCart(int id)
        {
            var foodItem = _context.FoodItems.FirstOrDefault(x => x.id == id);
            _cart.DeleteProductInCart(foodItem);
        }

        [HttpPost]
        public async Task UpdateTableNumberHours(int cartPoolTableId, uint numberHours)
        {
            if (_cart.CartPoolTables.Exists(x => x.id == cartPoolTableId))
            {
                if (numberHours <= 0)
                    numberHours = 1;

                if (numberHours >= 6)
                    numberHours = 6;

                var cartItem = _context.CartPoolTables.First(x => x.id == cartPoolTableId && x.cartId == _cart.cartId);
                cartItem.numberHours = numberHours;
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task UpdateProductQuantity(int cartFoodItemId, uint quantity)
        {
            if (_cart.CartFoodItems.Exists(x => x.id == cartFoodItemId))
            {
                if (quantity <= 0)
                    quantity = 1;

                if (quantity >= 10)
                    quantity = 10;

                var cartItem = _context.CartFoodItems.First(x => x.id == cartFoodItemId && x.cartId == _cart.cartId);
                cartItem.quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmOrder()
        {
            if (_cart.CartPoolTables.Any()
                || _cart.CartFoodItems.Any())
            {
                double receipt = 0;
                var order = new Order() { orderDate = DateTime.Now, user = await _userManager.GetUserAsync(User) };
                var tablesInCart = _cart.CartPoolTables;
                var productsInCart = _cart.CartFoodItems;

                if (tablesInCart.Any())
                {
                    foreach (var tableInCart in tablesInCart)
                    {
                        if (tableInCart.reservationDate.AddMinutes(2) < DateTime.Now)
                        {
                            return BadRequest($"Нельзя забронировать стол №{tableInCart.PoolTable.name} на прошедшее время.");
                        }

                        if (IsSuperimposedOnReservedTime(tableInCart))
                        {
                            return BadRequest($"Стол №{tableInCart.PoolTable.name} уже забронирован на эту дату и время.");
                        }

                        if (tableInCart.reservationDate.AddMinutes(-5) > DateTime.Now)
                        {
                            // если время не пересекается с другими заявками на этот стол, то создается новый статус
                            var statusReservOnDate = _context.Status
                                .FirstOrDefault(x => x.name == "Забронирован к дате");

                            if (tableInCart.PoolTable.statusTables.Any())
                            {
                                tableInCart.PoolTable.statusTables.Last().dateEnd = DateTime.Now;
                            }

                            tableInCart.PoolTable.statusTables.Add(new()
                            {
                                dateStart = tableInCart.reservationDate,
                                dateEnd = tableInCart.reservationDate.AddHours(tableInCart.numberHours),
                                status = statusReservOnDate
                            });

                            CreateReservJob(tableInCart);
                            _cart.DeleteTableInCart(tableInCart.PoolTable);
                        }
                        else
                        {
                            var statusReserve = _context.Status.First(x => x.name == "Забронирован");

                            if (tableInCart.PoolTable.statusTables.Any())
                            {
                                tableInCart.PoolTable.statusTables.Last().dateEnd = DateTime.Now;
                            }

                            tableInCart.PoolTable.statusTables.Add(new StatusTable()
                            {
                                dateStart = tableInCart.reservationDate,
                                dateEnd = tableInCart.reservationDate.AddHours(tableInCart.numberHours),
                                status = statusReserve
                            });

                            CreateJob(tableInCart);
                            _cart.DeleteTableInCart(tableInCart.PoolTable);
                        }

                        receipt += tableInCart.numberHours * tableInCart.PoolTable.typeTable.price;

                        order.poolTables.Add(new OrderPoolTable()
                        {
                            poolTable = tableInCart.PoolTable,
                            numberHours = tableInCart.numberHours
                        });
                    }
                }

                if (productsInCart.Any())
                {
                    foreach (var productInCart in productsInCart)
                    {
                        receipt += productInCart.quantity * productInCart.FoodItem.price;

                        order.foodItems.Add(new OrderFoodItem()
                        {
                            foodItem = productInCart.FoodItem,
                            quantity = productInCart.quantity
                        });
                        _cart.DeleteProductInCart(productInCart.FoodItem);
                    }
                }

                order.receipt = receipt;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
                return Ok();
            }
            return BadRequest("В корзине отсутствуют бильярдные столы и еда");
        }

        private bool IsSuperimposedOnReservedTime(CartPoolTable tableInCart)
        {
            var statuses = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .First(x => x.id == tableInCart.PoolTable.id)
                .statusTables.Where(x => x.status.name == "Забронирован к дате" && x.dateStart > DateTime.Now)
                .ToList();

            if (!statuses.Any())
            {
                return false;
            }

            foreach (var statusReserve in statuses)
            {
                if (tableInCart.reservationDate.DayOfYear != statusReserve.dateStart.DayOfYear) continue;

                var newDateReserv = tableInCart.reservationDate.AddHours(tableInCart.numberHours).TimeOfDay;

                if (newDateReserv.IsBetween(statusReserve.dateStart.TimeOfDay, statusReserve.dateEnd?.TimeOfDay))
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateReservJob(CartPoolTable tableInCart)
        {
            // создание задачи на бронирование стола к определенной дате
            BackgroundJob.Schedule<OrderService>(x => x.ReservationForSelectedDateJob(tableInCart.PoolTable.id),
                tableInCart.reservationDate);

            BackgroundJob.Schedule<OrderService>(x => x.ReleaseOnSelectedDateJob(tableInCart.PoolTable.id),
                tableInCart.reservationDate.AddHours(tableInCart.numberHours));
        }

        private void CreateJob(CartPoolTable tableInCart)
        {
            // моментальное бронирование
            BackgroundJob.Enqueue<OrderService>(x => x.ReservationForSelectedDateJob(tableInCart.PoolTable.id));

            BackgroundJob.Schedule<OrderService>(x => x.ReleaseOnSelectedDateJob(tableInCart.PoolTable.id),
                tableInCart.reservationDate.AddHours(tableInCart.numberHours));
        }

        #endregion

        #region History

        [HttpGet]
        public async Task<IActionResult> _History()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Orders
                .Where(x => x.user.Id == user.Id)
                .Include(x => x.user)
                .Include(x => x.poolTables).ThenInclude(x => x.poolTable)
                .Include(x => x.foodItems).ThenInclude(x => x.foodItem)
                .ToListAsync();
            return PartialView(orders);
        }

        #endregion

        #region Account settings

        [HttpGet]
        public async Task<IActionResult> _AccountSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            return PartialView(user);
        }

        [HttpPost]
        public async Task<bool> ChangePassword(string OldPassword, string NewPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            IdentityResult result =
                await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            return result.Succeeded;
        }

        public async Task ChangeEmail(string newEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            user.Email = newEmail;
            user.UserName = newEmail;
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
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
        public void DeleteTableInCart(int id)
        {
            if (_cart.CartPoolTables.Exists(x => x.PoolTable != null && x.PoolTable.id == id))
            {
                var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
                _cart.DeleteTableInCart(table);
            }
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
        public async Task ConfirmOrder()
        {
            if (_cart.CartPoolTables.Any()
                || _cart.CartFoodItems.Any())
            {
                double cheque = 0;
                var order = new Order() { orderDate = DateTime.Now, user = await _userManager.GetUserAsync(User) };
                var tablesInCart = _cart.CartPoolTables;
                var productsInCart = _cart.CartFoodItems;

                if (tablesInCart.Any())
                {
                    var statusReserve = _context.Status.First(x => x.name == "Забронирован");

                    foreach (var tableInCart in tablesInCart)
                    {
                        cheque += tableInCart.numberHours * tableInCart.PoolTable.typeTable.price;

                        if (tableInCart.PoolTable.statusTables.Any())
                        {
                            tableInCart.PoolTable.statusTables.Last().dateEnd = DateTime.Now;
                        }

                        tableInCart.PoolTable.statusTables.Add(new StatusTable()
                        { dateStart = DateTime.Now, dateEnd = DateTime.Now.AddHours(tableInCart.numberHours), status = statusReserve });

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
                        cheque += productInCart.quantity * productInCart.FoodItem.price;

                        order.foodItems.Add(new OrderFoodItem()
                        {
                            foodItem = productInCart.FoodItem,
                            quantity = productInCart.quantity
                        });
                    }
                }

                order.cheque = cheque;
                _context.Orders.Add(order);
                _cart.DeleteAllItemsInCart();

                await _context.SaveChangesAsync();
            }
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
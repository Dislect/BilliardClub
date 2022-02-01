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
            return View(_cart.CartItems);
        }

        [HttpGet]
        public IActionResult _Order()
        {
            return PartialView(_cart.CartItems);
        }

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

        [HttpPost]
        public async Task UpdateQuantity(int cartItemId, uint quantity)
        {
            if (_cart.CartItems.Exists(x => x.id == cartItemId))
            {
                if (quantity <= 0)
                    quantity = 1;

                if (quantity >= 15)
                    quantity = 15;

                var cartItem = _context.CartItems.First(x => x.cartId == _cart.cartId && x.id == cartItemId);
                cartItem.quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public void DeleteTableInCart(int id)
        {
            if (_cart.CartItems.Exists(x => x.PoolTable != null && x.PoolTable.id == id))
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
        public async Task ConfirmOrder()
        {
            if (_cart.CartItems.Count != 0)
            {
                double cheque = 0;
                var order = new Order() { orderDate = DateTime.Now, user = await _userManager.GetUserAsync(User) };
                var tablesInCart = _cart.CartItems.Where(x => x.PoolTable != null).ToList();
                var productsInCart = _cart.CartItems.Where(x => x.FoodItem != null).ToList();
                
                if (tablesInCart.Count != 0)
                {
                    var statusReserve = _context.Status.First(x => x.name == "Забронирован");

                    foreach (var tableInCart in tablesInCart)
                    {
                        cheque += tableInCart.quantity * tableInCart.PoolTable.typeTable.price;

                        if (tableInCart.PoolTable.statusTables.Count != 0)
                        {
                            tableInCart.PoolTable.statusTables.Last().dateEnd = DateTime.Now;
                        }

                        tableInCart.PoolTable.statusTables.Add(new StatusTable()
                            {dateStart = DateTime.Now ,dateEnd = DateTime.Now.AddHours(tableInCart.quantity), status = statusReserve});

                        order.poolTables.Add(new OrderPoolTable()
                        {
                            poolTable = tableInCart.PoolTable,
                            quantity = tableInCart.quantity
                        });
                    }
                }

                if (productsInCart.Count != 0)
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
    }
}
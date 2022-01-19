using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using BilliardClub.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.Controllers
{
    [Authorize(Roles = "user,employee")]
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
            LKViewModel viewModel = new()
            {
                CartItems = _cart.CartItems
            };
            return View(viewModel);
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
                .Include(x => x.user)
                .Include(x => x.poolTables)
                .Include(x => x.restaurantMenu)
                .Where(x => x.user.Id == user.Id).ToListAsync();
            return PartialView(orders);
        }


        [HttpGet]
        public async Task<IActionResult> _AccountSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ChangePasswordViewModel()
            {
                Email = user.Email
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<bool> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            IdentityResult result =
                await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result.Succeeded;
        }
        
        [HttpPost]
        public void DeleteTableInCart(int id)
        {
            var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
            _cart.DeleteTableInCart(table);
        }

        public void DeleteAllItemsInCart()
        {
            _cart.DeleteAllItemsInCart();
        }

        [HttpPost]
        public async Task ConfirmOrder(double cheque)
        {
            var order = new Order() {orderDate = DateTime.Now, cheque = cheque, user = await _userManager.GetUserAsync(User)};
            var tables = _cart.CartItems.Where(x => x.PoolTable != null).Select(x => x.PoolTable).ToList();
            var products = _cart.CartItems.Where(x => x.RestaurantMenu != null).Select(x => x.RestaurantMenu).ToList();
            order.poolTables.AddRange(tables);
            order.restaurantMenu.AddRange(products);
            _context.Orders.Add(order);
            _cart.DeleteAllItemsInCart();
            await _context.SaveChangesAsync();
        }
    }
}

// TODO: таблицу order привязать к user + cart (cart_id)
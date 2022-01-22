﻿using Microsoft.AspNetCore.Http;
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
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        public void DeleteTableInCart(int id)
        {
            var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
            _cart.DeleteTableInCart(table);
        }

        [HttpPost]
        public void DeleteProductInCart(int id)
        {
            var foodItem = _context.FoodItems.FirstOrDefault(x => x.id == id);
            _cart.DeleteProductInCart(foodItem);
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
            var products = _cart.CartItems.Where(x => x.FoodItem != null).Select(x => x.FoodItem).ToList();
            order.poolTables.AddRange(tables);
            order.restaurantMenu.AddRange(products);
            _context.Orders.Add(order);
            _cart.DeleteAllItemsInCart();
            await _context.SaveChangesAsync();
        }
    }
}
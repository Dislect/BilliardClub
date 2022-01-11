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
            return PartialView();
        }

        [HttpPost]
        public void DeleteTableInCart(int id)
        {
            var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
            _cart.DeleteTableInCart(table);
        }
    }
}

// TODO: таблицу order привязать к user + cart (cart_id)
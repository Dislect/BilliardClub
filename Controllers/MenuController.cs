using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.Controllers
{
    public class MenuController : Controller
    {
        private readonly Context _context;
        private readonly Cart _cart;

        public MenuController(Context context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public async Task<ActionResult> Catalog()
        {
            return View(await _context.FoodItems.ToListAsync());
        }

        [HttpGet]
        public bool CheckCartOnThisItem(int productId)
        {
            return _cart.CartItems.Exists(x => x.FoodItem != null && x.FoodItem.id == productId);
        }

        [HttpPost]
        public async Task AddToCartProduct(int productId)
        {
            if (!CheckCartOnThisItem(productId))
            {
                var foodItem = _context.FoodItems.FirstOrDefault(x => x.id == productId);
                await _cart.AddToCartProduct(foodItem);
            }
        }
    }
}

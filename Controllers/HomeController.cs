using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using BilliardClub.View_Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        private readonly Cart _cart;

        public HomeController(Context context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public ActionResult Main()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Reservation()
        {
            _cart.CartItems = await _cart.GetCartItems();
            var model = new ReservationViewModel()
            {
                cart = _cart,
                poolTables = await _context.PoolTables
                    .Include(x => x.statusTables).ThenInclude(x => x.status)
                    .Include(x => x.typeTable)
                    .Include(x => x.tableRotation).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public void AddToCartTable(string id)
        {
            var table = _context.PoolTables.FirstOrDefault(x => x.id.ToString() == id);
            _cart.AddToCartTable(table);
        }

        [HttpGet]
        public async Task<bool> CheckingNumberTablesInCart()
        {
            return (await _cart.GetCartItems()).Count(x => x.PoolTable != null && x.cartItemId == _cart.cartId) < 2;
        }
    }
}

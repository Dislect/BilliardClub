using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
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
            return View( await _context.PoolTables
                .Include(x => x.statusTables)
                .Include(x => x.typeTable)
                .Include(x => x.tableRotation).ToListAsync());
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

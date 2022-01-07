using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using Microsoft.AspNetCore.Diagnostics;

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
        public ActionResult Reservation()
        {
            return View();
        }

        [HttpPost]
        public void AddToCartTable(string name)
        {
            var table = _context.PoolTables.FirstOrDefault(x => x.name == name);
            _cart.AddToCartTable(table);
        }
    }
}

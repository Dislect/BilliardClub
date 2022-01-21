using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(await _context.RestaurantMenus.ToListAsync());
        }
    }
}

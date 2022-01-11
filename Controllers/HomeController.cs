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

        public List<CartItem> CartItems { get; set; }

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
            return View(await InitModel());
        }

        [HttpPost]
        public void AddToCartTable(int id)
        {
            if (!_cart.CartItems.Exists(x => x.PoolTable.id == id))
            {
                var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
                var statusInCart = _context.Status.FirstOrDefault(x => x.id == 3);
                var statusTables = new StatusTable() { dateStart = DateTime.Now, status = statusInCart };

                _cart.AddToCartTable(table);
                table.statusTables.Add(statusTables); 
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public bool CheckingNumberTablesInCart()
        {
            return _cart.CartItems.Count(x => x.PoolTable != null && x.cartItemId == _cart.cartId) < 2;
        }

        public async Task<ReservationViewModel> InitModel()
        {
            var model = new ReservationViewModel()
            {
                cart = _cart,
                poolTables = await _context.PoolTables
                    .Include(x => x.statusTables).ThenInclude(x => x.status)
                    .Include(x => x.typeTable)
                    .Include(x => x.tableRotation).ToListAsync()
            };
            return model;
        }
    }
}

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
            return View(await InitModel());
        }

        [HttpPost]
        public async Task AddToCartTable(int id)
        {
            if (!_cart.CartItems.Exists(x => x.PoolTable != null && x.PoolTable.id == id))
            {
                var table = _context.PoolTables.FirstOrDefault(x => x.id == id);
                await _cart.AddToCartTable(table);
            }
        }

        [HttpGet]
        public bool CheckingNumberTablesInCart()
        {
            return _cart.CartItems.Count(x => x.PoolTable != null && x.cartId == _cart.cartId) < 2;
        }

        [HttpGet]
        public PoolTable GetTableInfo(int tableId)
        {
            var table = _context.PoolTables.First(x => x.id == tableId);
            table.idTableRotation = _context.TableRotations.First(x => x.poolTables.Contains(table)).id;
            table.idTypeTable = _context.TypeTables.First(x => x.poolTables.Contains(table)).id;
            return table;
        }

        [HttpPost]
        public void UpdatePoolTable(int tableId, int typeId, int rotationId, string number, int tableX, int tableY)
        {
            var table = _context.PoolTables.First(x => x.id == tableId);
            table.tableRotation = _context.TableRotations.First(x => x.id == rotationId);
            table.typeTable = _context.TypeTables.First(x => x.id == typeId);
            table.name = number;
            table.tableX = tableX;
            table.tableY = tableY;
            _context.SaveChanges();
        }

        public async Task<ReservationViewModel> InitModel()
        {
            var model = new ReservationViewModel()
            {
                cart = _cart,
                poolTables = await _context.PoolTables
                    .Include(x => x.statusTables).ThenInclude(x => x.status)
                    .Include(x => x.typeTable)
                    .Include(x => x.tableRotation).ToListAsync(),
                typeTables = await  _context.TypeTables.ToListAsync(),
                tableRotations = await _context.TableRotations.ToListAsync()
            };
            return model;
        }
    }
}

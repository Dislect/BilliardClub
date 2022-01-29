using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.Controllers
{
    [Authorize(Roles = "employee")]
    public class ManagementController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly Cart _cart;

        public ManagementController(Context context, UserManager<User> userManager, Cart cart)
        {
            _context = context;
            _userManager = userManager;
            _cart = cart;
        }

        [HttpGet]
        public ActionResult Сontrol()
        {
            var tables = 
                _context.PoolTables
                    .Include(x => x.statusTables).ThenInclude(x => x.status)
                    .Where(x => x.statusTables.Count != 0).ToList();
            return View(tables.Where(x => x.statusTables.Last().status.name == "Забронирован").ToList());
        }

        [HttpGet]
        public ActionResult _ReservedTables()
        {
            var tables =
                _context.PoolTables
                    .Include(x => x.statusTables).ThenInclude(x => x.status)
                    .Where(x => x.statusTables.Count != 0).ToList();
            return PartialView(tables.Where(x => x.statusTables.Last().status.name == "Забронирован").ToList());
        }

        [HttpPost]
        public void CancelReservation(int tableId)
        {
            if (_context.PoolTables.FirstOrDefault(x => x.id == tableId) != null)
            {
                var table = _context.PoolTables.Include(x => x.statusTables).ThenInclude(x => x.status).First(x => x.id == tableId);
                if (table.statusTables.Last().status.name == "Забронирован")
                {
                    var statusFree = _context.Status.First(x => x.name == "Свободен");
                    table.statusTables.Last().dateEnd = DateTime.Now;
                    table.statusTables.Add(new StatusTable() {dateStart = DateTime.Now, status = statusFree});
                    _context.SaveChanges();
                }
            }
        }
    }
}

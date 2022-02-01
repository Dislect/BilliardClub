using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using BilliardClub.View_Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task AddToCartTable(int id)
        {
            if (!_cart.CartItems.Exists(x => x.PoolTable != null && x.PoolTable.id == id))
            {
                var table = _context.PoolTables.First(x => x.id == id);
                await _cart.AddToCartTable(table);
            }
        }

        [HttpGet]
        [Authorize]
        public bool CheckingNumberTablesInCart()
        {
            return _cart.CartItems.Count(x => x.PoolTable != null && x.cartId == _cart.cartId) < 2;
        }

        [HttpGet]
        [Authorize(Roles = "employee")]
        public PoolTable GetTableInfo(int tableId)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .First(x => x.id == tableId);

            table.idTableRotation = _context.TableRotations.First(x => x.poolTables.Contains(table)).id;
            table.idTypeTable = _context.TypeTables.First(x => x.poolTables.Contains(table)).id;

            var status = table.statusTables.LastOrDefault();

            if (status != null)
                table.idStatus = _context.Status.First(x => x.name == status.status.name).id;
            else
                table.idStatus = -1;

            return table;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public void UpdatePoolTable(int tableId, int typeId, int rotationId, string number, int tableX, int tableY, int statusId)
        {
            var table = _context.PoolTables.First(x => x.id == tableId);
            table.tableRotation = _context.TableRotations.First(x => x.id == rotationId);
            table.typeTable = _context.TypeTables.First(x => x.id == typeId);

            if (_context.Status.FirstOrDefault(x => x.id == statusId) != null)
            {
                var status = _context.Status.First(x => x.id == statusId);
                if (table.statusTables.LastOrDefault() != null)
                {
                    table.statusTables.Last().dateEnd = DateTime.Now;
                }
                table.statusTables.Add(new StatusTable() { dateStart = DateTime.Now, status = status });
            }

            table.name = number;
            table.tableX = tableX;
            table.tableY = tableY;

            _context.SaveChanges();
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public void DeletePoolTableInPlan(int tableId)
        {
            var table = _context.PoolTables.FirstOrDefault(x => x.id == tableId);
            if (table != null)
            {
                _context.PoolTables.Remove(table);
                _context.SaveChanges();
            }
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public void AddPoolTableInPlan()
        {
            _context.PoolTables.Add( new PoolTable()
            {
                name = "new",
                tableX = 710,
                tableY = 275,
                typeTable = _context.TypeTables.FirstOrDefault(),
                tableRotation = _context.TableRotations.FirstOrDefault()
            });
            _context.SaveChanges();
        }

        private async Task<ReservationViewModel> InitModel()
        {
            var model = new ReservationViewModel()
            {
                cart = _cart,
                poolTables = await _context.PoolTables
                    .Include(x => x.statusTables).ThenInclude(x => x.status)
                    .Include(x => x.typeTable)
                    .Include(x => x.tableRotation).ToListAsync(),
                typeTables = await  _context.TypeTables.ToListAsync(),
                tableRotations = await _context.TableRotations.ToListAsync(),
                statusList = await _context.Status.ToListAsync()
            };

            return model;
        }
    }
}

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

        #region Reservation

        [HttpGet]
        public async Task<ActionResult> Reservation()
        {
            return View(await InitModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCartTable(int id, DateTime? dateReservation)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .FirstOrDefault(x => x.id == id);

            if (table == null)
            {
                return BadRequest("Не найден стол для бронирования");
            }

            if (_cart.CartPoolTables.Exists(x => x.PoolTable != null && x.PoolTable.id == id))
            {
                return BadRequest("Стол уже находится в корзине");
            }

            if (table.statusTables.LastOrDefault()?.status.name == "Забронирован")
            {
                return BadRequest("Данный стол уже забронирован");
            }

            if (table.statusTables.LastOrDefault()?.status.name == "В корзине")
            {
                return BadRequest("Данный стол уже кто-то заказывает");
            }

            await _cart.AddToCartTable(table, dateReservation ?? DateTime.Now);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult CheckingNumberTablesInCart()
        {
            int maxCountTablesInCart = 2;
            if (_cart.CartPoolTables.Count(x => x.PoolTable != null && x.cartId == _cart.cartId) < maxCountTablesInCart)
            {
                return Ok();
            }
            return BadRequest("Пользователь может забронировать не более 2-х столов за раз!");
        }

        #endregion

        #region Administration

        [HttpGet]
        [Authorize(Roles = "employee")]
        public IActionResult GetTableInfo(int tableId)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .FirstOrDefault(x => x.id == tableId);

            if (table != null)
            {
                table.idTableRotation = _context.TableRotations.First(x => x.poolTables.Contains(table)).id;
                table.idTypeTable = _context.TypeTables.First(x => x.poolTables.Contains(table)).id;

                var status = table.statusTables.LastOrDefault();

                if (status != null)
                    table.idStatus = _context.Status.First(x => x.name == status.status.name).id;
                else
                    table.idStatus = -1;

                return Ok(table);
            }

            return BadRequest("Не найден бильярдный стол!");
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult UpdatePoolTable(int tableId, int typeId, int rotationId, string number, int tableX, int tableY, int statusId)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .FirstOrDefault(x => x.id == tableId);

            if (table != null)
            {
                var rotation = _context.TableRotations.FirstOrDefault(x => x.id == rotationId);
                if (rotation is null)
                {
                    return BadRequest("Не найден тип вращения");
                }
                table.tableRotation = rotation;

                var typeTable = _context.TypeTables.FirstOrDefault(x => x.id == typeId);
                if (typeTable is null)
                {
                    return BadRequest("Не найден тип стола");
                }
                table.typeTable = typeTable;

                var status = _context.Status.FirstOrDefault(x => x.id == statusId);
                if (status != null)
                {
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
                return Ok();
            }

            return BadRequest("Не найден бильярдный стол для изменения!");
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult DeletePoolTableInPlan(int tableId)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .Include(x => x.orders)
                .FirstOrDefault(x => x.id == tableId);

            if (table != null)
            {
                _context.PoolTables.Remove(table);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest("Не найден стол для удаления!");
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public void AddPoolTableInPlan()
        {
            _context.PoolTables.Add(new PoolTable()
            {
                name = "new",
                tableX = 710,
                tableY = 275,
                typeTable = _context.TypeTables.FirstOrDefault(),
                tableRotation = _context.TableRotations.FirstOrDefault()
            });
            _context.SaveChanges();
        }

        #endregion

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

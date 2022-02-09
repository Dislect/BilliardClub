using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using BilliardClub.View_Models;
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
            var tables = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .Where(x => x.statusTables.Any())
                .ToList();

            return View(tables.Where(x => x.statusTables.Last().status.name == "Забронирован").ToList());
        }

        #region Reserved tables

        [HttpGet]
        public ActionResult _ReservedTables()
        {
            var tables = _context.PoolTables
                .Include(x => x.statusTables).ThenInclude(x => x.status)
                .Where(x => x.statusTables.Any()).ToList();

            return PartialView(tables.Where(x => x.statusTables.Last().status.name == "Забронирован").ToList());
        }

        [HttpPost]
        public void CancelReservation(int tableId)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .FirstOrDefault(x => x.id == tableId);

            if (table != null
                && table.statusTables.Any()
                && table.statusTables.Last().status.name == "Забронирован")
            {
                var statusFree = _context.Status.First(x => x.name == "Свободен");

                table.statusTables.Last().dateEnd = DateTime.Now;
                table.statusTables.Add(new StatusTable() {dateStart = DateTime.Now, status = statusFree});

                _context.SaveChanges();
            }
        }

        #endregion

        #region Types tables and rotation

        public async Task<ActionResult> _TypesTablesAndRotation()
        {
            var model = new TypeTableAndRotationViewModel()
            {
                typeTables = await _context.TypeTables.ToListAsync(),
                tableRotations = await _context.TableRotations.ToListAsync()
            };
            return PartialView(model);
        }

        public void AddTypeTable(string nameType, uint price)
        {
            if (!_context.TypeTables.Any(x => x.name == nameType))
            {
                _context.TypeTables.Add(new TypeTable()
                {
                    name = nameType,
                    price = price
                });
                _context.SaveChanges();
            }
        }

        public void ChangeTypeTable(int idTypeTable, string newNameType, uint newPrice)
        {
            var typeTable = _context.TypeTables.FirstOrDefault(x => x.id == idTypeTable);
            if (typeTable != null)
            {
                typeTable.name = newNameType;
                typeTable.price = newPrice;

                _context.SaveChanges();
            }
        }

        public void DeleteTypeTable(int idTypeTable)
        {
            var typeTable = _context.TypeTables.FirstOrDefault(x => x.id == idTypeTable);
            if (typeTable != null)
            {
                _context.TypeTables.Remove(typeTable);
                _context.SaveChanges();
            }
        }

        public void AddTableRotation(int angle)
        {
            if (!_context.TableRotations.Any(x => x.rotationAngle == angle))
            {
                _context.TableRotations.Add(new TableRotation()
                {
                    rotationAngle = angle
                });

                _context.SaveChanges();
            }
        }

        public void ChangeTableRotation(int idTableRotation, int newAngle)
        {
            var tableRotation = _context.TableRotations.FirstOrDefault(x => x.id == idTableRotation);
            if (tableRotation != null)
            {
                tableRotation.rotationAngle = newAngle;

                _context.SaveChanges();
            }
        }

        public void DeleteTableRotation(int idTableRotation)
        {
            var tableRotation = _context.TableRotations.FirstOrDefault(x => x.id == idTableRotation);
            if (tableRotation != null)
            {
                _context.TableRotations.Remove(tableRotation);

                _context.SaveChanges();
            }
        }

        #endregion

        #region Tables status history

        public ActionResult _TablesStatusHistory()
        {
            var model = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .ToList();

            return PartialView(model);
        }

        public void AddStatus(int idTable, int idStatus)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .FirstOrDefault(x => x.id == idTable);

            var status = _context.Status.FirstOrDefault(x => x.id == idStatus);

            if (table != null && status != null)
            {
                if (table.statusTables.Any())
                {
                    table.statusTables.Last().dateEnd = DateTime.Now;
                }

                table.statusTables.Add(new StatusTable()
                {
                    dateStart = DateTime.Now,
                    status = status
                });
            }
        }

        public void ChangeStatus(int idStatusTable, int idStatus, DateTime? dateStart, DateTime? dateEnd)
        {
            var statusTable = _context.StatusTables.FirstOrDefault(x => x.id == idStatusTable);

            if (statusTable != null)
            {
                var status = _context.Status.FirstOrDefault(x => x.id == idStatus);

                if (status != null)
                {
                    statusTable.status = status;
                }

                if (dateStart != null)
                {
                    statusTable.dateStart = dateStart.Value;
                }
                
                if (dateEnd != null)
                {
                    statusTable.dateEnd = dateEnd;
                }
                _context.SaveChanges();
            }
        }

        public void DeleteStatus(int idStatusTable)
        {
            var statusTable = _context.StatusTables.FirstOrDefault(x => x.id == idStatusTable);

            if (statusTable != null)
            {
                _context.StatusTables.Remove(statusTable);
                _context.SaveChanges();
            }
        }

        #endregion

        #region User orders

        public ActionResult _UserOrders()
        {
            var model = _context.Users
                .Include(x => x.orders)
                .ThenInclude(x => x.poolTables)
                .ThenInclude(x => x.poolTable)
                .ThenInclude(x => x.typeTable)
                .Include(x => x.orders)
                .ThenInclude(x => x.foodItems)
                .ToList();

            return PartialView(model);
        }

        public void AddOrderToUser(string idUser, int[] idPooltables, int[] idFoodItems, DateTime orderDate)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == idUser);

            if (user != null)
            {
                var order = new Order()
                {
                    user = user,
                    orderDate = orderDate
                    // TODO
                };
            }
        }

        public void ChangeOrderToUser()
        {

        }

        public void DeleteOrderToUser()
        {

        }

        #endregion

    }
}

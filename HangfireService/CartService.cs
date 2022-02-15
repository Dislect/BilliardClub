using System;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BilliardClub.HangfireService
{
    public class CartService
    {
        private Context _context;
        private static CartService _cartService;

        public static CartService Instance => _cartService ??= new CartService();

        public static CartService GetCartService(IServiceProvider services)
        {
            var context = services.GetService<Context>();
            _cartService = new CartService()
            {
                _context = context
            };

            return _cartService;
        }

        public async Task DeleteTableInCartJob(int tableId, string cartId)
        {
            var table = _context.PoolTables
                .Include(x => x.statusTables)
                .ThenInclude(x => x.status)
                .FirstOrDefault(x => x.id == tableId);

            if (table == null)
            {
                return;
            }

            if (_context.CartPoolTables.Include(x => x.PoolTable)
                .Any(x => x.PoolTable != null && x.PoolTable.id == table.id && cartId == x.cartId))
            {
                _context.CartPoolTables.Remove(
                    _context.CartPoolTables.First(x => x.PoolTable.id == table.id && cartId == x.cartId));

                var statusFree = _context.Status.First(x => x.name == "Свободен");

                if (table.statusTables.Any())
                {
                    table.statusTables.Last().dateEnd = DateTime.Now;
                }

                table.statusTables.Add(new StatusTable() { dateStart = DateTime.Now, status = statusFree });
                await _context.SaveChangesAsync();
            }
        }
    }
}
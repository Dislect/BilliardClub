using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BilliardClub.Models
{
    public class Cart
    {
        private readonly Context _context;
        public static Cart cart;

        public string cartId { get; set; }

        public List<CartPoolTable> CartPoolTables  => GetCartPoolTables().Result;
        public List<CartFoodItem> CartFoodItems => GetCartFoodItems().Result;

        public Cart(Context context)
        {
            this._context = context;
        }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<Context>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            cart = new Cart(context) {cartId = cartId};

            return cart;
        }

        public async Task AddToCartTable(PoolTable table, DateTime dateReservation)
        {
            var statusInCart = _context.Status.FirstOrDefault(x => x.name == "В корзине");

            if (table.statusTables.Any())
            {
                table.statusTables.Last().dateEnd = DateTime.Now;
            }
            table.statusTables.Add(new StatusTable() { dateStart = dateReservation, status = statusInCart });

            _context.CartPoolTables.Add(new CartPoolTable()
            {
                cartId = cartId,
                PoolTable = table,
                numberHours = 1,
                reservationDate = dateReservation
            });
            await _context.SaveChangesAsync();

           await DeleteTableInCartTask(table);
        }

        private async Task DeleteTableInCartTask(PoolTable table)
        {
            await Task.Delay(new TimeSpan(0,1,0));

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

                table.statusTables.Add(new StatusTable() {dateStart = DateTime.Now, status = statusFree});
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddToCartProduct(FoodItem product)
        {
            _context.CartFoodItems.Add(new CartFoodItem()
            {
                cartId = cartId,
                FoodItem = product,
                quantity = 1
            });
            await _context.SaveChangesAsync();
        }

        public void DeleteTableInCart(PoolTable table)
        {
            _context.CartPoolTables.Remove(CartPoolTables.First(x => x.PoolTable != null && x.PoolTable.id == table.id));
            var statusFree = _context.Status.First(x => x.name == "Свободен");

            if (table.statusTables.Any())
            {
                table.statusTables.Last().dateEnd = DateTime.Now;
            }

            table.statusTables.Add(new StatusTable() { dateStart = DateTime.Now, status = statusFree });
            _context.SaveChanges();
        }

        public void DeleteProductInCart(FoodItem foodItem)
        {
            _context.CartFoodItems.Remove(CartFoodItems.First(x => x.FoodItem.id == foodItem.id));
            _context.SaveChanges();
        }

        public void DeleteAllItemsInCart()
        {
            _context.CartPoolTables.RemoveRange(CartPoolTables);
            _context.CartFoodItems.RemoveRange(CartFoodItems);
            _context.SaveChanges();
        }

        private async Task<List<CartPoolTable>> GetCartPoolTables()
        {
            return await _context.CartPoolTables.Where(item => item.cartId == cartId)
                .Include(x => x.PoolTable).ThenInclude(x => x.statusTables)
                .Include(x => x.PoolTable).ThenInclude(x => x.typeTable)
                .ToListAsync();
        }

        private async Task<List<CartFoodItem>> GetCartFoodItems()
        {
            return await _context.CartFoodItems.Where(item => item.cartId == cartId)
                .Include(x => x.FoodItem)
                .ToListAsync();
        }
    }
}
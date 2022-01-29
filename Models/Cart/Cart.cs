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

        public List<CartItem> CartItems => GetCartItems().Result;

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

        public Task AddToCartTable(PoolTable table)
        {
            var statusInCart = _context.Status.FirstOrDefault(x => x.name == "В корзине");
            if (table.statusTables.Count != 0)
            {
                table.statusTables.Last().dateEnd = DateTime.Now;
            }
            table.statusTables.Add(new StatusTable() { dateStart = DateTime.Now, status = statusInCart });
            _context.CartItems.Add(new CartItem()
            {
                cartId = cartId,
                PoolTable = table,
                quantity = 1
            });
            _context.SaveChanges();

            // удаление стола из корзины через некоторое время и обновление статуса стола
            var deleteTableInCartTask = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(60000);
                if (_context.CartItems.Include(x => x.PoolTable).ToList().Exists(x => x.PoolTable != null && x.PoolTable.id == table.id && cartId == x.cartId))
                {
                    _context.CartItems.Remove(_context.CartItems.First(x => x.PoolTable.id == table.id && cartId == x.cartId));
                }
                var statusFree = _context.Status.First(x => x.name == "Свободен");
                if (table.statusTables.Count != 0)
                {
                    table.statusTables.Last().dateEnd = DateTime.Now;
                }
                table.statusTables.Add(new StatusTable(){dateStart = DateTime.Now, status = statusFree});
                _context.SaveChanges();
            });
            return deleteTableInCartTask;
        }

        public async Task AddToCartProduct(FoodItem product)
        {
            _context.CartItems.Add(new CartItem()
            {
                cartId = cartId,
                FoodItem = product,
                quantity = 1
            });
            await _context.SaveChangesAsync();
        }

        public void DeleteTableInCart(PoolTable table)
        {
            _context.CartItems.Remove(CartItems.First(x => x.PoolTable != null && x.PoolTable.id == table.id));
            var statusFree = _context.Status.First(x => x.name == "Свободен");
            if (table.statusTables.Count != 0)
            {
                table.statusTables.Last().dateEnd = DateTime.Now;
            }
            table.statusTables.Add(new StatusTable() { dateStart = DateTime.Now, status = statusFree });
            _context.SaveChanges();
        }

        public void DeleteProductInCart(FoodItem foodItem)
        {
            _context.CartItems.Remove(CartItems.First(x => x.FoodItem.id == foodItem.id));
            _context.SaveChanges();
        }

        public void DeleteAllItemsInCart()
        {
            _context.CartItems.RemoveRange(CartItems);
            _context.SaveChanges();
        }

        private async Task<List<CartItem>> GetCartItems()
        {
            return await _context.CartItems.Where(item => item.cartId == cartId)
                .Include(x => x.PoolTable).ThenInclude(x => x.statusTables)
                .Include(x => x.PoolTable).ThenInclude(x => x.typeTable)
                .Include(x => x.FoodItem).ToListAsync();
        }
    }
}
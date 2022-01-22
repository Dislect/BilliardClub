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
            var statusInCart = _context.Status.FirstOrDefault(x => x.id == 3); // "3 = В корзине"
            var statusTables = new StatusTable() { dateStart = DateTime.Now, status = statusInCart };
            table.statusTables.Add(statusTables);
            _context.CartItems.Add(new CartItem()
            {
                cartItemId = cartId,
                PoolTable = table
            });
            _context.SaveChanges();

            // удаление стола из корзины через некоторое время
            var deleteTableInCartTask = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(60000);
                _context.CartItems.Remove(_context.CartItems.First(x => x.PoolTable.id == table.id && cartId == x.cartItemId));
                _context.StatusTables.Remove(_context.StatusTables.First(x => x.id == statusTables.id));
                _context.SaveChanges();
            });
            return deleteTableInCartTask;
        }

        public async Task AddToCartProduct(FoodItem product)
        {
            _context.CartItems.Add(new CartItem()
            {
                cartItemId = cartId,
                FoodItem = product
            });
            await _context.SaveChangesAsync();
        }

        public void DeleteTableInCart(PoolTable table)
        {
            _context.CartItems.Remove(CartItems.First(x => x.PoolTable.id == table.id));
            _context.StatusTables.Remove(_context.StatusTables.Include(x => x.poolTable).Include(x => x.status)
                .First(x => x.poolTable.id == table.id && x.status.id == 3));
            _context.SaveChanges();
        }

        public void DeleteProductInCart(FoodItem foodItem)
        {
            _context.CartItems.Remove(CartItems.First(x => x.FoodItem.id == foodItem.id));
            _context.SaveChanges();
        }

        public void DeleteAllItemsInCart()
        {
            var tables = cart.CartItems.Where(x => x.PoolTable != null).Select(x => x.PoolTable).ToList();
            foreach (var table in tables)
            {
                DeleteTableInCart(table);
            }
            _context.CartItems.RemoveRange(CartItems);
            _context.SaveChanges();
        }

        private async Task<List<CartItem>> GetCartItems()
        {
            return await _context.CartItems.Where(item => item.cartItemId == cartId)
                .Include(x => x.PoolTable).ThenInclude(x => x.typeTable)
                .Include(x => x.FoodItem).ToListAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public void AddToCartTable(PoolTable table)
        {
            _context.CartItems.Add(new CartItem()
            {
                cartItemId = cartId,
                PoolTable = table
            }); 
            _context.SaveChanges();
        }

        public void AddToCartProduct(RestaurantMenu product)
        {
            _context.CartItems.Add(new CartItem()
            {
                cartItemId = cartId,
                RestaurantMenu = product
            });
            _context.SaveChanges();
        }

        public void DeleteTableInCart(PoolTable table)
        {
            _context.CartItems.Remove(CartItems.First(x => x.PoolTable.id == table.id));
            _context.StatusTables.Remove(_context.StatusTables.Include(x => x.poolTable).Include(x => x.status)
                .First(x => x.poolTable.id == table.id && x.status.id == 3));
            _context.SaveChanges();
        }

        public void DeleteProductInCart(RestaurantMenu restaurantMenu)
        {
            _context.CartItems.Remove(CartItems.First(x => x.id == restaurantMenu.id));
            _context.SaveChanges();
        }

        private async Task<List<CartItem>> GetCartItems()
        {
            return await _context.CartItems.Where(item => item.cartItemId == cartId)
                .Include(x => x.PoolTable).ThenInclude(x => x.typeTable)
                .Include(x => x.RestaurantMenu).ToListAsync();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.Controllers
{
    public class MenuController : Controller
    {
        private readonly Context _context;
        private readonly Cart _cart;

        public MenuController(Context context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public async Task<ActionResult> Catalog()
        {
            return View(await _context.FoodItems.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public bool CheckCartOnThisItem(int productId)
        {
            return _cart.CartItems.Exists(x => x.FoodItem != null && x.FoodItem.id == productId);
        }

        [HttpPost]
        [Authorize]
        public async Task AddToCartProduct(int productId)
        {
            var foodItem = _context.FoodItems.FirstOrDefault(x => x.id == productId);

            if (foodItem != null
                && !CheckCartOnThisItem(productId))
            {
                await _cart.AddToCartProduct(foodItem);
            }
        }
    }
}

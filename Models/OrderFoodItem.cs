using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class OrderFoodItem
    {
        public int id { get; set; }
        public FoodItem foodItem { get; set; }
        public Order order { get; set; }
        public uint quantity { get; set; }
    }
}
using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class FoodItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public uint price { get; set; }
        public string picturePath { get; set; }
        public List<OrderFoodItem> orders { get; set; } = new();
    }
}
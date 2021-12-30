using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class RestaurantMenu
    {
        public int id { get; set; }
        public string title { get; set; }
        public uint price { get; set; }
        public List<RestaurantOrderHistory> RestaurantOrderHistories { get; set; } = new();
    }
}
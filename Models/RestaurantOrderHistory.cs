using System;

namespace BilliardClub.Models
{
    public class RestaurantOrderHistory
    {
        public int id { get; set; }
        public DateTime dateOrder { get; set; }
        public RestaurantMenu restaurantMenuItem { get; set; }
        public User user { get; set; }
    }
}
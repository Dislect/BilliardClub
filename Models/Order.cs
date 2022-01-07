using System;
using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class Order
    {
        public int id { get; set; }
        public DateTime orderDate { get; set; }
        public double cheque { get; set; }
        public User user { get; set; }
        public List<RestaurantMenu> restaurantMenu { get; set; } = new();
        public List<PoolTable> poolTables { get; set; } = new();
    }
}
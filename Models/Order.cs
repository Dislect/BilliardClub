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
        public List<OrderFoodItem> foodItems { get; set; } = new();
        public List<OrderPoolTable> poolTables { get; set; } = new();
    }
}
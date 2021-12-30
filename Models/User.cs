using System;
using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string firstNane { get; set; }
        public string lastNane { get; set; }
        public string email { get; set; }
        public DateTime BirthDate { get; set; }
        public string passwordHash { get; set; }
        public List<UserRole> userRoles { get; set; } = new();
        public List<RestaurantOrderHistory> restaurantOrderHistories { get; set; } = new();
        public List<RentalHistory> rentalHistories { get; set; } = new();
    }
}
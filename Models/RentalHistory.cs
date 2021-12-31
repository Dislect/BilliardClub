using System;

namespace BilliardClub.Models
{
    public class RentalHistory
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public DateTime rentalTime { get; set; }
        public User user { get; set; }
        public PoolTable poolTable { get; set; }
    }
}
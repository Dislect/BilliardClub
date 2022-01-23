using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class OrderPoolTable
    {
        public int id { get; set; }
        public PoolTable poolTable { get; set; }
        public Order order { get; set; }
        public uint quantity { get; set; }
    }
}
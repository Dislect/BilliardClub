using System;

namespace BilliardClub.Models
{
    public class CartPoolTable
    {
        public int id { get; set; }
        public PoolTable PoolTable { get; set; }
        public uint numberHours { get; set; }
        public DateTime reservationDate { get; set; }
        public string cartId { get; set; }
    }
}

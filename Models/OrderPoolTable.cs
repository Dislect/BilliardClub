using System.ComponentModel.DataAnnotations;

namespace BilliardClub.Models
{
    public class OrderPoolTable
    {
        public int id { get; set; }
        [Required]
        public PoolTable poolTable { get; set; }
        [Required]
        public Order order { get; set; }
        public uint numberHours { get; set; }
    }
}
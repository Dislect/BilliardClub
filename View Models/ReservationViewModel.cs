using System.Collections.Generic;
using BilliardClub.Models;

namespace BilliardClub.View_Models
{
    public class ReservationViewModel
    {
        public Cart cart { get; set; }
        public List<PoolTable> poolTables { get; set; }
    }
}
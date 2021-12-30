using System;

namespace BilliardClub.Models
{
    public class StatusTable
    {
        public int id { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int? idPoolTable { get; set; }
        public int? idStatus { get; set; }
        public PoolTable poolTable { get; set; }
        public Status status { get; set; }
    }
}
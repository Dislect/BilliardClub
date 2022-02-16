using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilliardClub.Models
{
    public class StatusTable
    {
        public int id { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public PoolTable poolTable { get; set; }
        public Status status { get; set; }

        [NotMapped]
        public int? idPoolTable { get; set; }
        [NotMapped]
        public int? idStatus { get; set; }
    }
}
using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
        List<StatusTable> statusTables { get; set; }
    }
}
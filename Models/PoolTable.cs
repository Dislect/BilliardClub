using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class PoolTable
    {
        public int id { get; set; }
        public string name { get; set; }
        public int tableX { get; set; }
        public int tableY { get; set; }
        public int? idTypeTable { get; set; }
        public int? idTableRotation { get; set; }
        public TypeTable typeTable { get; set; }
        public TableRotation tableRotation { get; set; }
        public List<OrderPoolTable> orders { get; set; } = new();
        public List<StatusTable> statusTables { get; set; } = new();
    }
}
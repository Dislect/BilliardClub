using System.Collections.Generic;

namespace BilliardClub.Models
{
    public abstract class TypeTable
    {
        public int id { get; set; }
        public string name { get; set; }
        public uint price { get; set; }
        public List<PoolTable> poolTables { get; set; }
    }
}
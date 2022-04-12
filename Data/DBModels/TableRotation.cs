using System.Collections.Generic;

namespace BilliardClub.Models
{
    public abstract class TableRotation
    {
        public int id { get; set; }
        public int rotationAngle { get; set; }
        public List<PoolTable> poolTables { get; set;}
    }
}
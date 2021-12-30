using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class TableRotation
    {
        public int id { get; set; }
        public int rotationAngle { get; set; }
        public List<PoolTable> poolTables { get; set; }
    }
}
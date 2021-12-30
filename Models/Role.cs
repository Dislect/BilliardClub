using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class Role
    {
        public int id { get; set; }
        public string nane { get; set; }
        public List<UserRole> userRoles { get; set; } = new();
    }
}
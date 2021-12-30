using System;
using System.Collections.Generic;

namespace BilliardClub.Models
{
    public class UserRole
    {
        public int id { get; set; }
        public DateTime dateAddRole { get; set; }
        public int? idRole { get; set; }
        public int? idUser { get; set; }
        public User user { get; set; }
        public Role role { get; set; }
        public List<UserRole> userRoles { get; set; } = new();
    }
}
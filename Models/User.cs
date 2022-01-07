using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BilliardClub.Models
{
    public class User : IdentityUser
    {
        [MaxLength(32)]
        public string firstName { get; set; }

        [MaxLength(32)]
        public string lastName { get; set; }

        public DateTime BirthDate { get; set; }

        public List<Order> orders { get; set; }
    }
}
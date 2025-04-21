using System;
using System.Collections.Generic;

namespace ElectronicShop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } = "User";
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Quan hệ
        public virtual ICollection<Order> Orders { get; set; }
    }
}

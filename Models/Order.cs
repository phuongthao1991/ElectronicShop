using System;
using System.Collections.Generic;

namespace ElectronicShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }

        // Quan hệ
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

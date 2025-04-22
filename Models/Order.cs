using ElectronicShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicShop.Models
{
    //{
    //    public class Order
    //    {
    //        public int Id { get; set; }
    //        public int UserId { get; set; }
    //        public decimal TotalPrice { get; set; }
    //        public string Status { get; set; } = "Pending";
    //        public DateTime? OrderDate { get; set; } = DateTime.Now;
    //        public string PaymentMethod { get; set; }

    //        // Quan hệ
    //        public virtual User User { get; set; }
    //        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    //    }
    //}


    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}


//namespace ElectronicShop.Models
//{
//    public class OrderDetail
//    {
//        public int Id { get; set; }
//        public int OrderId { get; set; }
//        public int ProductId { get; set; }
//        public int Quantity { get; set; }
//        public decimal Price { get; set; }

//        // Quan hệ
//        public virtual Order Order { get; set; }
//        public virtual Product Product { get; set; }
//    }
//}
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }

        // Quan hệ
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Quan hệ
        public virtual ICollection<Product> Products { get; set; }
    }
}

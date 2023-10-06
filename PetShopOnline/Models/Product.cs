using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Pictures = new HashSet<PictureProduct1>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public int? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool Discontinued { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<PictureProduct1> Pictures { get; set; }
    }
}

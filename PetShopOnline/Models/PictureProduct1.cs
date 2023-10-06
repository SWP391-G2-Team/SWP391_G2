using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class PictureProduct1
    {
        public PictureProduct1()
        {
            Products = new HashSet<Product>();
        }

        public int PictureId { get; set; }
        public string? Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

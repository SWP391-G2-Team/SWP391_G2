using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class Gallery
    {
        public Gallery()
        {
            PictureGalleries = new HashSet<PictureGallery>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<PictureGallery> PictureGalleries { get; set; }
    }
}

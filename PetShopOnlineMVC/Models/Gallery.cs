using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class Gallery
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<PictureGallery> PictureGalleries { get; set; } = new List<PictureGallery>();
}

using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class PictureProduct1
{
    public int PictureId { get; set; }

    public string? Picture { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

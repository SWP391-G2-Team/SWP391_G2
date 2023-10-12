using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class PictureGallery
{
    public int PicId { get; set; }

    public int? Id { get; set; }

    public string? Caption { get; set; }

    public string? Picture { get; set; }

    public virtual Gallery? IdNavigation { get; set; }
}

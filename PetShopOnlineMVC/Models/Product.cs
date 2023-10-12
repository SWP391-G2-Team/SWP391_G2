using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class Product
{
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

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PictureProduct1> Pictures { get; set; } = new List<PictureProduct1>();
}

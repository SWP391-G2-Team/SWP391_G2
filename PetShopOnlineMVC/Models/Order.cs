using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string CustomerId { get; set; } = null!;

    public string? EmployeeId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public string? OrderStatus { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ReasonCancel> ReasonCancels { get; set; } = new List<ReasonCancel>();

    public virtual ICollection<Ship> Ships { get; set; } = new List<Ship>();
}

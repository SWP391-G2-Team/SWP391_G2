using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class ReasonCancel
{
    public int ReasonId { get; set; }

    public int? OrderId { get; set; }

    public string? Reason { get; set; }

    public byte StatusCancel { get; set; }

    public virtual Order? Order { get; set; }
}

using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? CustomerId { get; set; }

    public string? EmployeeId { get; set; }

    public int? Role { get; set; }

    public bool? IsActive { get; set; }

    public int? ShipperId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Shipper? Shipper { get; set; }
}

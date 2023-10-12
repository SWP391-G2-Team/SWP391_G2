using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public string? Phone { get; set; }

    public string? Name { get; set; }

    public string? District { get; set; }

    public string? Province { get; set; }

    public string? Wards { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

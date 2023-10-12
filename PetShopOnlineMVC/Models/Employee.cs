using System;
using System.Collections.Generic;

namespace PetShopOnlineMVC.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? FirstName { get; set; }

    public int? DepartmentId { get; set; }

    public string? Title { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? HireDate { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

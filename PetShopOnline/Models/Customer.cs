using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Orders = new HashSet<Order>();
        }

        public string CustomerId { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? Wards { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ReasonCancels = new HashSet<ReasonCancel>();
            Ships = new HashSet<Ship>();
        }

        public int OrderId { get; set; }
        public string CustomerId { get; set; } = null!;
        public string? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? OrderStatus { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ReasonCancel> ReasonCancels { get; set; }
        public virtual ICollection<Ship> Ships { get; set; }
    }
}

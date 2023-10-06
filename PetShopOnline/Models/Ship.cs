using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class Ship
    {
        public int ShipId { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipContact { get; set; }
        public decimal? Freight { get; set; }
        public int? ShipperId { get; set; }
        public int? OrderId { get; set; }
        public string? PhoneContact { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}

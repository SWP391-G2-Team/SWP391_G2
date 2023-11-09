
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetShopOnline.Pages.Shipper.OrderShipper
{
    [Authorize(Roles = "3")]
    public class OrderListModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        public OrderListModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public Models.Account Account { get; set; }
        public Models.Shipper Shipper { get; private set; }

        [BindProperty]
        public List<ShipViewModel> Ships { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
            Shipper = dBContext.Shippers.FirstOrDefault(c => c.ShipperId == Account.ShipperId);

            IQueryable<Ship> query = dBContext.Ships
               .Include(s => s.Order)
                   .ThenInclude(o => o.OrderDetails)
                       .ThenInclude(od => od.Product);

            List<Ship> displayedShips = await query.ToListAsync();

            Ships = new List<ShipViewModel>();
            foreach (var ship in displayedShips)
            {
                var orderDetails = ship.Order?.OrderDetails;
                var productNames = orderDetails != null ? string.Join(", ", orderDetails.Select(od => od.Product.ProductName)) : "";

                ShipViewModel shipViewModel = new ShipViewModel
                {
                    ShipId = ship.ShipId,
                    OrderId = (int)ship.OrderId,    
                    ProductNames = productNames,
                    Address = ship.ShipAddress,
                    PhoneContact = ship.PhoneContact,
                    ShipContact = ship.ShipContact,
                    OrderStatus = ship.Order.OrderStatus
                };
                if (ship.Order.OrderStatus.Equals("Future") || ship.Order.OrderStatus.Equals("Complete"))
                {
                    Ships.Add(shipViewModel);
                } 
            }

            return Page();
        }
    }

    public class ShipViewModel
    {
        public int ShipId { get; set; }
        public int OrderId { get; set; }
        public string ProductNames { get; set; }
        public string Address { get; set; }
        public string PhoneContact { get; set; }
        public string ShipContact { get; set; }
        public string OrderStatus { get; set; }
    }
}



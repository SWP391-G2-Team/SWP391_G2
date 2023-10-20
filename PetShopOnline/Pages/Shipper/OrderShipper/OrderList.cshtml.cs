/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetShop_Project_SWP391.Pages.Shipper.OrderShipper
{
    [Authorize(Roles = "3")]
    public class OrderListModel : PageModel
    {
        private readonly ProjectContext dBContext;
        public OrderListModel(ProjectContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public Models.Account Account { get; set; }
        public Models.Shipper Shipper { get; private set; }

        [BindProperty]
        public Dictionary<Models.Order, List<OrderDetail>> OrderDetails { get; set; }

        [BindProperty]
        public List<Models.Product> Products { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public List<KeyValuePair<Models.Order, List<OrderDetail>>> DisplayedOrderDetails { get; set; }

        public async Task<IActionResult> OnGet(int? PageNum, string statusFilter, int? PageSizeChange)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/account/singnin");

            Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
            Shipper = dBContext.Shippers.FirstOrDefault(c => c.ShipperId == Account.ShipperId);

            PageSize = PageSizeChange ?? 10;

            if (!string.IsNullOrEmpty(statusFilter))
            {
                var query = dBContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Where(o => o.OrderStatus == statusFilter)
                    .AsQueryable();

                OrderDetails = new Dictionary<Models.Order, List<OrderDetail>>();
                foreach (var order in query)
                {
                    List<OrderDetail> odlist = order.OrderDetails.ToList();
                    OrderDetails.Add(order, odlist);
                }

                // Filter the order list based on the page and page size
                PageNumber = PageNum ?? 1;
                int totalItems = OrderDetails.Count; // Number of order details
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

                DisplayedOrderDetails = OrderDetails
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                // Get the list of products
                Products = dBContext.Products.ToList();
            }
            else
            {
                // No filter case, keep the old logic
                GetOrderDetails(PageNum, PageSize);
            }

            ViewData["CurPage"] = PageNumber;
            ViewData["TotalPage"] = TotalPages;
            return Page();
        }

        public void GetOrderDetails(int? PageNum, int PageSize)
        {
            var orders = dBContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .OrderByDescending(x => x.OrderStatus)
                .ToList();

            OrderDetails = new Dictionary<Models.Order, List<OrderDetail>>();
            foreach (var order in orders)
            {
                List<OrderDetail> odlist = order.OrderDetails.ToList();
                OrderDetails.Add(order, odlist);
            }

            PageNumber = PageNum ?? 1;
            int totalItems = OrderDetails.Count; // Number of order details
            TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            DisplayedOrderDetails = OrderDetails
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            Products = dBContext.Products.ToList();
        }
    }
}
*/
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
                return RedirectToPage("/account/singnin");

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



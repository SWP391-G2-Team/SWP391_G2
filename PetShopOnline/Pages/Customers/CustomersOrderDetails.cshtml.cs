using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Customers
{
    [Authorize(Roles = "2")]
    public class CustomersOrderDetailsModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        public CustomersOrderDetailsModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
        public Models.Shipper Shipper { get; private set; }

        [BindProperty]
        public Customer Customer { get; set; }

        [BindProperty]
        public @Models.Order? Order { get; set; }

        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; }

        [BindProperty]
        public Ship Ship { get; set; }

        [BindProperty]
        public float SumAllPrice { get; set; }

        [BindProperty]
        public float SumPriceSubtotal { get; set; }

        public List<Models.Product> Products { get; set; }
        public async Task<IActionResult> OnGetAsync(int OrderId)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Customer = dBContext.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);

            if (OrderId == null || dBContext.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await dBContext.OrderDetails.FirstOrDefaultAsync(od => od.OrderId == OrderId);

            var shipDetail = await dBContext.Ships.FirstOrDefaultAsync(s => s.OrderId == OrderId);

            var order = await dBContext.Orders.Include(p => p.OrderDetails).ThenInclude(o => o.Product).FirstOrDefaultAsync(o => o.OrderId == OrderId);

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Ship = shipDetail;
                Order = order;
                OrderDetails = order.OrderDetails.ToList();
                SumAllPrice = (float)order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity) + (float)shipDetail.Freight;
                SumPriceSubtotal = (float)order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity);
            }

            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostActiveAsync(int OrderId)
        {
            Models.Order order = dBContext.Orders.FirstOrDefault(o => o.OrderId == OrderId);
            Order = order;

            if (Order.OrderStatus == "Future")
            {
                Order.OrderStatus = "Cancel";
                await dBContext.SaveChangesAsync(); // Save the changes to the database
            }

            return RedirectToPage("/Customers/CustomerInfomation"); // Redirect to the same page after updating the order status
        }

    }
}
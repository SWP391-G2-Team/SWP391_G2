using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Shipper.OrderShipper
{
    [Authorize(Roles = "3")]
    public class OrderDetailModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        public OrderDetailModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
        public Models.Shipper Shipper { get; private set; }

        [BindProperty]
        public @Models.Order? Order { get; set; }

        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; }

        [BindProperty]
        public Ship Ship { get; set; }

        [BindProperty]
        public float SumPriceSubtotal { get; set; }

        [BindProperty]
        public float SumAllPrice { get; set; }

        public List<Models.Product> Products { get; set; }
        public async Task<IActionResult> OnGetAsync(int OrderId)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Shipper = dBContext.Shippers.FirstOrDefault(c => c.ShipperId == Account.ShipperId);



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
            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Shipper = dBContext.Shippers.FirstOrDefault(c => c.ShipperId == Account.ShipperId);
            var shipDetail = await dBContext.Ships.FirstOrDefaultAsync(s => s.OrderId == OrderId);
            Ship = shipDetail;
            Models.Order order = dBContext.Orders.FirstOrDefault(o => o.OrderId == OrderId);
            Order = order;
            int check = 0;

            if (Order.OrderStatus == "Complete" && Shipper != null && check == 0)
            {
                Order.OrderStatus = "Shipped";
                Ship.ShipperId = Shipper.ShipperId;
                await dBContext.SaveChangesAsync();
                check = 1;
            }

            if (Order.OrderStatus == "Shipped" && Shipper != null && check == 0)
            {
                Order.OrderStatus = "Delivered";
                Ship.ShipperId = Shipper.ShipperId;
                await dBContext.SaveChangesAsync();
                check = 1;
            }

            return RedirectToPage("/Shipper/OrderShipperCollectItems/Index");
        }
    }
}

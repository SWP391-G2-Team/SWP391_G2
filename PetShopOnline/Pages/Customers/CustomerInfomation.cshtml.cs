using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetShopOnline.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetShopOnline.Pages.Customers
{
    [Authorize(Roles = "2")]
    public class CustomerInfomationModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        private readonly IConfiguration configuration;

        public CustomerInfomationModel(DTB_PETSHOPContext dBContext, IConfiguration configuration)
        {
            this.dBContext = dBContext;
            this.configuration = configuration;
        }

        [BindProperty]
        public Models.Account Account { get; set; }
        [BindProperty]
        public List<Models.Account> Accounts { get; set; }
        [BindProperty]
        public List<Models.Customer> Customers { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }

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

        public async Task<IActionResult> OnGet(int? PageNum, string? txtSearch)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
            Customer = dBContext.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);

            if (txtSearch != null)
            {
                // Lấy danh sách sản phẩm cần tìm kiếm
                var searchProducts = await dBContext.Products.Where(p => p.ProductName.Contains(txtSearch.Trim())).ToListAsync();
                var productIds = searchProducts.Select(p => p.ProductId).ToList();

                // Lấy danh sách order chi tiết có chứa sản phẩm tìm kiếm
                var orderDetails = await dBContext.OrderDetails
                    .Include(od => od.Order)
                    .Include(od => od.Product)
                    .Where(od => productIds.Contains(od.ProductId))
                    .ToListAsync();

                // Lấy danh sách order tương ứng với các order chi tiết tìm kiếm
                var orderIds = orderDetails.Select(od => od.OrderId).ToList();
                var orders = await dBContext.Orders
                    .Include(o => o.Customer)
                    .Where(o => orderIds.Contains(o.OrderId))
                    .ToListAsync();

                // Gom các order chi tiết vào từng order
                OrderDetails = new Dictionary<Models.Order, List<OrderDetail>>();
                foreach (var orderDetail in orderDetails)
                {
                    if (!OrderDetails.ContainsKey(orderDetail.Order))
                    {
                        OrderDetails[orderDetail.Order] = new List<OrderDetail>();
                    }
                    OrderDetails[orderDetail.Order].Add(orderDetail);
                }

                // Filter danh sách order theo trang và kích thước trang
                PageNumber = PageNum ?? 1;
                int totalItems = OrderDetails.Count; // Số lượng order chi tiết
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

                DisplayedOrderDetails = OrderDetails
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                // Lấy danh sách sản phẩm
                Products = searchProducts;
            }
            else
            {
                // Trường hợp không có tìm kiếm, giữ lại logic cũ
                GetOrderDetails(PageNum);
            }

            ViewData["CurPage"] = PageNumber;
            ViewData["TotalPage"] = TotalPages;

            return Page();

        }

        public void GetOrderDetails(int? PageNum)
        {
            var orders = dBContext.Orders
                .Where(o => o.CustomerId == Account.CustomerId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .OrderByDescending(x => x.OrderId)
                .ToList();

            OrderDetails = new Dictionary<Models.Order, List<OrderDetail>>();
            foreach (var order in orders)
            {
                List<OrderDetail> odlist = order.OrderDetails.ToList();
                OrderDetails.Add(order, odlist);
            }

            // Tính toán và thiết lập giá trị cho CurPage và TotalPage
            PageNumber = PageNum ?? 1;
            int totalItems = OrderDetails.Count; // Số lượng order chi tiết
            TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            DisplayedOrderDetails = OrderDetails
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            Products = dBContext.Products.ToList();
        }
    }
}

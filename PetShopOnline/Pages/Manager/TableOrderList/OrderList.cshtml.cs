using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Manager.TableOrderList
{
    [Authorize(Roles = "1")]
    public class OrderListModel : PageModel
    {
        private readonly DTB_PETSHOPContext _projectContext;

        public OrderListModel(DTB_PETSHOPContext projectContext)
        {
            _projectContext = projectContext;
        }

        public PaginatedList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGet(int? pageIndex)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/Account/SignIn");
            }
            else
            {
                int pageSize = 10; // Number of orders per page
                IQueryable<Order> ordersQuery = _projectContext.Orders.Include(o => o.Customer);

                Orders = await PaginatedList<Order>.CreateAsync(ordersQuery, pageIndex ?? 1, pageSize);

                return Page();
            }
        }

        public IActionResult OnGetDetail(int orderId)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/Account/SignIn");
            }
            else
            {
                return RedirectToPage("Detail", new { orderId = orderId });
            }
        }
    }
}

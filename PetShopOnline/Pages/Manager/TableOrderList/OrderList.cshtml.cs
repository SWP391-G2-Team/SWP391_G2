using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PetShop_Project_SWP391.Pages.Manager.TableOrderList
{
    [Authorize(Roles = "1")]
    public class OrderListModel : PageModel
    {
        private readonly ProjectContext _projectContext;

        public OrderListModel(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public PaginatedList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGet(int? pageIndex)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/account/singnin");
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
                return RedirectToPage("/account/singnin");
            }
            else
            {
                return RedirectToPage("Detail", new { orderId = orderId });
            }
        }
    }
}

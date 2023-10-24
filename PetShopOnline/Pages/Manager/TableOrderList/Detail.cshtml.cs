using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Manager.TableOrderList
{
    [Authorize(Roles = "1")]
    public class DetailModel : PageModel
    {
        private readonly DTB_PETSHOPContext _projectcontext;
        public DetailModel(DTB_PETSHOPContext projectContext)
        {
            _projectcontext = projectContext;
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/Account/SignIn");
            }
            else
            {
                Order = await _projectcontext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Include(o => o.Customer)
                    .Include(o => o.Employee)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (Order == null)
                {
                    return NotFound();
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostActiveAsync(int orderId, string action)
        {
            Models.Order order = _projectcontext.Orders.FirstOrDefault(o => o.OrderId == orderId);
            Order = order;

            if (Order.OrderStatus == "Future")
            {
                if (action == "complete")
                {
                    Order.OrderStatus = "Complete";
                }
                else if (action == "cancel")
                {
                    Order.OrderStatus = "Cancel";
                }

                await _projectcontext.SaveChangesAsync();
            }

            // Redirect back to the Order Detail page
            return RedirectToPage("Detail", new { orderId = orderId });
        }
    }

}

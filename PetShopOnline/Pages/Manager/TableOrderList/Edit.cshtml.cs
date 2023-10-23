using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Manager.TableOrderList
{
    [Authorize(Roles = "1")]
    public class EditModel : PageModel
    {
        private readonly DTB_PETSHOPContext _projectContext;

        public EditModel(DTB_PETSHOPContext projectContext)
        {
            _projectContext = projectContext;
        }

        [BindProperty]
        public Order Order { get; set; }

        [BindProperty]
        public string SelectedOrderStatus { get; set; }

        public List<SelectListItem> OrderStatuses { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/Account/SignIn");
            }

            Order = await _projectContext.Orders.FindAsync(orderId);

            if (Order == null)
            {
                return NotFound();
            }

            // Initialize the list of OrderStatuses
            OrderStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Pending", Text = "Pending" },
                new SelectListItem { Value = "Processing", Text = "Processing" },
                new SelectListItem { Value = "Shipped", Text = "Shipped" },
                new SelectListItem { Value = "Delivered", Text = "Delivered" }
            };

            // Set the selected order status to the current order's status
            SelectedOrderStatus = Order.OrderStatus;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update the order status based on the value in SelectedOrderStatus
            Order = await _projectContext.Orders.FindAsync(Order.OrderId);

            if (Order == null)
            {
                return NotFound();
            }

            Order.OrderStatus = SelectedOrderStatus;
            await _projectContext.SaveChangesAsync();

            return RedirectToPage("/Manager/TableOrderList/Detail", new { orderId = Order.OrderId });
        }
    }
}


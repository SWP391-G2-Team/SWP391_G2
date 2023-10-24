using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Manager.Shippers
{
    [Authorize(Roles = "1")]
    public class DetailModel : PageModel
    {
        private readonly DTB_PETSHOPContext _context;

        public DetailModel(DTB_PETSHOPContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.Account Account { get; set; }
        [BindProperty]
        public Models.Shipper Shipper { get; set; }
        public Models.Employee Employee { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("PetSession") == null) return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));

            Employee = _context.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);
            if (id == null || _context.Shippers == null)
            {
                return NotFound();
            }
            var shipper = await _context.Shippers.FirstOrDefaultAsync(c => c.ShipperId == id);
            if (shipper == null)
            {
                return NotFound();
            }
            else
            {
                Shipper = shipper;
            }
            return Page();
        }
    }
}

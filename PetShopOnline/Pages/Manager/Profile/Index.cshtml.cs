using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Manager.Profile
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {

        private readonly DTB_PETSHOPContext dBContext;
        public IndexModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Models.Employee Employee { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Employee = dBContext.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);
            return Page();
        }
    }
}

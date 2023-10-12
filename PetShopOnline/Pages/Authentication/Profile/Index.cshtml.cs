using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShop_Project_SWP391.Models;
using System.Text.Json;

namespace PetShop_Project_SWP391.Pages.Manager.Profile
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {

        private readonly ProjectContext dBContext;
        public IndexModel(ProjectContext dBContext)
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
                return RedirectToPage("/account/singnin");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Employee = dBContext.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);
            return Page();
        }
    }
}


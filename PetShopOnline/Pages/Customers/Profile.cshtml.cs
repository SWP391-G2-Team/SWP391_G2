using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Customers
{
    [Authorize(Roles = "2")]
    public class ProfileModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        public ProfileModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Customer = dBContext.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);
            return Page();
        }
    }
}

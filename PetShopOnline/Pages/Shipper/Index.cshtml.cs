using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;
using System.Text.Json;

namespace PetShop_Project_SWP391.Pages.Shipper
{
    [Authorize(Roles = "3")]
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
        public Models.Shipper Shipper { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/account/singnin");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Shipper = dBContext.Shippers.FirstOrDefault(c => c.ShipperId == Account.ShipperId);


            return Page();
        }
    }
}

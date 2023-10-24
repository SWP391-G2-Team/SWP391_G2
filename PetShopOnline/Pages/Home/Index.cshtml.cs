using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        public IndexModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [BindProperty]
        public List<Category> Category { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Category = await dBContext.Categories.ToListAsync();

            return Page();
        }
    }
}

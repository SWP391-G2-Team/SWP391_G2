using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Manager.Blog
{
    [Authorize(Roles = "1")]
    public class BlogDetailModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        private readonly IConfiguration configuration;
        public BlogDetailModel(DTB_PETSHOPContext dBContext, IConfiguration configuration)
        {
            this.dBContext = dBContext;
            this.configuration = configuration;
        }
        [BindProperty]
        public List<Models.Account> Accounts { get; set; }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
        public @Models.BlogDetail BlogDetail { get; set; }
        [BindProperty]
        public List<Models.BlogDetail> BlogDetails { get; set; }
        [BindProperty]
        public List<Models.Customer> Customers { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public @Models.Employee Employee { get; set; }
        [BindProperty]
        public List<Models.Employee> Employees { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/account/signin");
            }
            else
            {


                var blogdetail = await dBContext.BlogDetails.FirstOrDefaultAsync(m => m.BlogId == id);
                if (blogdetail == null)
                {
                    return NotFound();
                }
                BlogDetail = blogdetail;

                return Page();
            }
        }

        private bool BlogDetailExists(int id)
        {
            return (dBContext.BlogDetails?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}


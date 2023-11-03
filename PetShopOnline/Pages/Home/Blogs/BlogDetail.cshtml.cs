using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Customers.Blogs
{
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
        public DateTime PublishedDate { get; set; }
        public string FormattedDate => PublishedDate.ToString("d MMMM yyyy");

        public async Task<IActionResult> OnGet(int id)
        {
            var blogdetail = await dBContext.BlogDetails.FirstOrDefaultAsync(m => m.BlogId == id);
            if (blogdetail == null)
            {
                return NotFound();
            }
            BlogDetail = blogdetail;

            // Get the three latest blogs
            //3 bài mới nhất 
            BlogDetails = await dBContext.BlogDetails
                .OrderByDescending(blog => blog.BlogId)
                .Take(3)
                .ToListAsync();
            /*BlogDetails = blogdetail.PageTitle.Take(3).ToList();*/


            return Page();
        }

        private bool BlogDetailExists(int id)
        {
            return (dBContext.BlogDetails?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}

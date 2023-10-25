using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Customers.Blog
{
    public class ListModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        private readonly IConfiguration configuration;
        public ListModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;

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
        public async Task<IActionResult> OnGet()
        {


                if (dBContext.BlogDetails != null)
                {
                    BlogDetails = await dBContext.BlogDetails
                   .ToListAsync();
                }
                return Page();
            }
        }
    }

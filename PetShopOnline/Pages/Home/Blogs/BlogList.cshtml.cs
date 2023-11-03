using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Customers.Blog
{
    public class BlogListModel : PageModel
    {
        /*[Authorize(Roles = "2")]*/
        private readonly DTB_PETSHOPContext dBContext;
        private readonly IConfiguration configuration;
        public BlogListModel(DTB_PETSHOPContext dBContext, IConfiguration configuration)
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
        public DateTime PublishedDate { get; set; }
        public string FormattedDate => PublishedDate.ToString("d MMMM yyyy");
        public async Task<IActionResult> OnGet(int? PageNum, string? txtSearch)
        {
            if (dBContext.BlogDetails != null)
            {
                BlogDetails = await dBContext.BlogDetails
               .ToListAsync();
            }

            if (PageNum <= 0 || PageNum is null) PageNum = 1;
            int PageSize = Convert.ToInt32(configuration.GetValue<string>("AppSettings:PageSize"));

            // Kiểm tra giá trị PageSize và gán giá trị mặc định nếu nó bằng 0
            if (PageSize == 0)
            {
                PageSize = 6; // Giá trị mặc định
            }

            IQueryable<BlogDetail> query = dBContext.BlogDetails;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.PageTitle.Contains(txtSearch.Trim()));
            }
            query = query.OrderByDescending(x => x.BlogId);

            int TotalProduct = await query.CountAsync();

            // Kiểm tra PageSize để đảm bảo không chia cho 0
            int TotalPage = PageSize > 0 ? (TotalProduct / PageSize) : 0;
            if (TotalProduct % PageSize != 0) TotalPage++;
            ViewData["TotalPage"] = TotalPage;
            ViewData["CurPage"] = PageNum;
            ViewData["txtSearch"] = txtSearch;

            BlogDetails = await query.Skip((int)(PageNum - 1) * PageSize).Take(PageSize).ToListAsync();


            return Page();
        }
    }
}





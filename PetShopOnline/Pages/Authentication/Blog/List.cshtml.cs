using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;

namespace PetShop_Project_SWP391.Pages.Manager.Blog
{
    [Authorize(Roles = "1")]
    public class ListModel : PageModel
    {


        private readonly ProjectContext dBContext;
        private readonly IConfiguration configuration;
        public ListModel(ProjectContext dBContext, IConfiguration configuration)
        {
            this.dBContext = dBContext;
            this.configuration = configuration;
        }

        [BindProperty]
        public List<Models.Account> Accounts { get; set; }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
       /* public @Models.BlogDetail BlogDetail { get; set; }*/
        public List<Models.BlogDetail> BlogDetails { get; set; } = default!;
        public BlogDetail BlogDetail { get; set; }

        public async Task<IActionResult> OnGet(int? PageNum, string? txtSearch)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/account/singnin");
            }
            else
            {
                if (PageNum <= 0 || PageNum is null) PageNum = 1;
                int PageSize = Convert.ToInt32(configuration.GetValue<string>("AppSettings:PageSize"));

                // Kiểm tra giá trị PageSize và gán giá trị mặc định nếu nó bằng 0
                if (PageSize == 0)
                {
                    PageSize = 3; // Giá trị mặc định
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
        public async Task<IActionResult> OnPost(int id)
        {
            var blog = await dBContext.BlogDetails.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            dBContext.BlogDetails.Remove(blog);
            await dBContext.SaveChangesAsync();

            return RedirectToPage("/Manager/Blog/List");
        }




    }
}


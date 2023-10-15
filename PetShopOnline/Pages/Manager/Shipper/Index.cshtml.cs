using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Manager.Shipper
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {
        private readonly DTB_PETSHOPContext dBContext;
        private readonly IConfiguration configuration;
        public IndexModel(DTB_PETSHOPContext dBContext, IConfiguration configuration)
        {
            this.dBContext = dBContext;
            this.configuration = configuration;
        }
        [BindProperty]
        public List<Models.Account> Accounts { get; set; }
        [BindProperty]
        public List<Models.Shipper> Shippers { get; set; }
        [BindProperty]
        public @Models.Account Account { get; set; }
        //public Employee employee { get; set; }
        public async Task<IActionResult> OnGet(int? PageNum, string? txtSearch)
        {
            if (HttpContext.Session.GetString("PetSession") == null)
            {
                return RedirectToPage("/Account/SignIn");
            }
            else
            {
                if (PageNum <= 0 || PageNum is null) PageNum = 1;
                int PageSize = Convert.ToInt32(configuration.GetValue<string>("AppSettings:PageSize"));

                // Kiểm tra giá trị PageSize và gán giá trị mặc định nếu nó bằng 0
                if (PageSize == 0)
                {
                    PageSize = 10; // Giá trị mặc định
                }

                //Accounts = await dBContext.Accounts.Where(x => x.Role == 3).Include(x => x.Shipper).ToListAsync();
                Shippers = await dBContext.Shippers.ToListAsync();

                if (txtSearch != null) Shippers = Shippers.Where(x => x.Name.Contains(txtSearch.Trim())).ToList();

                int TotalShipper = Shippers.Count;

                // Kiểm tra PageSize để đảm bảo không chia cho 0
                int TotalPage = PageSize > 0 ? (TotalShipper / PageSize) : 0;
                if (TotalShipper % PageSize != 0) TotalPage++;
                ViewData["TotalPage"] = TotalPage;
                ViewData["CurPage"] = PageNum;
                ViewData["txtSearch"] = txtSearch;

                Shippers = Shippers.Skip((int)(((PageNum - 1) * PageSize + 1) - 1)).Take(PageSize).ToList();

                return Page();
            }
        }

        public IActionResult OnGetActive(int? id)
        {
            if (id == null) return Redirect("~/Manager/Shipper/Index");

            Models.Account account = dBContext.Accounts.FirstOrDefault(x => x.AccountId == id);
            if (account == null) return Redirect("~/Manager/Shipper/index");

            if (account.IsActive == true)
            {
                account.IsActive = false;
            }
            else
            {
                account.IsActive = true;
            }
            dBContext.SaveChanges();

            return Redirect("~/Manager/Shipper/Index");
        }
    }
}

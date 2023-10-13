using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using PetShopOnline.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShopOnline.Controllers.Manager.ChangePassword
{
    public class ChangePassword : Controller
    {
        [Authorize(Roles = "1")]
        public class IndexModel : PageModel
        {
            private readonly DTB_PETSHOPContext dbcontext;
            public IndexModel(DTB_PETSHOPContext dbcontext)
            {
                this.dbcontext = dbcontext;
            }

            [BindProperty]
            public Models.Account Account { get; set; }
            public int AccountId { get; set; }

            [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc.")]
            [BindProperty]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
            [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất {1} ký tự.")]
            [RegularExpression("^(?!.*\\s)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái viết thường, một chữ cái viết hoa, một chữ số và một ký tự đặc biệt. Không được chứa khoảng trắng.")]
            [BindProperty]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Xác nhận lại mật khẩu là bắt buộc.")]
            [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            [BindProperty]
            public string ConfirmPassword { get; set; }

            public async Task<IActionResult> OnGet()
            {
                if (HttpContext.Session.GetString("PetSession") == null)
                    return RedirectToPage("/account/singnin");

                Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
                return Page();
            }

            public async Task<IActionResult> OnPost()
            {
                if (ModelState.IsValid)
                {
                    var acc = await dbcontext.Accounts.FirstOrDefaultAsync(x => x.AccountId == Account.AccountId);
                    Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));

                    if (CurrentPassword != Account.Password)
                    {
                        ViewData["PasswordError"] = "Mật khẩu cũ không chính xác!";
                        return Page();
                    }

                    if (NewPassword != ConfirmPassword)
                    {
                        ViewData["PasswordError"] = "Mật khẩu xác nhận không khớp!";
                        return Page();
                    }

                    acc.Password = NewPassword;
                    dbcontext.Accounts.Update(acc);
                    await dbcontext.SaveChangesAsync();

                    ViewData["Success"] = "Thay đổi mật khẩu thành công!";
                }

                return Page();
            }
        }
    }
}

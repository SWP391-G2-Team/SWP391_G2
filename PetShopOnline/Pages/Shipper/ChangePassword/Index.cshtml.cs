﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PetShop_Project_SWP391.Pages.Shipper.ChangePassword
{
    [Authorize(Roles = "3")]
    public class IndexModel : PageModel
    {
        private readonly ProjectContext dbcontext;
        public IndexModel(ProjectContext dbcontext)
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
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái viết thường, một chữ cái viết hoa, một chữ số và một ký tự đặc biệt.")]
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

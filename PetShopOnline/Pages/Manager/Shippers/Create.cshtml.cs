using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PetShopOnline.Pages.Manager.Shippers
{
    [Authorize(Roles = "1")]
    public class CreateModel : PageModel
    {
        private readonly DTB_PETSHOPContext context;

        public CreateModel(DTB_PETSHOPContext context)
        {
            this.context = context;
        }
        [BindProperty]
        public Models.Shipper Shipper { get; set; }
        [BindProperty]
        public Models.Account Account { get; set; }
        [BindProperty]
        public Models.Employee Employee { get; set; }

        [BindProperty]
        public string EmailShipper { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất {1} ký tự.")]
        [RegularExpression("^(?!.*\\s)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái viết thường, một chữ cái viết hoa, một chữ số và một ký tự đặc biệt. Không được chứa khoảng trắng.")]
        [BindProperty]
        public string PasswordShipper { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
            Employee = context.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Shipper.Name) ||
                string.IsNullOrEmpty(Shipper.Phone) ||
                string.IsNullOrEmpty(Shipper.Address) ||
                string.IsNullOrEmpty(Shipper.District) ||
                string.IsNullOrEmpty(Shipper.Wards) ||
                string.IsNullOrEmpty(Shipper.Province) ||
                string.IsNullOrEmpty(EmailShipper) ||
                string.IsNullOrEmpty(PasswordShipper)
                )
            {
                ViewData["msgEmpty"] = "Vui lòng nhập cả tất cả thông tin bên trên";
                return Page();
            }
            var acc = await context.Accounts.FirstOrDefaultAsync(a => a.Email == EmailShipper);
            if (acc == null)
            {
                var shipper = new Models.Shipper()
                {
                    Name = Shipper.Name,
                    Phone = Shipper.Phone,
                    Address = Shipper.Address,
                    District = Shipper.District,
                    Wards = Shipper.Wards,
                    Province = Shipper.Province
                };

                context.Shippers.Add(shipper);
                await context.SaveChangesAsync();

                var newAcc = new Models.Account()
                {
                    Email = EmailShipper,
                    Password = PasswordShipper,
                    ShipperId = shipper.ShipperId,
                    Role = 3,
                    IsActive = true,
                };

                context.Accounts.Add(newAcc);
                await context.SaveChangesAsync();

                ViewData["msgSuccess"] = "Thêm Shipper thành công";
                return Page();
            }
            else
            {
                ViewData["msgEmailSame"] = "Tài khoản đã tồn tại";
                return Page();
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetShop_Project_SWP391.Pages.Manager.Employee
{
    [Authorize(Roles = "1")]
    public class CreateAccountModel : PageModel
    {
        private readonly ProjectContext projectContext;
        public CreateAccountModel(ProjectContext projectContext)
        {
            this.projectContext = projectContext;
        }

        [BindProperty]
        public Models.Employee Employee { get; set; }

        [BindProperty]
        public Models.Account Account { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất {1} ký tự.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái viết thường, một chữ cái viết hoa, một chữ số và một ký tự đặc biệt.")]
        [BindProperty]
        public string NewPassword { get; set; }
        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Account.Email) || string.IsNullOrEmpty(Account.Password) ||
                string.IsNullOrEmpty(Employee.FirstName) || string.IsNullOrEmpty(Employee.LastName) ||
                string.IsNullOrEmpty(Employee.Title) || string.IsNullOrEmpty(Employee.Address))
            {
                ViewData["msgEmpty"] = "Vui lòng nhập tất cả thông tin bên trên";
                return Page();
            }
            if (NewPassword != ConfirmPassword)
            {
                ViewData["msgPassword"] = "Xác nhận mật khẩu không trùng khớp";
                return Page();
            }
            if (ModelState.IsValid)
            {
                var acc = await projectContext.Accounts.FirstOrDefaultAsync(a => a.Email == Account.Email);
                if (acc == null)
                {
                    var emp = new Models.Employee()
                    {
                        EmployeeId = GenerateEmpID(10),
                        FirstName = Employee.FirstName,
                        LastName = Employee.LastName,
                        Title = Employee.Title,
                        BirthDate = DateTime.Now,
                        Address = Employee.Address,
                    };

                    var newAcc = new Models.Account()
                    {
                        Email = Account.Email,
                        Password = NewPassword,
                        EmployeeId = emp.EmployeeId,
                        Role = 1,
                        IsActive = true,
                    };
                    await projectContext.Employees.AddAsync(emp);
                    await projectContext.Accounts.AddAsync(newAcc);
                    await projectContext.SaveChangesAsync();
                    ViewData["msgSuccess"] = "Đăng ký tài khoản thành công";
                    return Page();
                }
                else
                {
                    ViewData["msgEmail"] = "Tài khoản đã tồn tại!";
                    return Page();
                }
            }
            else
            {
                return Page();
            };
        }

        public string GenerateEmpID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }
    }
}

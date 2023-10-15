using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PetShopOnline.Pages.Manager.Profile
{
    [Authorize(Roles = "1")]
    public class EditProfileModel : PageModel
    {
        private readonly  DTB_PETSHOPContext dBContext;
        public EditProfileModel(DTB_PETSHOPContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [BindProperty]
        public @Models.Account Account { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Models.Employee employee { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/Account/SignIn");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            employee = dBContext.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (
               string.IsNullOrEmpty(employee.FirstName) ||
               string.IsNullOrEmpty(employee.LastName) ||
               string.IsNullOrEmpty(employee.Phone) ||
               string.IsNullOrEmpty(employee.Address))
            {
                ViewData["msgEmpty"] = "Vui lòng nhập cả tất cả thông tin bên trên";
                return Page();
            }
            if (!Regex.IsMatch(employee.Phone, @"^0\d{9}$"))
            {
                ViewData["msgPhone"] = "Số điện thoại không hợp lệ. Vui lòng nhập số bắt đầu từ 0 và có 10 chữ số!";
                return Page();
            }
            var acc = await dBContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == Account.AccountId);
            var emp = await dBContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == Account.EmployeeId);

            var checkemail = await dBContext.Accounts.FirstOrDefaultAsync(x => x.AccountId != Account.AccountId && x.Email.Equals(Account.Email));


            if (emp != null)
            {
                emp.FirstName = employee.FirstName;
                emp.LastName = employee.LastName;
                emp.Phone = employee.Phone;
                emp.BirthDate = employee.BirthDate;
                emp.Address = employee.Address;

                //acc.Email = Account.Email;
                await dBContext.SaveChangesAsync();
                return RedirectToPage("/Manager/Profile/Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PetShop_Project_SWP391.Pages.Shipper.Profile
{
    [Authorize(Roles = "3")]
    public class EditProfileShipperModel : PageModel
    {
        private readonly ProjectContext dBContext;
        public EditProfileShipperModel(ProjectContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [BindProperty]
        public @Models.Account Account { get; set; }

        [BindProperty]
        public Models.Shipper Shipper { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/account/singnin");

            Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
            Shipper = dBContext.Shippers.FirstOrDefault(c => c.ShipperId == Account.ShipperId);
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
            if (
               string.IsNullOrEmpty(Shipper.Name) ||
               string.IsNullOrEmpty(Shipper.Address) ||
               string.IsNullOrEmpty(Shipper.District) ||
               string.IsNullOrEmpty(Shipper.Province) ||
               string.IsNullOrEmpty(Shipper.Wards) ||
               string.IsNullOrEmpty(Shipper.Phone))
            {
                ViewData["msgEmpty"] = "Vui lòng nhập cả tất cả thông tin bên trên";
                return Page();
            }
            if (!Regex.IsMatch(Shipper.Phone, @"^0\d{9}$"))
            {
                ViewData["msgPhone"] = "Số điện thoại không hợp lệ. Vui lòng nhập số bắt đầu từ 0 và có 10 chữ số!";
                return Page();
            }
            var shippper = await dBContext.Shippers.FirstOrDefaultAsync(x => x.ShipperId == Account.ShipperId);
            if (shippper != null)
            {
                shippper.Name = Shipper.Name;
                shippper.Address = Shipper.Address;
                shippper.District = Shipper.District;
                shippper.Province = Shipper.Province;
                shippper.Wards = Shipper.Wards;
                shippper.Phone = Shipper.Phone;
                await dBContext.SaveChangesAsync();
                ViewData["msgSuccess"] = "Thay đổi thông tin thành công";
            };
            return RedirectToPage("/Shipper/Profile/Index");
        }
    }
}

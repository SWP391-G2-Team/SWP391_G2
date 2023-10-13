using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PetShopOnline.Controllers.Account
{
    public class LogOut : Controller
    {
        public class LogoutModel : PageModel
        {
            public async Task<IActionResult> OnGet()
            {
                if (User.Identity.IsAuthenticated)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.Session.Remove("PetSession");
                }
                return RedirectToPage("/index");
            }
        }
    }
}

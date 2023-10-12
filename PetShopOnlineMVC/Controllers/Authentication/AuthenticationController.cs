using Microsoft.AspNetCore.Mvc;

namespace PetShopOnlineMVC.Controllers.Authentication
{
    public class AuthenticationController : Controller
    {
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}

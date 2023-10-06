using Microsoft.AspNetCore.Mvc;

namespace PetShopOnline.Controllers.Account
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

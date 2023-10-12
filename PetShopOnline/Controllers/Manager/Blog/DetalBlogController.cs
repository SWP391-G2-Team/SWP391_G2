using Microsoft.AspNetCore.Mvc;

namespace PetShopOnline.Controllers.Manager.Blog
{
    public class DetalBlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace PetShopOnline.Controllers.Manager.Blog
{
    public class ListBlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

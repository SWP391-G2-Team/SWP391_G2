using Microsoft.AspNetCore.Mvc;

namespace PetShopOnlineMVC.Controllers.Product
{
    public class ProductController : Controller
    {
        [HttpGet("ProductList")]
        public IActionResult ProductList()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Product
{
    public class ProductDetailModel : PageModel
    {
        private readonly DTB_PETSHOPContext db;
        public ProductDetailModel(DTB_PETSHOPContext dbContext)
        {
            db = dbContext;
        }
        [BindProperty]
        public Models.Product Product { get; set; }
        [BindProperty]
        public List<Models.Product> Products { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Models.Account Account { get; set; }
        [BindProperty]
        public List<Cart>? CartItems { get; set; }
        [BindProperty]
        decimal? total { get; set; }
        public IActionResult OnGet(int id)
        {
            Product = db.Products.Include(x => x.Pictures)
                .FirstOrDefault(p => p.ProductId == id);
            return Page();
        }
        public IActionResult OnGetAddToCart(int id)
        {
            string? cart = HttpContext.Session.GetString("CartSession");
            Product = db.Products.Include(x => x.Pictures).FirstOrDefault(p => p.ProductId == id);
            total = 0;
            if (cart == null)
            {
                CartItems = new List<Cart>();
                CartItems.Add(new Cart(Product, 1));
                total += Product.UnitPrice;
            }
            else
            {
                string? cartsDeserialize = HttpContext.Session.GetString("CartSession");
                CartItems = JsonSerializer.Deserialize<List<Cart>>(cartsDeserialize);
                total = GetTotal(CartItems);
                int index = getIndexOfProductInCart(Product, CartItems);
                if (index != -1) CartItems[index].Quantity++;
                else CartItems.Add(new Cart(Product, 1));
                total += Product.UnitPrice;
            }
            string cartsSerialize = JsonSerializer.Serialize(CartItems);
            HttpContext.Session.SetString("CartSession", cartsSerialize);

            string totalSerialize = JsonSerializer.Serialize(total);
            HttpContext.Session.SetString("total", totalSerialize);

            if (HttpContext.Session.GetString("PetSession") != null)
            {
                Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
                Customer = db.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);
            }
            ViewData["total"] = total;
            ViewData["successmess"] = "Thêm Vào Giỏ Hàng Thành Công!";
            return Page();
        }
        public decimal? GetTotal(List<Cart> carts)
        {
            decimal? total = 0;
            foreach (Cart cart in carts)
            {
                total += cart.Product.UnitPrice * cart.Quantity;
            }
            return total;
        }
        public int getIndexOfProductInCart(Models.Product product, List<Cart> carts)
        {
            for (int i = 0; i < carts.Count; i++)
            {
                if (carts[i].Product.ProductId == product.ProductId)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

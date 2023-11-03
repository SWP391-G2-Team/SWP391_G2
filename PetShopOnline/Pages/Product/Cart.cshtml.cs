using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Text;
using System.Text.Json;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Product
{
    [Authorize(Roles = "2")]
    public class CartModel : PageModel
    {
        private readonly DTB_PETSHOPContext db;
        public CartModel(DTB_PETSHOPContext db)
        {
            this.db = db;
        }
        [BindProperty]
        public Ship Ship { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }


        [BindProperty]
        public Models.Product Product { get; set; }

        [BindProperty]
        public Models.Account Account { get; set; }

        [BindProperty]
        public List<Cart>? CartItems { get; set; }

        [BindProperty]
        decimal? total { get; set; }
        public IActionResult OnGet(int id)
        {
            string? cart = HttpContext.Session.GetString("CartSession");
            Models.Product product = db.Products.Include(x => x.Pictures).FirstOrDefault(p => p.ProductId == id);
            if (cart == null)
            {
                ViewData["emptymess"] = "Cart empty!";
                return Page();
            }

            CartItems = JsonSerializer.Deserialize<List<Cart>>(cart);
            total = GetTotal(CartItems);

            if (HttpContext.Session.GetString("PetSession") != null)
            {
                Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
                Customer = db.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);
            }

            ViewData["total"] = total;
            return Page();
        }
        public IActionResult OnGetBuyNow(int id)
        {
            string? cart = HttpContext.Session.GetString("CartSession");
            Models.Product product = db.Products.Include(x => x.Pictures).FirstOrDefault(p => p.ProductId == id);
            total = 0;

            if (cart == null)
            {
                //tao mot gio hang moi va them san pham do vao
                CartItems = new List<Cart>();
                CartItems.Add(new Cart(product, 1));
                total += product.UnitPrice;
            }
            else
            {
                string? cartsdeserialize = HttpContext.Session.GetString("CartSession");
                //deserialize Dictionary from string got from session
                CartItems = JsonSerializer.Deserialize<List<Cart>>(cartsdeserialize);
                total = GetTotal(CartItems);

                int index = getIndexOfProductInCart(product, CartItems);
                if (index != -1)
                    CartItems[index].Quantity++; //tang so luong
                else
                    CartItems.Add(new Cart(product, 1)); //them san pham 

                total += product.UnitPrice;
            }

            //serialize dictionary to string to store in session
            string cartsserialize = JsonSerializer.Serialize(CartItems);
            HttpContext.Session.SetString("CartSession", cartsserialize);

            string totalserialize = JsonSerializer.Serialize(total);
            HttpContext.Session.SetString("total", totalserialize);

            if (HttpContext.Session.GetString("PetSession") != null)
            {
                Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
                Customer = db.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);
            }

            ViewData["total"] = total;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Customer.Name != null && Customer.Phone != null
                && Customer.Province != null && Customer.Address != null)
            {
                var session = HttpContext.Session.GetString("PetSession");
                var acc = new Models.Account();
                var cus = new Customer();
                if (session is not null)
                {
                    Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
                    Customer = db.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);
                }
                else
                {
                    return RedirectToPage("/Account/SignIn");
                }

                var order = new Models.Order()
                {
                    CustomerId = Customer.CustomerId,
                    OrderDate = DateTime.Now,
                    ShippedDate = DateTime.Now.AddDays(7),
                    OrderStatus = "Future",
                };

                await db.Orders.AddAsync(order);
                await db.SaveChangesAsync();

                int LastID = order.OrderId;

                string? cartsdeserialize = HttpContext.Session.GetString("CartSession");
                CartItems = JsonSerializer.Deserialize<List<Cart>>(cartsdeserialize);

                var orderDetails = new List<OrderDetail>();

                foreach (var item in CartItems)
                {
                    var od = new OrderDetail()
                    {
                        OrderId = LastID,
                        ProductId = item.Product.ProductId,
                        UnitPrice = (decimal)item.Product.UnitPrice,
                        Quantity = (short)item.Quantity,
                        Discount = 0
                    };
                    var product = db.Products.FirstOrDefault(d => d.ProductId == item.Product.ProductId);
                    product.QuantityPerUnit = product.QuantityPerUnit - item.Quantity;
                    if (product.QuantityPerUnit < 0)
                    {
                        ViewData["msgQuantityProduct"] = "Sản phẩm vượt qua số lượng hàng đang có";
                        return Page();
                    }
                    if (product.QuantityPerUnit == 0)
                    {
                        product.Status = false;
                    }

                    orderDetails.Add(od);
                }

                await db.OrderDetails.AddRangeAsync(orderDetails);
                await db.SaveChangesAsync();

                var shipData = new Models.Ship()
                {
                    ShipAddress = Customer.Address,
                    ShipCity = Customer.Province,
                    PhoneContact = Customer.Phone,
                    ShipContact = Customer.Name,
                    Freight = 0,
                    OrderId = LastID
                };

                await db.Ships.AddAsync(shipData);
                await db.SaveChangesAsync();

                HttpContext.Session.Remove("CartSession");

                if (session is null) return RedirectToPage("/index");
                ViewData["msg"] = "Order successful";
                return RedirectToPage("/Customers/CustomerInfomation");

            }
            else
            {
                return RedirectToPage("/Product/Cart");
            }
        }

        public IActionResult OnGetDelete(int id)
        {
            string? cartsdeserialize = HttpContext.Session.GetString("CartSession");
            CartItems = JsonSerializer.Deserialize<List<Cart>>(cartsdeserialize);
            Models.Product product = db.Products.Include(x => x.Pictures).FirstOrDefault(p => p.ProductId == id);
            int index = getIndexOfProductInCart(product, CartItems);
            CartItems.RemoveAt(index);
            total = GetTotal(CartItems);

            string cartsserialize = JsonSerializer.Serialize(CartItems);
            HttpContext.Session.SetString("CartSession", cartsserialize);

            string totalserialize = JsonSerializer.Serialize(total);
            HttpContext.Session.SetString("total", totalserialize);

            if (CartItems.Count == 0)
            {
                ViewData["emptymess"] = "Cart empty!";
            }
            ViewData["total"] = total;
            return RedirectToPage("/Product/Cart");
        }

        public IActionResult OnGetPlus(int id)
        {
            string? cartsdeserialize = HttpContext.Session.GetString("CartSession");
            CartItems = JsonSerializer.Deserialize<List<Cart>>(cartsdeserialize);
            Models.Product product = db.Products.Include(x => x.Pictures).FirstOrDefault(p => p.ProductId == id);
            int index = getIndexOfProductInCart(product, CartItems);
            CartItems[index].Quantity++;
            total = GetTotal(CartItems);

            string cartsserialize = JsonSerializer.Serialize(CartItems);
            HttpContext.Session.SetString("CartSession", cartsserialize);

            string totalserialize = JsonSerializer.Serialize(total);
            HttpContext.Session.SetString("total", totalserialize);

            if (CartItems.Count == 0)
            {
                ViewData["emptymess"] = "Cart empty!";
            }
            ViewData["total"] = total;
            return RedirectToPage("/Product/Cart");
        }

        public IActionResult OnGetMinus(int id)
        {
            string? cartsdeserialize = HttpContext.Session.GetString("CartSession");
            CartItems = JsonSerializer.Deserialize<List<Cart>>(cartsdeserialize);
            Models.Product product = db.Products.Include(x => x.Pictures).FirstOrDefault(p => p.ProductId == id);
            int index = getIndexOfProductInCart(product, CartItems);
            CartItems[index].Quantity--;

            if (CartItems[index].Quantity == 0) CartItems.Remove(CartItems[index]);

            total = GetTotal(CartItems);

            string cartsserialize = JsonSerializer.Serialize(CartItems);
            HttpContext.Session.SetString("CartSession", cartsserialize);

            string totalserialize = JsonSerializer.Serialize(total);
            HttpContext.Session.SetString("total", totalserialize);

            if (CartItems.Count == 0)
            {
                ViewData["emptymess"] = "Cart empty!";
            }
            ViewData["total"] = total;
            return RedirectToPage("/Product/Cart");
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
                if (carts[i].Product.ProductId == product.ProductId) return i;
            }
            return -1;
        }

        private string GenerateCusID(int length)
        {
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }
    }
}
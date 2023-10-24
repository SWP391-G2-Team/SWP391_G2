using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.ListProducts
{
    public class ListModel : PageModel
    {
        private readonly DTB_PETSHOPContext _projectContext;
        public List<CategoryProductModel> CategoryProducts { get; set; }
        public bool IsAllProducts { get; set; }

        public ListModel(DTB_PETSHOPContext projectContext)
        {
            _projectContext = projectContext;
        }

        public IActionResult OnGet(bool isAllProducts = false, int? categoryId = null)
        {
            IsAllProducts = isAllProducts;

            if (IsAllProducts)
            {
                CategoryProducts = new List<CategoryProductModel>
                {
                    new CategoryProductModel
                    {
                        Category = new Category { CategoryName = "Tất cả sản phẩm" },
                        Products = _projectContext.Products
                            .Include(p => p.Pictures)
                            .ToList()
                    }
                };
            }
            else if (categoryId.HasValue)
            {
                CategoryProducts = new List<CategoryProductModel>
                {
                    new CategoryProductModel
                    {
                        Category = _projectContext.Categories
                            .FirstOrDefault(c => c.CategoryId == categoryId),
                        Products = _projectContext.Products
                            .Include(p => p.Pictures)
                            .Where(p => p.CategoryId == categoryId)
                            .ToList()
                    }
                };
            }
            else
            {
                CategoryProducts = _projectContext.Categories
                    .Include(c => c.Products)
                    .ThenInclude(p => p.Pictures)
                    .Select(c => new CategoryProductModel
                    {
                        Category = c,
                        Products = c.Products.ToList()
                    })
                    .ToList();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Xử lý khi yêu cầu là Ajax
                if (categoryId.HasValue)
                {
                    var products = _projectContext.Products
                        .Include(p => p.Pictures)
                        .Where(p => p.CategoryId == categoryId)
                        .ToList();
                    return Partial("_CategoryProductsPartial", products);
                }
                else
                {
                    var allProducts = _projectContext.Products
                        .Include(p => p.Pictures)
                        .ToList();
                    return Partial("_CategoryProductsPartial", allProducts);
                }
            }

            return Page();
        }

        public class CategoryProductModel
        {
            public Category Category { get; set; }
            public List<PetShopOnline.Models.Product> Products { get; set; }
        }
    }
}

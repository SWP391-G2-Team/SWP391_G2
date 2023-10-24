using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Pages.Manager.Products
{
    [Authorize(Roles = "1")]
    public class DetailModel : PageModel
    {
            private readonly DTB_PETSHOPContext _context;
            public DetailModel(DTB_PETSHOPContext context)
            {
                _context = context;
            }

            public Models.Product Product { get; set; }
            [BindProperty]
            public @Models.Account Account { get; set; }
            [BindProperty]
            public Customer Customer { get; set; }
            public Models.Employee Employee { get; set; }
            public async Task<IActionResult> OnGetAsync(int? id)
            {
                if (HttpContext.Session.GetString("PetSession") == null) return RedirectToPage("/Account/SignIn");

                Account = JsonSerializer.Deserialize<@Models.Account>(HttpContext.Session.GetString("PetSession"));
                //Customer = dBContext.Customers.FirstOrDefault(c => c.CustomerId == Account.CustomerId);

                Employee = _context.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);
                if (id == null || _context.Products == null)
                {
                    return NotFound();
                }
                var product = await _context.Products.Include(x => x.Pictures)
                    .Include(p => p.Category).FirstOrDefaultAsync(c => c.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    Product = product;
                }
                return Page();
            }
        }
    }


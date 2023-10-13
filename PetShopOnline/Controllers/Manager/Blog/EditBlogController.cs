using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Text.Json;

namespace PetShopOnline.Controllers.Manager.Blog
{
    public class EditBlogController : Controller
    {
        [Authorize(Roles = "1")]
        public class EditModel : PageModel
        {
            private readonly PetShopOnline.Models.DTB_PETSHOPContext _context;

            public EditModel(PetShopOnline.Models.DTB_PETSHOPContext context)
            {
                _context = context;
            }

            [BindProperty]
            public BlogDetail BlogDetail { get; set; }
            [BindProperty]
            public Models.Account Account { get; set; }

            [BindProperty]
            public Models.Employee Employee { get; set; }

            public async Task<IActionResult> OnGetAsync(int? id)
            {
                if (HttpContext.Session.GetString("PetSession") == null)
                    return RedirectToPage("/account/singnin");

                Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
                Employee = _context.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);

                if (id == null || _context.BlogDetails == null)
                {
                    return NotFound();
                }

                var blogdetail = await _context.BlogDetails.FirstOrDefaultAsync(m => m.BlogId == id);
                if (blogdetail == null)
                {
                    return NotFound();
                }
                BlogDetail = blogdetail;
                return Page();
            }

            public async Task<IActionResult> OnPostAsync(IFormFile imageFile, int id)
            {
                var blg = await _context.BlogDetails.FirstOrDefaultAsync(m => m.BlogId == id);
                if (imageFile != null && imageFile.Length > 0)
                {
                    Console.WriteLine(imageFile);
                    blg.FeaturedImageUrl = "/img/" + imageFile.FileName;


                }
                var today = DateTime.Today;
                if (blg != null)
                {
                    blg.Content = BlogDetail.Content;
                    blg.Heading = BlogDetail.Heading;
                    blg.PageTitle = BlogDetail.PageTitle;
                    blg.ShortDescription = BlogDetail.ShortDescription;
                    blg.PublishedDate = BlogDetail.PublishedDate;

                    if (blg.PublishedDate < today || blg.PublishedDate > today) // Nếu ngày đăng nhỏ hơn ngày hôm nay, thì đặt ngày đăng bằng ngày hôm nay
                    {
                        blg.PublishedDate = today;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Manager/Blog/List");
                }
                else
                {
                    return Page();
                }
            }

            private bool BlogDetailExists(int id)
            {
                return (_context.BlogDetails?.Any(e => e.BlogId == id)).GetValueOrDefault();
            }
        }
    }
}

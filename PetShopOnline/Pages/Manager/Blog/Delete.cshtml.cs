/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;

namespace PetShop_Project_SWP391.Pages.Manager.Blog
{
    public class DeleteModel : PageModel
    {
        private readonly PetShop_Project_SWP391.Models.ProjectContext _context;

        public DeleteModel(PetShop_Project_SWP391.Models.ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BlogDetail BlogDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.BlogDetails == null)
            {
                return NotFound();
            }

            var blogdetail = await _context.BlogDetails.FirstOrDefaultAsync(m => m.BlogId == id);

            if (blogdetail == null)
            {
                return NotFound();
            }
            else
            {
                BlogDetail = blogdetail;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.BlogDetails == null)
            {
                return NotFound();
            }
            var blogdetail = await _context.BlogDetails.FindAsync(id);

            if (blogdetail != null)
            {
                BlogDetail = blogdetail;
                _context.BlogDetails.Remove(BlogDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Manager/Blog/List");
        }
    }
}
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop_Project_SWP391.Models;

namespace PetShop_Project_SWP391.Pages.Manager.Blog
{
    public class DeleteModel : PageModel
    {
        private readonly ProjectContext _context;

        public DeleteModel(ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BlogDetail BlogDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BlogDetail = await _context.BlogDetails.FirstOrDefaultAsync(m => m.BlogId == id);

            if (BlogDetail == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (BlogDetail == null)
            {
                return NotFound();
            }

            var blogDetailToDelete = await _context.BlogDetails.FindAsync(BlogDetail.BlogId);

            if (blogDetailToDelete != null)
            {
                _context.BlogDetails.Remove(blogDetailToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Manager/Blog/List");
        }
    }
}

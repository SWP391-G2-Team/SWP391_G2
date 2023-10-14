using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.Manager.Blog
{
    public class DeleteModel : PageModel
    {
        private readonly DTB_PETSHOPContext _context;

        public DeleteModel(DTB_PETSHOPContext context)
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
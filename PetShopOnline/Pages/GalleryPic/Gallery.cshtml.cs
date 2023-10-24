using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;

namespace PetShopOnline.Pages.GalleryPic
{
    public class GalleryModel : PageModel
    {
        private readonly DTB_PETSHOPContext _context;

        public GalleryModel(DTB_PETSHOPContext context)
        {
            _context = context;
        }

        public IList<PictureGallery> Pictures { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Pictures = await _context.PictureGalleries.ToListAsync();
            return Page();
        }
    }
}

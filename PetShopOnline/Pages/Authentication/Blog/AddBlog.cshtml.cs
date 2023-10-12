using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShop_Project_SWP391.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetShop_Project_SWP391.Pages.Manager.Blog
{
    [Authorize(Roles = "1")]
    public class AddBlogModel : PageModel
    {
        private readonly ProjectContext dBContext;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AddBlogModel(ProjectContext dBContext, IWebHostEnvironment hostingEnvironment)
        {
            this.dBContext = dBContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public Models.BlogDetail BlogDetail { get; set; }

        [BindProperty]
        public Models.Account Account { get; set; }

        [BindProperty]
        public Models.Employee Employee { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("PetSession") == null)
                return RedirectToPage("/account/singnin");

            Account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("PetSession"));
            Employee = dBContext.Employees.FirstOrDefault(c => c.EmployeeId == Account.EmployeeId);

            return Page();
        }
        public async Task<IActionResult> OnPost(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                Console.WriteLine(imageFile);
                BlogDetail.FeaturedImageUrl = "/img/" + imageFile.FileName.ToString();

                //dBContext.BlogDetails.Add(BlogDetail);

            }

            var today = DateTime.Today;

            var blog = new BlogDetail()
            {
                Content = BlogDetail.Content,
                Heading = BlogDetail.Heading,
                PageTitle = BlogDetail.PageTitle,
                ShortDescription = BlogDetail.ShortDescription,
                FeaturedImageUrl = BlogDetail.FeaturedImageUrl,
                PublishedDate = DateTime.Now
            };

            if (blog.PublishedDate < today || blog.PublishedDate > today) // Nếu ngày đăng nhỏ hơn ngày hôm nay, thì đặt ngày đăng bằng ngày hôm nay
            {
                blog.PublishedDate = today;
            }

            dBContext.BlogDetails.Add(blog);
            await dBContext.SaveChangesAsync();

            ViewData["msgSuccess"] = "Tạo Blog thành công";
            return RedirectToPage("/manager/Blog/List");


        }
    
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using PetShopOnline.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DTB_PETSHOPContext>();
builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(5));
builder.Services.AddSignalR();
//cookie authen
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login_Page";
        options.AccessDeniedPath = "/error";
    });

var app = builder.Build();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
//app.UseCookiePolicy();
app.MapRazorPages();
app.Run();

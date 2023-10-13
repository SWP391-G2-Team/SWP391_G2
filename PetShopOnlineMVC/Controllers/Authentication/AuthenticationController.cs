using Microsoft.AspNetCore.Mvc;
using PetShopOnlineMVC.Models;

namespace PetShopOnlineMVC.Controllers.Authentication
{

    public class AuthenticationController : Controller
    {

        DtbPetshopContext _context;

        public AuthenticationController()
        {
            _context = new DtbPetshopContext();
        }

        #region login
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(IFormCollection iFormCollection)
        {
            string email = Request.Form["email"];
            string pwd = Request.Form["password"];
            Account account = _context.Accounts.
                SingleOrDefault(a => a.Email == email && a.Password == pwd);
            if (account == null)
            {
                ViewData["LoginFail"] = "Wrong email and password! Try again!";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("account", email);
                return RedirectToAction("ProductList", "Product");
            }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("account");
            return RedirectToAction("Login");
        }
        #endregion

        #region register
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(IFormCollection iFormCollection)
        {
            string fullName = Request.Form["fullName"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];
            string pwd = Request.Form["password"];
            string address = Request.Form["address"];
            string district = Request.Form["district"];
            string province = Request.Form["province"];
            string wards = Request.Form["wards"];

            // Generate a new UUID (GUID)
            Guid newGuid = Guid.NewGuid();

            string userId = newGuid.ToString();

            //Get current date
            DateTime date = DateTime.Now;

            //create new customer
            Customer customer = new Customer
            {
                CustomerId = userId,
                CreateDate = date,
                Name = fullName,
                District = district,
                Province = province,
                Wards = wards,
                Address = address,
            };

            //Create new account
            Account acc = new Account
            {
                CustomerId = userId,
                Email = email,
                Password = pwd,
                IsActive = true,
                Role = 2,
            };

            _context.Customers.Add(customer);
            _context.Accounts.Add(acc);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
        #endregion

        #region change password
        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            string email = HttpContext.Session.GetString("account");
            if (!string.IsNullOrEmpty(email))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(IFormCollection iFormCollection)
        {
            string email = HttpContext.Session.GetString("account");
            string oldPass = Request.Form["oldPass"];
            string newPass = Request.Form["newPass"];

            Account acc = _context.Accounts.
                SingleOrDefault(a => a.Email == email && a.Password == oldPass);
            if (acc == null)
            {
                ViewData["WrongPass"] = "Old password is not correct";
                return View();
            }
            else
            {
                acc.Password = newPass;
                _context.Entry(acc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

        }
        #endregion
    }
}

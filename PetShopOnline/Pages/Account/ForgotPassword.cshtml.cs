using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopOnline.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using System;

namespace PetShopOnline.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly DTB_PETSHOPContext projectContext;

        public ForgotPasswordModel(DTB_PETSHOPContext projectContext)
        {
            this.projectContext = projectContext;
        }

        [BindProperty]
        public Models.Account Account { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Account.Email))
            {
                ViewData["msgEmpty"] = "Vui lòng nhập địa chỉ email đã được đăng ký";
                return Page();
            }
            bool checkResetPass = false;
            string newPassword = GeneratePassword(10);
            var acc = await projectContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Account.Email));
            if (acc != null)
            {
                try
                {
                    // Gửi email chứa mật khẩu mới
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("petshoponline134@gmail.com");
                        mail.To.Add(Account.Email);
                        mail.Subject = "Password Reset";
                        mail.Body = $"Hi {Account.Email},\n\nYour new password is: {newPassword}.\n\nThank you.";

                        using (SmtpClient smtpClient = new SmtpClient())
                        {
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential("petshoponline134@gmail.com", "zaby twuk rqde ujhj");
                            smtpClient.EnableSsl = true;
                            smtpClient.Port = 587;
                            smtpClient.Host = "smtp.gmail.com";
                            smtpClient.Send(mail);
                        }
                    }
                    ViewData["message"] = "Xác nhận tài khoản trên Email trong Hộp thư đến (Inbox) hoặc Thư rác (Spam)";
                    checkResetPass = true;
                }
                catch
                {
                    ViewData["error"] = "Gửi mail lỗi!";
                    checkResetPass = false;
                }
            }
            else
            {
                ViewData["msg"] = "Tài khoản không tồn tại trong hệ thống";
                return Page();
            }
            if (checkResetPass)
            {
                Account.Password = newPassword;
                acc.Password = newPassword;
                ViewData["messageSuccess"] = "Lấy mật khẩu thành công. Thư xác nhận có thể trong Hộp thư đến(Inbox) hoặc Thư rác(Spam).";
                await projectContext.SaveChangesAsync();
            }
            return Page();
        }

        public string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            StringBuilder sb = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }
    }
}

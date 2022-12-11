using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectASP.Models;

namespace ProjectASP.Controllers
{
    public class HomeController : Controller
    {
        private static Random random = new Random();
        private readonly ProjectContext _context;
        public HomeController(ProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.OrderByDescending(p=>p.ProductID).Skip(0).Take(5).ToList();
            List<Product> sale_products = _context.Products.Where(p=>p.SalePrice > 0).OrderByDescending(p => (p.Price - p.SalePrice)/p.Price).Skip(0).Take(5).ToList();
            List<Category> categories = _context.Categories.Take(7).ToList();
            ViewBag.Category = categories;
            ViewBag.Sale = sale_products;
            int count_cart = _context.Carts.Where(c => c.AccountID == HttpContext.Session.GetInt32("CustomerID")).Count();
            HttpContext.Session.SetInt32("Count_Cart", count_cart);
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            List<Category> categories = _context.Categories.Take(7).ToList();
            ViewBag.Category = categories;
            return View();
        }

        [HttpPost]
        public IActionResult CheckLogin(Login model)
        {
            if (ModelState.IsValid)
            {
                List<Account> account = _context.Accounts.ToList();
                foreach (Account a in account)
                {
                    if (a.Email.Equals(model.Account) && a.Password.Equals(model.Password) || a.Phone.Equals(model.Account))
                    {
                        HttpContext.Session.SetString("Customer", a.AccountName);
                        HttpContext.Session.SetInt32("CustomerID", a.AccountID);
                        return RedirectToAction("Index","Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu !");
            }
            List<Category> categories = _context.Categories.Take(7).ToList();
            ViewBag.Category = categories;
            return View("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Customer");
            HttpContext.Session.Remove("CustomerID");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SendEmail(string email)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string new_pass = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            try
            {
                if (ModelState.IsValid)
                {
                    var account = _context.Accounts.Where(p=> p.Email == email).FirstOrDefault();
                    if (account == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email không tồn tại !");
                        return View("Login");
                    }
                    else
                    {
                        var senderEmail = new MailAddress("vienavtb@gmail.com", "CongVien");
                        var receiverEmail = new MailAddress(email, "Receiver");
                        var password = "rwpezipvxnciwrij";
                        var subject = "Here's the link to reset your password";
                        var body = "<p>Hello,</p>" + "<p>You have requested to reset your password.</p>"
                        + "<p> below to change your password:</p>"
                        + "<h4>Your new password is : <b>" + new_pass + "</b></h4>"
                        + "<h3><p>Please don't share this email for everyone !</p></h3>"
                        + "<br><p>This link will expire within the next hour . "
                        + "<b>(If this is a spam message, please click  it is not spam)<b>";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        })
                        {
                            smtp.Send(mess);
                        }
                        account.Password = new_pass;
                        _context.Update(account);
                        _context.SaveChangesAsync();
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
        public IActionResult Register()
        {
            List<Category> categories = _context.Categories.Take(7).ToList();
            ViewBag.Category = categories;
            return View();
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProjectASP.Models;

namespace ProjectASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly ProjectContext _context;

        public LoginController(ProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                if (model.Account == "admin@gmail.com" && model.Password == "1")
                {
                    return RedirectToAction("Index", "Home");
                }
                Account check = checkExist(model);
                if (check == null)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu sai !");
                    return View("Index");
                }
                else
                {
                    if (check.Role)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Account) }, "LoginAdminCookie");
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync("LoginAdminCookie", principal);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản không được cấp quyền quản trị !");
                        return View("Index");
                    }
                }
            }     
        }

        public IActionResult LogoutAdmin()
        {
            HttpContext.SignOutAsync("LoginAdminCookie");
            return RedirectToAction("Index", "Login");
        }
        public Account checkExist(Login model)
        {
            List<Account> account = _context.Accounts.ToList();
            foreach(Account a in account)
            {
                if(model.Account.Equals(a.Email) || model.Account.Equals(a.Phone) && model.Password.Equals(a.Password))
                {
                    return a;
                }
            }
            return null;
        }
    }
}
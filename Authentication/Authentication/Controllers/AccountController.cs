using Authentication.Models;
using Authentication.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _appContext;

        public AccountController(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                User user = _appContext.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash);
            
                if(user != null)
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login or password");                    
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                User user = _appContext.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash);

                if(user == null)
                {
                    _appContext.Users.Add(new User
                    {
                        Email = model.Email,
                        Password = passwordHash
                    });

                    await _appContext.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError("", "Email is already taken");               
            }
            return View(model);
        }


        private async Task Authenticate(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
            };

            ClaimsIdentity identity = new ClaimsIdentity(
                claims, 
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
             );

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}

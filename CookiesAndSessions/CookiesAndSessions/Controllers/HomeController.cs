using CookiesAndSessions.Extentions;
using CookiesAndSessions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CookiesAndSessions.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if( HttpContext.Request.Cookies.ContainsKey("lang"))
            {
                ViewBag.Lang = HttpContext.Request.Cookies["lang"];
            }
            else
            {
                HttpContext.Response.Cookies.Append("lang", "en");
            }


            if(HttpContext.Session.Keys.Contains("name"))
            {

                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Cart = (Cart)HttpContext.Session.GetObject("cart", typeof(Cart));
            }
            else
            {
                HttpContext.Session.SetString("name", "John");

                Cart cart = new Cart { Count = 12, DateTime = DateTime.Now };

                HttpContext.Session.SetObject("cart", cart, typeof(Cart));
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

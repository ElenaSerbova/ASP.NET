using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCRouting.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCRouting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BlogContext _blogContext;

        public HomeController(ILogger<HomeController> logger, BlogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _blogContext.Posts.ToListAsync());
        }

        public async Task<IActionResult> Details(string articleName)
        {
            return View(await _blogContext.Posts
                .Include(p=>p.PostTags)
                .ThenInclude(pt=>pt.Tag)
                .FirstOrDefaultAsync(p=>p.Title == articleName));
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

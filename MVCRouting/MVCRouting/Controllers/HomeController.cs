using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCRouting.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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

        [Route("")]
        [Route("Home")]
        [Route("[controller]/[action]")]        
        public async Task<IActionResult> Index()
        {
            var posts = await _blogContext.Posts.ToListAsync();
            return View(posts);
        }

        [Route("article/{articleName:slugify}")]
        [Route("post/{articleName:slugify}")]
        public async Task<IActionResult> Details(string articleName)
        {
            string title = Regex.Replace(articleName,
                                 @"-",
                                 " ",
                                 RegexOptions.CultureInvariant);

            var post = await _blogContext.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Title.ToLower() == title);

            return View(post);
        }


        [Route("tag/{tagName}")]
        public async Task<IActionResult> GetByTag(string tagName)
        {
            var posts = _blogContext.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .SelectMany(p => p.PostTags
                                    .Where(pt => pt.Tag.Name == tagName)
                                    .Select(pt => pt.Post));
                
            return View("Index", posts);
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

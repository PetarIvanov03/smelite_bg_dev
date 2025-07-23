using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smelite_app.Services;
using smelite_app.Models;

namespace smelite_app.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetPublishedAsync();
            return View(posts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var post = await _blogService.GetPublishedByIdAsync(id.Value);
            if (post == null) return NotFound();
            return View(post);
        }
    }
}

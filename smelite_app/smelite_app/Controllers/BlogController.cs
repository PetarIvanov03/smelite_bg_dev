using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smelite_app.Services;
using smelite_app.ViewModels.Blog;
using System.Linq;

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
            var vm = posts.Select(p => new BlogPostListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                AuthorName = p.Author != null ? $"{p.Author.FirstName} {p.Author.LastName}" : null,
                CreatedAt = p.CreatedAt,
                IsPublished = p.IsPublished
            }).ToList();
            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var post = await _blogService.GetPublishedByIdAsync(id.Value);
            if (post == null) return NotFound();

            var vm = new BlogPostDetailsViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CoverImageUrl = post.CoverImageUrl,
                AuthorName = post.Author != null ? $"{post.Author.FirstName} {post.Author.LastName}" : string.Empty,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                IsPublished = post.IsPublished
            };

            return View(vm);
        }
    }
}

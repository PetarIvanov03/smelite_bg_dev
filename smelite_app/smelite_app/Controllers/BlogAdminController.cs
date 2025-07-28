using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smelite_app.Services;
using smelite_app.Models;
using smelite_app.ViewModels.Blog;
using System.Linq;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogAdminController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogAdminController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetAllAsync();
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
            var post = await _blogService.GetByIdAsync(id.Value);
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost
                {
                    Title = vm.Title,
                    Content = vm.Content,
                    CoverImageUrl = vm.CoverImageUrl,
                    IsPublished = true,
                    AuthorId = _userManager.GetUserId(User),
                    CreatedAt = DateTime.UtcNow
                };

                await _blogService.AddAsync(blogPost);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _blogService.GetByIdAsync(id.Value);
            if (blogPost == null) return NotFound();

            var vm = new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                CoverImageUrl = blogPost.CoverImageUrl,
                IsPublished = blogPost.IsPublished
            };

            ViewData["BlogPostId"] = blogPost.Id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BlogPostId"] = id;
                return View(vm);
            }

            var blogPost = await _blogService.GetByIdAsync(id);
            if (blogPost == null) return NotFound();

            try
            {
                blogPost.Title = vm.Title;
                blogPost.Content = vm.Content;
                blogPost.CoverImageUrl = vm.CoverImageUrl;
                blogPost.IsPublished = vm.IsPublished;
                blogPost.UpdatedAt = DateTime.UtcNow;

                await _blogService.UpdateAsync(blogPost);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _blogService.GetByIdAsync(blogPost.Id) == null)
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var post = await _blogService.GetByIdAsync(id.Value);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

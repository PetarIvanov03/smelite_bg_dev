using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smelite_app.Services;
using smelite_app.Models;

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
            return View(posts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var post = await _blogService.GetByIdAsync(id.Value);
            if (post == null) return NotFound();
            return View(post);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.AuthorId = _userManager.GetUserId(User);
                blogPost.CreatedAt = DateTime.UtcNow;
                await _blogService.AddAsync(blogPost);
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _blogService.GetByIdAsync(id.Value);
            if (blogPost == null) return NotFound();
            return View(blogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
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
            return View(blogPost);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var post = await _blogService.GetByIdAsync(id.Value);
            if (post == null) return NotFound();
            return View(post);
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

using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Helpers;

namespace smelite_app.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repo;
        private readonly EmailSender _emailSender;
        private readonly IEmailSubscriptionService _subscriptionService;

        public BlogService(IBlogRepository repo, EmailSender emailSender, IEmailSubscriptionService subscriptionService)
        {
            _repo = repo;
            _emailSender = emailSender;
            _subscriptionService = subscriptionService;
        }

        public Task<List<BlogPost>> GetAllAsync()
        {
            return _repo.GetAll().OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public Task<List<BlogPost>> GetPublishedAsync()
        {
            return _repo.GetAll().Where(p => p.IsPublished).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public Task<BlogPost?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task<BlogPost?> GetPublishedByIdAsync(int id)
        {
            return _repo.GetAll().Where(p => p.IsPublished).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(BlogPost post)
        {
            await _repo.AddAsync(post);
            var emails = await _subscriptionService.GetActiveEmailsAsync();
            foreach (var email in emails)
            {
                var link = $"/Blog/Details/{post.Id}";
                await _emailSender.SendEmailAsync(email, post.Title, $"{post.Content}<br/><a href='{link}'>Прочети повече</a>");
            }
        }

        public Task UpdateAsync(BlogPost post)
        {
            return _repo.UpdateAsync(post);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post != null)
            {
                await _repo.DeleteAsync(post);
            }
        }
    }
}

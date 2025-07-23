using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using smelite_app.Repositories;

namespace smelite_app.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repo;
        public BlogService(IBlogRepository repo)
        {
            _repo = repo;
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

        public Task AddAsync(BlogPost post)
        {
            return _repo.AddAsync(post);
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

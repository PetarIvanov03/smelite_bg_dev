using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<BlogPost> GetAll()
        {
            return _context.BlogPosts.Include(p => p.Author);
        }

        public Task<BlogPost?> GetByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(BlogPost post)
        {
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BlogPost post)
        {
            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BlogPost post)
        {
            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface IBlogRepository
    {
        IQueryable<BlogPost> GetAll();
        Task<BlogPost?> GetByIdAsync(int id);
        Task AddAsync(BlogPost post);
        Task UpdateAsync(BlogPost post);
        Task DeleteAsync(BlogPost post);
    }
}

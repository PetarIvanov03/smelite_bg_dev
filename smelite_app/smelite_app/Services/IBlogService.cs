using smelite_app.Models;

namespace smelite_app.Services
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetAllAsync();
        Task<List<BlogPost>> GetPublishedAsync();
        Task<BlogPost?> GetByIdAsync(int id);
        Task<BlogPost?> GetPublishedByIdAsync(int id);
        Task AddAsync(BlogPost post);
        Task UpdateAsync(BlogPost post);
        Task DeleteAsync(int id);
    }
}

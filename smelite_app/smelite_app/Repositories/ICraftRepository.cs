using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface ICraftRepository
    {
        IQueryable<MasterProfile> GetAll();
        Task<MasterProfile?> GetByIdAsync(int id);
    }
}

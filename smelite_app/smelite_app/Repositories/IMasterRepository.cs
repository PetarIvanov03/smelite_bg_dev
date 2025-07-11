using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface IMasterRepository
    {
        IQueryable<MasterProfile> GetAll();
        Task<MasterProfile?> GetByIdAsync(int id);
    }
}

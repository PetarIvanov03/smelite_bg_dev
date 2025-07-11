using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface IMasterRepository
    {
        Task AddProfileAsync(MasterProfile profile);
    }
}

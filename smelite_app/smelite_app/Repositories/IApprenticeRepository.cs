using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface IApprenticeRepository
    {
        Task AddProfileAsync(ApprenticeProfile profile);
    }
}
using smelite_app.Models;
using smelite_app.Repositories;

namespace smelite_app.Services
{
    public class MasterService : IMasterService
    {
        private readonly IMasterRepository _masterRepository;

        public Task<MasterProfile?> GetByUserIdAsync(string userId)
        {
            return _masterRepository.GetByUserIdAsync(userId);
        }

        public Task UpdateProfileAsync(MasterProfile profile)
        {
            return _masterRepository.UpdateProfileAsync(profile);
        }

        public Task AddCraftAsync(int masterProfileId, Craft craft)
        {
            return _masterRepository.AddCraftAsync(masterProfileId, craft);
        }

        public Task<List<Craft>> GetCraftsAsync(int masterProfileId)
        {
            return _masterRepository.GetCraftsAsync(masterProfileId);
        }

        public Task<Craft?> GetCraftByIdAsync(int craftId)
        {
            return _masterRepository.GetCraftByIdAsync(craftId);
        }

        public Task UpdateCraftAsync(Craft craft)
        {
            return _masterRepository.UpdateCraftAsync(craft);
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId)
        {
            return _masterRepository.GetApprenticeshipsAsync(masterProfileId);
        }
    }
}

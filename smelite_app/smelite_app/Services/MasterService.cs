using smelite_app.Models;
using smelite_app.Repositories;

namespace smelite_app.Services
{
    public class MasterService : IMasterService
    {
        private readonly IMasterRepository _masterRepository;

        public MasterService(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository
                ?? throw new ArgumentNullException(nameof(masterRepository));
        }
        public Task<MasterProfile?> GetByUserIdAsync(string userId)
        {
            return _masterRepository.GetByUserIdAsync(userId);
        }

        public Task UpdateProfileAsync(MasterProfile profile)
        {
            return _masterRepository.UpdateProfileAsync(profile);
        }

        public Task AddCraftAsync(int masterProfileId, Craft craft, IEnumerable<CraftOffering> offerings, IEnumerable<CraftImage>? images)
        {
            return _masterRepository.AddCraftAsync(masterProfileId, craft, offerings, images);
        }

        public Task<List<Craft>> GetCraftsAsync(int masterProfileId)
        {
            return _masterRepository.GetCraftsAsync(masterProfileId);
        }

        public Task<Craft?> GetCraftByIdAsync(int craftId)
        {
            return _masterRepository.GetCraftByIdAsync(craftId);
        }

        public Task UpdateCraftAsync(Craft craft, IEnumerable<CraftOffering> offerings, IEnumerable<int>? removeImageIds, IEnumerable<CraftImage>? newImages)
        {
            return _masterRepository.UpdateCraftAsync(craft, offerings, removeImageIds, newImages);
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId)
        {
            return _masterRepository.GetApprenticeshipsAsync(masterProfileId);
        }
    }
}

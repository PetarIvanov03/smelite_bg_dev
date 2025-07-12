using smelite_app.Enums;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.ViewModels.Apprentice;

namespace smelite_app.Services
{
    public class ApprenticeService : IApprenticeService
    {
        private readonly IApprenticeRepository _apprenticeRepository;
        private readonly ICraftRepository _craftRepository;

        public ApprenticeService(IApprenticeRepository apprenticeRepository, ICraftRepository craftRepository)
        {
            _apprenticeRepository = apprenticeRepository ?? throw new ArgumentNullException(nameof(apprenticeRepository));
            _craftRepository = craftRepository ?? throw new ArgumentNullException(nameof(craftRepository));
        }

        public Task<ApprenticeProfile?> GetByUserIdAsync(string userId)
        {
            return _apprenticeRepository.GetByUserIdAsync(userId);
        }

        public async Task<ApprenticeProfileViewModel?> GetProfileAsync(string userId)
        {
            var profile = await _apprenticeRepository.GetByUserIdAsync(userId);
            if (profile == null) return null;

            return new ApprenticeProfileViewModel
            {
                FirstName = profile.ApplicationUser.FirstName,
                LastName = profile.ApplicationUser.LastName,
                Email = profile.ApplicationUser.Email!,
                ProfileImageUrl = profile.ApplicationUser.ProfileImageUrl,
                PersonalInformation = profile.PersonalInformation
            };
        }

        public Task UpdateProfileAsync(ApprenticeProfile profile)
        {
            return _apprenticeRepository.UpdateProfileAsync(profile);
        }

        public async Task AddApprenticeshipAsync(int apprenticeProfileId, int craftOfferingId)
        {
            var offering = await _craftRepository.GetCraftOfferingByIdAsync(craftOfferingId) ?? throw new InvalidOperationException();
            var masterProfileId = offering.Craft.MasterProfileCrafts.First().MasterProfileId;

            var apprenticeship = new Apprenticeship
            {
                ApprenticeProfileId = apprenticeProfileId,
                MasterProfileId = masterProfileId,
                CraftOfferingId = craftOfferingId,
                Status = ApprenticeshipStatus.Pending.ToString()
            };

            await _apprenticeRepository.AddApprenticeshipAsync(apprenticeship);
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int apprenticeProfileId)
        {
            return _apprenticeRepository.GetApprenticeshipsAsync(apprenticeProfileId);
        }
    }
}

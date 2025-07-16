using System;
using smelite_app.Enums;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.ViewModels.Apprentice;
using smelite_app.Helpers;

namespace smelite_app.Services
{
    public class ApprenticeService : IApprenticeService
    {
        private readonly IApprenticeRepository _apprenticeRepository;
        private readonly ICraftRepository _craftRepository;
        private readonly EmailSender _emailSender;

        public ApprenticeService(IApprenticeRepository apprenticeRepository, ICraftRepository craftRepository, EmailSender emailSender)
        {
            _apprenticeRepository = apprenticeRepository ?? throw new ArgumentNullException(nameof(apprenticeRepository));
            _craftRepository = craftRepository ?? throw new ArgumentNullException(nameof(craftRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
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

        public async Task<Payment> AddApprenticeshipAsync(int apprenticeProfileId, int craftOfferingId)
        {
            var offering = await _craftRepository.GetCraftOfferingByIdAsync(craftOfferingId)
                ?? throw new InvalidOperationException();
            var masterProfileId = offering.Craft.MasterProfileCrafts.First().MasterProfileId;

            var total = offering.Price;
            var fee = Math.Round(total * 0.1m, 2);

            var apprenticeship = new Apprenticeship
            {
                ApprenticeProfileId = apprenticeProfileId,
                MasterProfileId = masterProfileId,
                CraftOfferingId = craftOfferingId,
                Status = ApprenticeshipStatus.Pending.ToString(),
                Payment = new Payment
                {
                    PayerProfileId = apprenticeProfileId,
                    RecipientProfileId = masterProfileId,
                    AmountTotal = total,
                    PlatformFee = fee,
                    AmountToRecipient = total - fee,
                    PaidOn = DateTime.UtcNow,
                    Method = "Unknown",
                    Status = PaymentStatus.Pending.ToString(),
                    TransactionId = Guid.NewGuid().ToString()
                }
            };

            await _apprenticeRepository.AddApprenticeshipAsync(apprenticeship);
            await _emailSender.SendEmailAsync(Variables.defaultEmail,
                "Apprenticeship requested",
                $"Apprentice {apprenticeProfileId} requested offering {craftOfferingId}.");

            return apprenticeship.Payment;
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int apprenticeProfileId)
        {
            return _apprenticeRepository.GetApprenticeshipsAsync(apprenticeProfileId);
        }
    }
}

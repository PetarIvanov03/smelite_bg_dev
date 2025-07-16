using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.ViewModels.Master;
using smelite_app.Helpers;

namespace smelite_app.Services
{
    public class MasterService : IMasterService
    {
        private readonly IMasterRepository _masterRepository;
        private readonly EmailSender _emailSender;

        public MasterService(IMasterRepository masterRepository, EmailSender emailSender)
        {
            _masterRepository = masterRepository
                ?? throw new ArgumentNullException(nameof(masterRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public Task<MasterProfile?> GetByUserIdAsync(string userId)
        {
            return _masterRepository.GetByUserIdAsync(userId);
        }

        public async Task<MasterProfileViewModel?> GetProfileAsync(string userId)
        {
            var profile = await _masterRepository.GetByUserIdAsync(userId);
            if (profile == null) return null;

            return new MasterProfileViewModel
            {
                FirstName = profile.ApplicationUser.FirstName,
                LastName = profile.ApplicationUser.LastName,
                Email = profile.ApplicationUser.Email!,
                ProfileImageUrl = profile.ApplicationUser.ProfileImageUrl,
                PersonalInformation = profile.PersonalInformation,
                StripeConnected = !string.IsNullOrWhiteSpace(profile.StripeAccountId)
                    && profile.StripeAccountId.StartsWith("acct_")
            };
        }

        public Task UpdateProfileAsync(MasterProfile profile)
        {
            return _masterRepository.UpdateProfileAsync(profile);
        }

        public async Task AddCraftAsync(int masterProfileId, CraftViewModel model, string webRootPath, string userId)
        {
            var craft = new Craft
            {
                Name = model.Name,
                CraftDescription = model.CraftDescription,
                ExperienceYears = model.ExperienceYears,
                CraftTypeId = model.CraftTypeId
            };

            var offerings = model.Offerings.Select(o => new CraftOffering
            {
                CraftLocation = new CraftLocation { Name = o.LocationName },
                CraftPackage = new CraftPackage { SessionsCount = o.SessionsCount, Label = o.PackageLabel },
                Price = o.Price
            }).ToList();

            var images = new List<CraftImage>();
            if (model.Images != null)
            {
                var uploads = Path.Combine(webRootPath, "CraftsImages");
                Directory.CreateDirectory(uploads);
                foreach (var file in model.Images)
                {
                    if (file.Length == 0) continue;
                    var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploads, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                    images.Add(new CraftImage { ImageUrl = "/CraftsImages/" + fileName });
                }
            }
            else
            {
                images.Add(new CraftImage { ImageUrl = Variables.defaultCraftImageUrl });
            }

            await _masterRepository.AddCraftAsync(masterProfileId, craft, offerings, images);
            await _emailSender.SendEmailAsync(Variables.defaultEmail,
                "Craft created",
                $"Master {masterProfileId} created craft '{craft.Name}'.");
        }

        public Task<List<Craft>> GetCraftsAsync(int masterProfileId)
        {
            return _masterRepository.GetCraftsAsync(masterProfileId);
        }

        public Task<Craft?> GetCraftByIdAsync(int craftId)
        {
            return _masterRepository.GetCraftByIdAsync(craftId);
        }

        public async Task UpdateCraftAsync(EditCraftViewModel model, int masterProfileId, string webRootPath, string userId)
        {
            var craft = await _masterRepository.GetCraftByIdAsync(model.Id) ?? throw new InvalidOperationException();

            craft.Name = model.Name;
            craft.CraftDescription = model.CraftDescription;
            craft.ExperienceYears = model.ExperienceYears;
            craft.CraftTypeId = model.CraftTypeId;

            var offerings = model.Offerings.Select(o => new CraftOffering
            {
                CraftLocation = new CraftLocation { Name = o.LocationName },
                CraftPackage = new CraftPackage { SessionsCount = o.SessionsCount, Label = o.PackageLabel },
                Price = o.Price
            }).ToList();

            var newImages = new List<CraftImage>();
            if (model.Images != null)
            {
                var uploads = Path.Combine(webRootPath, "CraftsImages");
                Directory.CreateDirectory(uploads);
                foreach (var file in model.Images)
                {
                    if (file.Length == 0) continue;
                    var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploads, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                    newImages.Add(new CraftImage { ImageUrl = "/CraftsImages/" + fileName });
                }
            }

            await _masterRepository.UpdateCraftAsync(craft, offerings, model.RemoveImageIds, newImages);
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId)
        {
            return _masterRepository.GetApprenticeshipsAsync(masterProfileId);
        }
    }
}

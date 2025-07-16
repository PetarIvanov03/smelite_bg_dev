using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class MasterProfile
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
                                
        [MaxLength(2000)]
        public string? PersonalInformation { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(150)]
        public string? StripeAccountId { get; set; }

        public ICollection<MasterProfileCraft> MasterProfileCrafts { get; set; }

        public ICollection<Apprenticeship> Tasks { get; set; }
    }}
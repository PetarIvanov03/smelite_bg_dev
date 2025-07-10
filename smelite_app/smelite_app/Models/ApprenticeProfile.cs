using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class ApprenticeProfile
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [MaxLength(2000)]
        public string? PersonalInformation { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Apprenticeship> Apprenticeships { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
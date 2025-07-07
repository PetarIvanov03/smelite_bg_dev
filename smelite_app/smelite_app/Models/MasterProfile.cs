using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class MasterProfile
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [MaxLength(100)]
        public string CraftType { get; set; } // Занаят

        [MaxLength(200)]
        public string AtelierAddress { get; set; }

        [Range(0, 100)]
        public int ExperienceYears { get; set; }

        [MaxLength(2000)]
        public string Bio { get; set; }

        [MaxLength(100)]
        public string TrainingTypes { get; set; } // "Individual", "Group", "Both"

        [MaxLength(50)]
        public string PriceRange { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<MasterProfileImage> GalleryImages { get; set; }
        // Ще добавиш Apprenticeships по-късно ако искаш
    }
}
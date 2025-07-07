using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class ApprenticeProfile
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        [MaxLength(255)]
        public string Preferences { get; set; }

        public bool IsActive { get; set; } = true;

        // Ще добавиш Apprenticeships по-късно ако искаш
    }
}
using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.Master
{
    public class CraftViewModel
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? CraftDescription { get; set; }

        [Range(0, 100)]
        public int ExperienceYears { get; set; }
    }
}

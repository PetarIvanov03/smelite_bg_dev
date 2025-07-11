using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.Master
{
    public class CraftOfferingFormViewModel
    {
        [Required]
        [MaxLength(300)]
        [Display(Name = "Location")]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        [Range(0, 20)]
        [Display(Name = "Sessions")]
        public int SessionsCount { get; set; }

        [MaxLength(100)]
        [Display(Name = "Package Label")]
        public string? PackageLabel { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }
    }
}

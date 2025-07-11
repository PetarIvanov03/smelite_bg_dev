using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [Required]
        [Display(Name = "Craft Type")]
        public int CraftTypeId { get; set; }

        public SelectList? CraftTypes { get; set; }

        public List<CraftOfferingFormViewModel> Offerings { get; set; } = new();
    }
}

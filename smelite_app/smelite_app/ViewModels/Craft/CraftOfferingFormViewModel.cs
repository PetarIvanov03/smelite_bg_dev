using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.Master
{
    public class CraftOfferingFormViewModel
    {
        [Required]
        [Display(Name = "Location")]
        public int CraftLocationId { get; set; }

        [Required]
        [Display(Name = "Package")]
        public int CraftPackageId { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }
    }
}

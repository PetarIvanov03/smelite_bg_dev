using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class Craft
    {
        public int Id { get; set; }

        public int CraftTypeId { get; set; }
        public CraftType CraftType { get; set; }

        [MaxLength(1000)]
        public string? CraftDescription { get; set; }

        [Range(0, 100)]
        public int ExperienceYears { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public ICollection<CraftImage> Images { get; set; }
        public ICollection<MasterProfileCraft> MasterProfileCrafts { get; set; }
        public ICollection<CraftLocation> CraftLocations { get; set; }
        public ICollection<CraftPackage> CraftPackages { get; set; }
        public ICollection<CraftOffering> CraftOfferings { get; set; }
    }

}

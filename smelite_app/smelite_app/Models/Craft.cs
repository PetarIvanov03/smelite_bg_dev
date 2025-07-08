using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Location { get; set; }

        public int TrainingTypeId { get; set; }
        public TrainingType TrainingType { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public decimal Price { get; set; }

        public ICollection<CraftImage> Images { get; set; }
        public ICollection<MasterProfileCraft> MasterProfileCrafts { get; set; }
    }

}

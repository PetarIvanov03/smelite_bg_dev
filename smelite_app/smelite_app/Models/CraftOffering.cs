using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smelite_app.Models
{
    public class CraftOffering
    {
        public int Id { get; set; }

        [Required]
        public int CraftId { get; set; }
        public Craft Craft { get; set; }

        [Required]
        public int CraftLocationId { get; set; }
        public CraftLocation CraftLocation { get; set; }

        [Required]
        public int CraftPackageId { get; set; }
        public CraftPackage CraftPackage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}

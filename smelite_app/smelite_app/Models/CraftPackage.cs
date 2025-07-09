using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class CraftPackage
    {
        public int Id { get; set; }

        [Required]
        public int SessionsCount { get; set; }

        [MaxLength(100)]
        public string? Label { get; set; }

        public ICollection<CraftOffering> CraftOfferings { get; set; }
    }
}

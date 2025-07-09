using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class CraftLocation
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public ICollection<CraftOffering> CraftOfferings { get; set; }
    }
}

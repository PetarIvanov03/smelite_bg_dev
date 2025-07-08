using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class CraftImage
    {
        public int Id { get; set; }

        [Required]
        public int CraftId { get; set; }
        public Craft Craft { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }
    }
}
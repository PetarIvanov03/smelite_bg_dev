using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class CraftType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Craft> Crafts { get; set; }
    }

}

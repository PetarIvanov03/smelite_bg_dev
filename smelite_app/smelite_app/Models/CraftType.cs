using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class CraftType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Craft> Crafts { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class TrainingType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Craft> Crafts { get; set; }
    }

}

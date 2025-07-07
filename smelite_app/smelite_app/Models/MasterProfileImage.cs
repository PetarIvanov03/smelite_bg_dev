using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class MasterProfileImage
    {
        public int Id { get; set; }

        [Required]
        public int MasterProfileId { get; set; }
        public MasterProfile MasterProfile { get; set; }

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}
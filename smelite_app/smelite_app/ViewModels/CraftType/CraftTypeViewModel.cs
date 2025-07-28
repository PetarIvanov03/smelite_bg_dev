using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.CraftType
{
    public class CraftTypeViewModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}

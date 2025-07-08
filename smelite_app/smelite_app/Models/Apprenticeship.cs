using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class Apprenticeship
    {
        public int Id { get; set; }

        [Required]
        public int ApprenticeProfileId { get; set; }
        public ApprenticeProfile ApprenticeProfile { get; set; }

        [Required]
        public int MasterProfileId { get; set; }
        public MasterProfile MasterProfile { get; set; }

        [Required]
        public int CraftId { get; set; }
        public Craft Craft { get; set; }

        [MaxLength(2000)]
        public string? SelectedProps { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}

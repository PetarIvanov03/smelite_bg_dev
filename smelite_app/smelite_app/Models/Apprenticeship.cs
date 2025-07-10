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
        public int CraftOfferingId { get; set; }
        public CraftOffering CraftOffering { get; set; }

        [Required]
        public string Status { get; set; }
    }
}

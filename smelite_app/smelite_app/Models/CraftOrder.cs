using System;
using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class CraftOrder
    {
        public int Id { get; set; }

        [Required]
        public int ApprenticeProfileId { get; set; }
        public ApprenticeProfile ApprenticeProfile { get; set; }

        [Required]
        public int CraftOfferingId { get; set; }
        public CraftOffering CraftOffering { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [MaxLength(2000)]
        public string? AdditionalInfo { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class Payment
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int ApprenticeshipId { get; set; }
        public Apprenticeship Apprenticeship { get; set; }

        [Required]
        public string PayerUserId { get; set; }
        public ApprenticeProfile PayerUser { get; set; }

        [Required]
        public string RecipientUserId { get; set; }
        public MasterProfile RecipientUser { get; set; }

        [Required]
        public decimal AmountTotal { get; set; }

        [Required]
        public decimal PlatformFee { get; set; }

        [Required]
        public decimal AmountToRecipient { get; set; }

        [Required]
        public DateTime PaidOn { get; set; }

        [Required]
        public string Method { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string TransactionId { get; set; }
    }
}

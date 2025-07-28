using System;
using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class EmailSubscription
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    }
}

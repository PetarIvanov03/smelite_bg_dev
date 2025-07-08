using Microsoft.AspNetCore.Identity;
using smelite_app.Models;
using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        public MasterProfile MasterProfile { get; set; }
        public ApprenticeProfile ApprenticeProfile { get; set; }
    }
}
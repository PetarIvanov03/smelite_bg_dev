using Microsoft.AspNetCore.Identity;
using smelite_app.Models;
using System.ComponentModel.DataAnnotations;

namespace smelite_app.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(255)]
        public string ProfileImageUrl { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        public MasterProfile MasterProfile { get; set; }
        public ApprenticeProfile ApprenticeProfile { get; set; }
    }
}
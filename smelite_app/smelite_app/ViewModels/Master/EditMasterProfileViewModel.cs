using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace smelite_app.ViewModels.Master
{
    public class EditMasterProfileViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? ProfileImageUrl { get; set; }

        public IFormFile? ProfileImage { get; set; }

        [MaxLength(2000)]
        public string? PersonalInformation { get; set; }

        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.Admin
{
    public class AdminProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }
}

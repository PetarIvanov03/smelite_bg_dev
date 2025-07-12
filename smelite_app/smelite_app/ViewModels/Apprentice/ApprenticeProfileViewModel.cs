namespace smelite_app.ViewModels.Apprentice
{
    public class ApprenticeProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? PersonalInformation { get; set; }
    }
}

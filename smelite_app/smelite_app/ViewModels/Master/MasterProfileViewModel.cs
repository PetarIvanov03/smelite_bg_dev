using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.Master
{
    public class MasterProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? PersonalInformation { get; set; }

        /// <summary>
        /// Indicates whether the master has a valid connected Stripe account.
        /// </summary>
        public bool StripeConnected { get; set; }
    }
}

namespace smelite_app.Helpers
{
    public static class EmailMessages
    {
        public static string SubscriptionConfirmSubject = "Успешен абонамент";
        public static string SubscriptionConfirmBody = "Благодарим, че се абонирахте за нашия бюлетин.";

        public static string PaymentUpdatedSubject = "Payment updated";
        public static string PaymentUpdatedBody = "Payment {0} status changed to {1}.";

        public static string ApprenticeshipRequestedSubject = "Apprenticeship requested";
        public static string ApprenticeshipRequestedBody = "Apprentice {0} requested offering {1}.";

        public static string CraftCreatedSubject = "Craft created";
        public static string CraftCreatedBody = "Master {0} created craft '{1}'.";

        public static string NewAccountCreatedSubject = "New account created";
        public static string NewAccountCreatedBody = "User {0} registered as {1}.";

        public static string AccountVerifiedSubject = "Account verified";
        public static string AccountVerifiedBody = "User {0} has verified their account.";

        public static string ConfirmEmailSubject = "Confirm your email";
        public static string ConfirmEmailBody = "Please confirm your account by <a href='{0}'>clicking here</a>.";

        public static string BlogPostBody = "{0}<br/><a href='{1}'>Прочети повече</a>";
    }
}

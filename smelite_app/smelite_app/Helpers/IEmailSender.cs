namespace smelite_app.Helpers
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}

using System.Net.Mail;
using System.Net;

namespace smelite_app.Helpers
{
    public class EmailSender
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "smelitebg@gmail.com";
        private readonly string _smtpPass = "qprj rddn chmc bero";

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage(_smtpUser, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMTP error: {ex.Message}");
                return false;
            }
        }
    }
}

using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Bamboo.API
{
    public class EmailApi
    {
        public static async Task<bool> SendEmail(string recipient, string subject, string body)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("bambooprojectsystem@gmail.com", "Bamboo");
                var to = new EmailAddress(recipient, "User");
                var plainTextContent = body;
                var htmlContent = "<strong>fdasf</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return true;

            }
            catch { return false; }
        }
    }
}

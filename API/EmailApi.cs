using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using System.IO;
using System.Threading;

public class EmailApi
{
    static string[] Scopes = { GmailService.Scope.GmailSend };
    static string ApplicationName = "Bamboo";
    private Google.Apis.Gmail.v1.GmailService _service;

    public EmailApi()
    {
        UserCredential credential;

        using (var stream = new FileStream("credentialsGmail.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        // Create Gmail API service.
        _service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public static string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        // Special "url-safe" base64 encode.
        return Convert.ToBase64String(inputBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }


    public bool sendEmailAsync(string recipient, string subject, string content)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse("bambooproject23@gmail.com");
        email.To.Add(MailboxAddress.Parse(recipient));
        email.Subject = subject;
        email.Body = new TextPart("plain") { Text = content };

        var message = new Google.Apis.Gmail.v1.Data.Message();
        message.Raw = Base64UrlEncode(email.ToString());

        var result = _service.Users.Messages.Send(message, "me").Execute();
        return true;
    }
}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _senderEmail;
    private readonly string _password;

    public EmailService(string smtpServer, int port, string senderEmail, string password)
    {
        _smtpServer = smtpServer;
        _port = port;
        _senderEmail = senderEmail;
        _password = password;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, string attachmentPath = null)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Fitness GYM", _senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = body
        };

        if (!string.IsNullOrEmpty(attachmentPath))
        {
            builder.Attachments.Add(attachmentPath);
        }

        message.Body = builder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_smtpServer, _port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_senderEmail, _password);
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}

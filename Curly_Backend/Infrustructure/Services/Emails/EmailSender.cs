using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Infrustructure.Services.Emails;

public class EmailSender
{
    private const string SMTP_SERVER = "smtp.gmail.com";
    private const string SENDER = "curlynoreply@gmail.com";

    public async Task SendHtmlEmail(string to, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("20werasdf@gmail.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = "Password reset";
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        await smtp.ConnectAsync(SMTP_SERVER, 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(SENDER, "ecgu qmuu kmwm ajjk");
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
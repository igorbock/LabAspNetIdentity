namespace ShieldJWT.Services;

public class MailService : IShieldMail
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendConfirmCodeTo(string email, string name, string code, string subject)
    {
        using var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Shield JWT", _configuration["NoReply:Address"]));
        message.To.Add(new MailboxAddress(name, email));

        var basePath = Directory.GetCurrentDirectory();
        var content = $"Código de confirmação: {code}";
        var bodyBuilder = new BodyBuilder();

        message.Subject = subject;

        bodyBuilder.HtmlBody = content;
        message.Body = bodyBuilder.ToMessageBody();

        using SmtpClient client = new SmtpClient();
        client.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

        client.Authenticate(_configuration["NoReply:Address"], _configuration["NoReply:Password"]);

        client.Send(message);
        client.Disconnect(true);
    }
}

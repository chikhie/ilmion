namespace Ilmanar.Infra.Mail;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Ilmanar.Api.Interfaces;

public class SmtpMailService : IMailService

{
    private readonly SmtpClient _client;
    private readonly string _from;

    public SmtpMailService(IConfiguration config)
    {
        _from = config["Smtp:From"] ?? "noreply@myapp.com";

        _client = new SmtpClient(config["Smtp:Host"])
        {
            Port = int.Parse(config["Smtp:Port"] ?? "587"),
            Credentials = new NetworkCredential(
                config["Smtp:User"], 
                config["Smtp:Pass"]
            ),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        Console.WriteLine("Sending email old to {0}", to);
        var mail = new MailMessage
        {
            From = new MailAddress(_from),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };

        mail.To.Add(to);

        await _client.SendMailAsync(mail);
    }
}


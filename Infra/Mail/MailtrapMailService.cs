namespace Ilmanar.Infra.Mail;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Ilmanar.Api.Interfaces;

public class MailtrapMailService : IMailService
{
    private readonly HttpClient _httpClient;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public MailtrapMailService(IConfiguration config)
    {
        var apiToken = config["Mailtrap:ApiToken"] 
            ?? throw new ArgumentNullException("Mailtrap:ApiToken configuration is missing");
        _fromEmail = config["Mailtrap:FromEmail"] ?? "hello@ilmanar.site";
        _fromName = config["Mailtrap:FromName"] ?? "Ilmanar";

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://send.api.mailtrap.io/")
        };
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", apiToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        Console.WriteLine("Sending email to {0}", to);
        try
        {
            var emailData = new
            {
                from = new { email = _fromEmail, name = _fromName },
                to = new[] { new { email = to } },
                subject = subject,
                text = isHtml ? null : body,
                html = isHtml ? body : null,
                category = "Application Email"
            };

            var jsonContent = JsonSerializer.Serialize(emailData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/send", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send email. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while sending email: {0}", ex);
            throw;
        }
    }
}

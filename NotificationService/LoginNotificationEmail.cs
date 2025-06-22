using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationService;

public class LoginNotificationEmail
{
    private readonly ILogger<LoginNotificationEmail> _logger;

    public LoginNotificationEmail(ILogger<LoginNotificationEmail> logger)
    {
        _logger = logger;
    }

    [Function(nameof(LoginNotificationEmail))]
    public async Task Run([KafkaTrigger(
            "kafka:9092",
            "after-login-email-topic",
            ConsumerGroup = "function-consumer-group")] KafkaMessage message)
    {
        _logger.LogInformation($"Odebrano wiadomość z Kafki: {message.ToString()}");
        await SendEmailAsync("Zostałeś pomyślnie zalogowany", message.Value);

    }

    static async Task SendEmailAsync(string Message, string toEmail)
    {
        try
        {
            string? smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
            int smtpPort = 587;
            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                string? smtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME");
                string? smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = "Wiadomość z Kafki",
                    Body = Message,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"E-mail wysłany do {toEmail} z wiadomością: {Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas wysyłania e-maila: {ex.Message}");
        }
    }
}

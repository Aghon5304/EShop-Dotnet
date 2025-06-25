using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationService
{
    public class ShoppingCartProcess
    {
        private readonly ILogger<ShoppingCartProcess> _logger;
        public ShoppingCartProcess(ILogger<ShoppingCartProcess> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ShoppingCartProcess))]
        public async Task Run([KafkaTrigger(
            "kafka:9092",
            "after-cart-process-topic",
            ConsumerGroup = "function-consumer-group")] KafkaMessage message)
        {
            _logger.LogInformation($"Odebrano wiadomo�� z Kafki: {message.ToString()}");
            var deserialized = JsonDocument.Parse(message.Value).RootElement;
            Console.WriteLine($"message: {deserialized.GetProperty("Message").ToString()}\nEmail to:{deserialized.GetProperty("Email").ToString()}");
            await SendEmailAsync(deserialized.GetProperty("Message").ToString(), deserialized.GetProperty("Email".ToString()).ToString());
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
                        Subject = "Wiadomo�� z Kafki",
                        Body = Message,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine($"E-mail wys�any do {toEmail} z wiadomo�ci�: {Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B��d podczas wysy�ania e-maila: {ex.Message}");
            }
        }

    }
}

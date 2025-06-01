using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationService
{
    public class LoginNotificationEmail
    {
        private readonly ILogger<LoginNotificationEmail> _logger;

        public LoginNotificationEmail(ILogger<LoginNotificationEmail> logger)
        {
            _logger = logger;
        }

        [Function(nameof(LoginNotificationEmail))]
        public void Run([QueueTrigger("after-login-email-topic", Connection = "")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}

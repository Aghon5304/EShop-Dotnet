using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationService
{
    public class TestFunc
    {
        private readonly ILogger _logger;

        public TestFunc(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TestFunc>();
        }

        [Function("TestFunc")]
        public void Run([RabbitMQTrigger("after-login-email-topic", ConnectionStringSetting = "")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}

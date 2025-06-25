using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using ShoppingCart.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Producer;

public class KafkaProducer : IKafkaProducer
{
	private readonly IProducer<Null, string> _producer;
	private readonly ILogger<KafkaProducer> _logger;

	public KafkaProducer(ILogger<KafkaProducer> logger)
	{
		var config = new ProducerConfig
		{
			BootstrapServers = "kafka:9092"
		};

		_producer = new ProducerBuilder<Null, string>(config).Build();
		_logger = logger;
	}

	public async Task SendMessageAsync(string topic, ShoppingCartSendKafkaDTO message)
	{
		var serialized = System.Text.Json.JsonSerializer.Serialize(message);
        var result = await _producer.ProduceAsync(topic, new Message<Null, string> {Value = serialized });
		_logger.LogInformation($"Wysłano wiadomość: {message.Message} do partycji {result.Partition} z offsetem {result.Offset}");
	}
}

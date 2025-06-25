using Microsoft.Extensions.Logging;
using ShoppingCart.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Producer;

public interface IKafkaProducer
{
    Task SendMessageAsync(string topic, ShoppingCartSendKafkaDTO message);
}
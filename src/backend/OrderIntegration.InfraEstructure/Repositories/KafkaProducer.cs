using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using OrderIntegration.Domain.Entities;
using System.Text.Json;

namespace OrderIntegration.InfraEstructure.Repositories
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducer(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = configuration["Kafka:Topic"] ?? "orders-topic";
        }

        public async Task ProduceAsync(Order order)
        {
            var json = JsonSerializer.Serialize(order);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
        }
    }
}

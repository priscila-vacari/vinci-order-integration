using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using OrderIntegration.Domain.Entities;
using OrderIntegration.InfraEstructure.Interfaces;
using System.Text.Json;

namespace OrderIntegration.InfraEstructure.Repositories
{
    /// <summary>
    /// Produtor Kafka para pedidos
    /// </summary>
    public class KafkaProducer: IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        /// <summary>
        /// Construtor principal (produção)
        /// </summary>
        public KafkaProducer(IConfiguration configuration)
            : this(configuration, new ProducerBuilder<Null, string>(
                new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] }
            ).Build())
        {
        }

        /// <summary>
        /// Construtor alternativo para testes
        /// </summary>
        public KafkaProducer(IConfiguration configuration, IProducer<Null, string> producer)
        {
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
            _topic = configuration["Kafka:Topic"] ?? "orders-topic";
        }

        /// <summary>
        /// Envia o pedido para o tópico Kafka
        /// </summary>
        public async Task ProduceAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var json = JsonSerializer.Serialize(order);
            var message = new Message<Null, string> { Value = json };

            await _producer.ProduceAsync(_topic, message);
        }
    }
}

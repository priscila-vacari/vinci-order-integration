using Moq;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using OrderIntegration.Domain.Entities;
using OrderIntegration.InfraEstructure.Repositories;
using System.Text.Json;

namespace OrderIntegration.Tests.InfraEstructure
{
    public class KafkaProducerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IProducer<Null, string>> _producerMock;

        public KafkaProducerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Kafka:Topic"]).Returns("test-topic");

            _producerMock = new Mock<IProducer<Null, string>>();
        }

        [Fact]
        public async Task ProduceAsync_ShouldSendMessageToKafka()
        {
            // Arrange
            var order = new Order { Id = 1, Cliente = "Cliente Teste", DataPedido = DateTime.Today, Valor = 99.99M };
            var expectedMessage = new Message<Null, string> { Value = JsonSerializer.Serialize(order) };

            _producerMock
                .Setup(p => p.ProduceAsync(
                    "test-topic",
                    It.Is<Message<Null, string>>(m => m.Value == expectedMessage.Value),
                    default
                ))
                .ReturnsAsync(new DeliveryResult<Null, string>());

            var producer = new KafkaProducer(_configurationMock.Object, _producerMock.Object);

            // Act
            await producer.ProduceAsync(order);

            // Assert
            _producerMock.Verify(p => p.ProduceAsync("test-topic",
                It.Is<Message<Null, string>>(m => m.Value == expectedMessage.Value),
                default), Times.Once);
        }

        [Fact]
        public async Task ProduceAsync_WithNullOrder_ShouldThrowArgumentNullException()
        {
            // Arrange
            var producer = new KafkaProducer(_configurationMock.Object, _producerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => producer.ProduceAsync(null));
        }
    }
}

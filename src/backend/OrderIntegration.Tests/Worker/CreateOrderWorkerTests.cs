using Moq;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OrderIntegration.Application.Interfaces;
using OrderIntegration.Domain.Entities;
using OrderIntegration.Worker.Services;
using System.Text.Json;

namespace OrderIntegration.Worker.Tests
{
    public class CreateOrderWorkerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<ILogger<CreateOrderWorker>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConsumer<Ignore, string>> _mockConsumer;
        private readonly CreateOrderWorker _worker;

        public CreateOrderWorkerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger<CreateOrderWorker>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConsumer = new Mock<IConsumer<Ignore, string>>();

            _mockConfiguration.Setup(c => c["Kafka:Topic"]).Returns("orders-topic");
            _mockConfiguration.Setup(c => c["Kafka:BootstrapServers"]).Returns("localhost:9092");

            _worker = new CreateOrderWorker(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockOrderService.Object,
                _mockConsumer.Object);
        }

        private TestableCreateOrderWorker CreateWorker()
        {
            _mockConfiguration.Setup(c => c["Kafka:Topic"]).Returns("orders-topic");
            return new TestableCreateOrderWorker(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockOrderService.Object,
                _mockConsumer.Object
            );
        }

        // Classe derivada para expor o método protegido ExecuteAsync
        private class TestableCreateOrderWorker : CreateOrderWorker
        {
            public TestableCreateOrderWorker(
                ILogger<CreateOrderWorker> logger,
                IConfiguration configuration,
                IOrderService orderService,
                IConsumer<Ignore, string> consumer
            ) : base(logger, configuration, orderService, consumer) { }

            public Task TestExecuteAsync(CancellationToken stoppingToken) => base.ExecuteAsync(stoppingToken);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleCancellationGracefully()
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(100));

            await Task.Delay(200);
            await _worker.StartAsync(cts.Token);

            _mockOrderService.Verify(service => service.AddAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldConsumeMessage_AndCallOrderService()
        {
            var id = 1;
            var order = new Order { Id = id, Cliente = "Cliente A", Valor = 100, DataPedido = DateTime.Today };
            var json = JsonSerializer.Serialize(order);

            var result = new ConsumeResult<Ignore, string>
            {
                Message = new Message<Ignore, string> { Value = json }
            };

            _mockConsumer
                .Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                .Returns(result);

            var worker = CreateWorker();
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5)); 

            await worker.TestExecuteAsync(cts.Token);

            _mockOrderService.Verify(s => s.AddAsync(It.IsAny<Order>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleInvalidOrderGracefully()
        {
            var result = new ConsumeResult<Ignore, string>
            {
                Message = new Message<Ignore, string> { Value = "invalid-json" }
            };

            _mockConsumer
                .Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                .Returns(result);

            var worker = CreateWorker();
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            await worker.TestExecuteAsync(cts.Token);

            _mockOrderService.Verify(s => s.AddAsync(It.IsAny<Order>()), Times.Never);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            var worker = CreateWorker();

            worker.Dispose();

            _mockConsumer.Verify(c => c.Close(), Times.Once);
        }
    }
}

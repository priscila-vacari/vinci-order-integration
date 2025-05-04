using Confluent.Kafka;
using OrderIntegration.Application.Interfaces;
using OrderIntegration.Domain.Entities;
using OrderIntegration.Domain.Exceptions;
using System.Text.Json;

namespace OrderIntegration.Worker.Services
{
    public class CreateOrderWorker : BackgroundService
    {
        private readonly ILogger<CreateOrderWorker> _logger;
        private readonly IOrderService _orderService;
        private readonly string _topic;
        private readonly IConsumer<Ignore, string> _consumer;

        public CreateOrderWorker(ILogger<CreateOrderWorker> logger, IConfiguration configuration, IOrderService orderService, IConsumer<Ignore, string> consumer)
        {
            _logger = logger;
            _orderService = orderService;
            _consumer = consumer;
            _topic = configuration["Kafka:Topic"] ?? "orders-topic";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                var correlationId = Guid.NewGuid().ToString();

                using (_logger.BeginScope(new { CorrelationId = correlationId }))
                {
                    try
                    {
                        _logger.LogInformation("OrderWorkerService running at: {time} [CorrelationId: {correlationId}]", DateTime.Now, correlationId);

                        var result = _consumer.Consume(stoppingToken);
                        var order = JsonSerializer.Deserialize<Order>(result.Message.Value) ?? throw new OrderException("Pedido inválido.");

                        await _orderService.AddAsync(order);

                        _logger.LogInformation("OrderWorkerService process completed at {time} [CorrelationId: {correlationId}]", DateTime.Now, correlationId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during consolidation: {message} [CorrelationId: {correlationId}]", ex.Message, correlationId);
                    }
                }
            }
        }

        public override void Dispose() => _consumer.Close();
    }
}

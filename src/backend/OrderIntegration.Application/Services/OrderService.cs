using AutoMapper;
using OrderIntegration.Application.DTOs;
using OrderIntegration.Application.Interfaces;
using OrderIntegration.Domain.Entities;
using OrderIntegration.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using OrderIntegration.InfraEstructure.Repositories;
using OrderIntegration.InfraEstructure.Interfaces;

namespace OrderIntegration.Application.Services
{
    public class OrderService(ILogger<OrderService> logger, IMapper mapper, IRepository<Order> repository, KafkaProducer producer, OrderCacheRepository cache) : IOrderService
    {
        private readonly ILogger<OrderService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<Order> _repository = repository;
        private readonly KafkaProducer _producer = producer;
        private readonly OrderCacheRepository _cache = cache;

        public async Task SendToQueueAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _producer.ProduceAsync(order);
        }

        public async Task<OrderDTO> GetOrderAsync(int id)
        {
            _logger.LogInformation("Obtendo dados do pedido {id}.", id);

            var orderCachedDto = await GetOrderCache(id);
            if (orderCachedDto != null)
                return orderCachedDto;

            var order = await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Pedido não encontrado.");

            await AddOrderCache(order);

            var orderDto = _mapper.Map<OrderDTO>(order);
            return orderDto;
        }

        public async Task AddAsync(Order order)
        {
            _logger.LogInformation("Criando novo pedido: {Id}", order.Id);

            var orderExists = await _repository.GetByKeysAsync(order.Id);
            if (orderExists != null)
                throw new DuplicateEntryException("Pedido já cadastrado.");
            await _repository.AddAsync(order);
        }

        private async Task<OrderDTO?> GetOrderCache(int id)
        {
            var orderCached = await _cache.GetAsync(id);
            if (orderCached == null)
                return null;

            var orderCachedDto = _mapper.Map<OrderDTO>(orderCached);
            return orderCachedDto;
        }

        private async Task AddOrderCache(Order order)
        {
            if (order == null)
                return;

            await _cache.AddAsync(order);
        }
    }
}

using AutoMapper;
using OrderIntegration.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using OrderIntegration.Application.DTOs;
using OrderIntegration.Application.Services;
using OrderIntegration.Domain.Entities;
using OrderIntegration.InfraEstructure.Interfaces;
using OrderIntegration.InfraEstructure.Repositories;
using OrderIntegration.Application.Interfaces;

namespace OrderIntegration.Tests.Application.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<ILogger<OrderService>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IRepository<Order>> _repositoryMock = new();
        private readonly Mock<IKafkaProducer> _producerMock = new();
        private readonly Mock<IOrderCacheRepository> _cacheMock = new();
        private readonly IOrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(
                    _loggerMock.Object,
                    _mapperMock.Object,
                    _repositoryMock.Object,
                    _producerMock.Object,
                    _cacheMock.Object);
        }

        [Fact]
        public async Task SendToQueueAsync_ShouldCallKafkaProducer_WithMappedOrder()
        {
            var orderDto = new OrderDTO { Id = 1 };
            var order = new Order { Id = 1 };

            _mapperMock.Setup(m => m.Map<Order>(orderDto)).Returns(order);

            await _service.SendToQueueAsync(orderDto);

            _producerMock.Verify(p => p.ProduceAsync(order), Times.Once);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldReturnFromCache_WhenOrderExistsInCache()
        {
            var orderId = 1;
            var order = new Order { Id = orderId };
            var orderDto = new OrderDTO { Id = orderId };

            _cacheMock.Setup(c => c.GetAsync(orderId)).ReturnsAsync(order);
            _mapperMock.Setup(m => m.Map<OrderDTO>(order)).Returns(orderDto);

            var result = await _service.GetOrderAsync(orderId);

            Assert.Equal(orderId, result.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldReturnFromRepository_WhenNotInCache()
        {
            var orderId = 2;
            var order = new Order { Id = orderId };
            var orderDto = new OrderDTO { Id = orderId };

            _cacheMock.Setup(c => c.GetAsync(orderId)).ReturnsAsync((Order)null);
            _repositoryMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mapperMock.Setup(m => m.Map<OrderDTO>(order)).Returns(orderDto);

            var result = await _service.GetOrderAsync(orderId);

            Assert.Equal(orderId, result.Id);
            _cacheMock.Verify(c => c.AddAsync(order), Times.Once);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldThrowNotFoundException_WhenOrderNotFound()
        {
            _cacheMock.Setup(c => c.GetAsync(It.IsAny<int>())).ReturnsAsync((Order)null);
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetOrderAsync(99));
        }

        [Fact]
        public async Task AddAsync_ShouldThrowDuplicateEntryException_WhenOrderAlreadyExists()
        {
            var order = new Order { Id = 1 };
            _repositoryMock.Setup(r => r.GetByKeysAsync(order.Id)).ReturnsAsync(order);

            await Assert.ThrowsAsync<DuplicateEntryException>(() => _service.AddAsync(order));
        }
    }
}
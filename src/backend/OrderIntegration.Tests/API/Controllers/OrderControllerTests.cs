using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using OrderIntegration.API.Controllers.v1;
using OrderIntegration.Application.Interfaces;
using OrderIntegration.Application.DTOs;
using OrderIntegration.API.Models;

namespace OrderIntegration.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<ILogger<OrderController>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOrderService> _orderServiceMock = new();
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _controller = new OrderController(_loggerMock.Object, _mapperMock.Object, _orderServiceMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsOrder_WhenOrderExists()
        {
            var id = 1;
            var orderDto = new OrderDTO { Id = id, Cliente = "Cliente A", Valor = 100, DataPedido = DateTime.Today };
            var orderResponse = new OrderResponseModel { Id = orderDto.Id, Cliente = orderDto.Cliente, Valor = orderDto.Valor, DataPedido = orderDto.DataPedido };

            _orderServiceMock.Setup(s => s.GetOrderAsync(id)).ReturnsAsync(orderDto);
            _mapperMock.Setup(m => m.Map<OrderResponseModel>(orderDto)).Returns(orderResponse);

            var result = await _controller.Get(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrder = Assert.IsType<OrderResponseModel>(okResult.Value);
            Assert.Equal(orderResponse.Id, returnedOrder.Id);
        }

        [Fact]
        public async Task AddOrder_ReturnsAccepted_WhenOrderIsValid()
        {
            var id = 1;
            var request = new OrderRequestModel { Id = id, Cliente = "Cliente A", Valor = 100, DataPedido = DateTime.Today };
            var orderDto = new OrderDTO { Id = request.Id, Cliente = request.Cliente, Valor = request.Valor, DataPedido = request.DataPedido };

            _mapperMock.Setup(m => m.Map<OrderDTO>(request)).Returns(orderDto);
            _orderServiceMock.Setup(s => s.SendToQueueAsync(orderDto)).Returns(Task.CompletedTask);

            var result = await _controller.AddOrder(request);

            Assert.IsType<AcceptedResult>(result);
        }
    }
}

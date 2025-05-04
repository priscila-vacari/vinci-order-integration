using Asp.Versioning;
using AutoMapper;
using OrderIntegration.API.Models;
using OrderIntegration.Application.DTOs;
using OrderIntegration.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderIntegration.API.Controllers.v1
{
    /// <summary>
    /// Controller responsável pelos pedidos
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    /// <param name="orderService"></param>
    [ApiVersion("1.0")]
    public class OrderController(ILogger<OrderController> logger, IMapper mapper, IOrderService orderService) : BaseController(logger, mapper)
    {
        private readonly IOrderService _orderService = orderService;

        /// <summary>
        /// Busca o pedido de um respectivo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna dados do respectivo pedido</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obter dados do pedido {id}", id);

            var order = await _orderService.GetOrderAsync(id);

            var orderResponse = _mapper.Map<OrderResponseModel>(order);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Adiciona um novo pedido
        /// </summary>
        /// <param name="orderModel"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequestModel orderModel)
        {
            _logger.LogInformation("Enfileirar novo pedido");

            var orderDto = _mapper.Map<OrderDTO>(orderModel);
            await _orderService.SendToQueueAsync(orderDto);

            return Accepted();
        }
    }
}

using OrderIntegration.Application.DTOs;
using OrderIntegration.Domain.Entities;

namespace OrderIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task SendToQueueAsync(OrderDTO orderDto);
        Task<OrderDTO> GetOrderAsync(int id);
        Task AddAsync(Order order);
    }
}

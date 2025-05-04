using OrderIntegration.Domain.Entities;

namespace OrderIntegration.InfraEstructure.Interfaces
{
    public interface IOrderCacheRepository
    {
        Task<Order?> GetAsync(int id);
        Task AddAsync(Order order);
    }
}

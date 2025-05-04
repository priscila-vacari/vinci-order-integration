using OrderIntegration.Domain.Entities;

namespace OrderIntegration.InfraEstructure.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceAsync(Order order);
    }
}

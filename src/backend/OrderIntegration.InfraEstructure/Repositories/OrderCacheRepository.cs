using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderIntegration.Domain.Entities;
using OrderIntegration.InfraEstructure.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.InfraEstructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OrderCacheRepository: IOrderCacheRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public OrderCacheRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.Database);
            _collection = database.GetCollection<Order>(settings.Value.Collection);
        }

        public async Task<Order?> GetAsync(int id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _collection.InsertOneAsync(order);
        }
    }
}

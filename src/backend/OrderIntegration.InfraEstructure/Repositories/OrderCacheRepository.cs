using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderIntegration.Domain.Entities;

namespace OrderIntegration.InfraEstructure.Repositories
{
    public class OrderCacheRepository
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

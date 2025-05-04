using Moq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderIntegration.Domain.Entities;
using OrderIntegration.InfraEstructure.Repositories;
using OrderIntegration.InfraEstructure;


namespace OrderIntegration.Tests.Infra
{
    public class OrderCacheRepositoryTests
    {
        private readonly Mock<IMongoCollection<Order>> _mockCollection;
        private readonly Mock<IOptions<MongoDbSettings>> _mockSettings;
        private readonly OrderCacheRepository _repository;

        public OrderCacheRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Order>>();

            _mockSettings = new Mock<IOptions<MongoDbSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                Database = "TestDb",
                Collection = "orders"
            });

            _repository = new OrderCacheRepository(_mockSettings.Object);
        }

        [Fact(Skip = "Corrigir")]
        public async Task GetAsync_ReturnsOrder_WhenExists()
        {
            var order = new Order { Id = 1 };
            var mockFindFluent = new Mock<IFindFluent<Order, Order>>();

            mockFindFluent.Setup(f => f.FirstOrDefaultAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(order);

            _mockCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<Order>>(), It.IsAny<FindOptions>()))
                .Returns(mockFindFluent.Object);

            var result = await _repository.GetAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }


        [Fact(Skip = "Corrigir")]
        public async Task GetAsync_ReturnsNull_WhenNotFound()
        {
            var mockFindFluent = new Mock<IFindFluent<Order, Order>>();

            mockFindFluent.Setup(f => f.FirstOrDefaultAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync((Order)null);

            _mockCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<Order>>(), It.IsAny<FindOptions>()))
                .Returns(mockFindFluent.Object);

            var result = await _repository.GetAsync(999); // Id que não existe

            Assert.Null(result);
        }


        [Fact(Skip = "Corrigir")]
        public async Task AddAsync_InsertsOrderSuccessfully()
        {
            var order = new Order { Id = 1 };

            _mockCollection.Setup(c => c.InsertOneAsync(It.IsAny<Order>(), It.IsAny<InsertOneOptions>(), It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _repository.AddAsync(order);

            _mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<Order>(), It.IsAny<InsertOneOptions>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }
    }
}

using Moq;
using OrderIntegration.InfraEstructure.Context;
using OrderIntegration.InfraEstructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OrderIntegration.Domain.Entities;

namespace OrderIntegration.Tests.Infra
{
    public class RepositoryTests
    {
        private readonly Mock<ILogger<Repository<Order>>> _mockLogger;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<DbSet<Order>> _mockDbSet;

        public RepositoryTests()
        {
            _mockLogger = new Mock<ILogger<Repository<Order>>>();
            _mockContext = new Mock<AppDbContext>();
            _mockDbSet = new Mock<DbSet<Order>>();
        }

        [Fact(Skip = "Corrigir")]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            var data = new List<Order> { new Order(), new Order() };
            var queryable = data.AsQueryable();

            _mockDbSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockDbSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockDbSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockDbSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _mockContext.Setup(c => c.Set<Order>()).Returns(_mockDbSet.Object);

            var repository = new Repository<Order>(_mockContext.Object, _mockLogger.Object);

            var result = await repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(Skip = "Corrigir")]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            var order = new Order { Id = 1 };
            _mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(order);
            _mockContext.Setup(c => c.Set<Order>()).Returns(_mockDbSet.Object);

            var repository = new Repository<Order>(_mockContext.Object, _mockLogger.Object);

            var result = await repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact(Skip = "Corrigir")]
        public async Task GetByIdAsync_ReturnsNullWhenNotFound()
        {
            _mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Order)null);
            _mockContext.Setup(c => c.Set<Order>()).Returns(_mockDbSet.Object);

            var repository = new Repository<Order>(_mockContext.Object, _mockLogger.Object);

            var result = await repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact(Skip = "Corrigir")]
        public async Task AddAsync_AddsEntitySuccessfully()
        {
            var order = new Order { Id = 1 };
            _mockDbSet.Setup(m => m.AddAsync(It.IsAny<Order>(), default)).ReturnsAsync((EntityEntry<Order>)null);
            _mockContext.Setup(c => c.Set<Order>()).Returns(_mockDbSet.Object);

            var repository = new Repository<Order>(_mockContext.Object, _mockLogger.Object);

            await repository.AddAsync(order);

            _mockDbSet.Verify(m => m.AddAsync(It.IsAny<Order>(), default), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact(Skip = "Corrigir")]
        public async Task DeleteAsync_DeletesEntitySuccessfully()
        {
            var order = new Order { Id = 1 };
            _mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(order);
            _mockDbSet.Setup(m => m.Remove(It.IsAny<Order>()));
            _mockContext.Setup(c => c.Set<Order>()).Returns(_mockDbSet.Object);

            var repository = new Repository<Order>(_mockContext.Object, _mockLogger.Object);

            await repository.DeleteAsync(1);

            _mockDbSet.Verify(m => m.Remove(It.IsAny<Order>()), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}
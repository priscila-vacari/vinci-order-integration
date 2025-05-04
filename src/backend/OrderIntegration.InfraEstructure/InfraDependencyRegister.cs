using OrderIntegration.InfraEstructure.Context;
using OrderIntegration.InfraEstructure.Interfaces;
using OrderIntegration.InfraEstructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.InfraEstructure
{
    [ExcludeFromCodeCoverage]
    public static class InfraDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyContext(configuration);
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

            var mongoDbSettings = configuration.GetSection("MongoDB");
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = mongoDbSettings.GetSection("ConnectionString").Value ?? throw new InvalidOperationException("MongoDB connection string is not configured.");
                options.Database = mongoDbSettings.GetSection("Database").Value ?? throw new InvalidOperationException("MongoDB database is not configured.");
                options.Collection = mongoDbSettings.GetSection("Collection").Value ?? throw new InvalidOperationException("MongoDB collection is not configured.");
            });

            return services;
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            services.AddSingleton<IOrderCacheRepository, OrderCacheRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}

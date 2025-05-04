using Confluent.Kafka;
using OrderIntegration.Worker.Enums;
using OrderIntegration.Worker.Services;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Worker.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyServices(configuration);
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services, IConfiguration configuration) 
        {
            _ = Enum.TryParse(Environment.GetEnvironmentVariable("TYPE_SERVICE"), out ServiceType serviceType);

            services.AddHostedServiceWithCondition<CreateOrderWorker>(serviceType == ServiceType.CreateOrderService);

            services.AddSingleton<IConsumer<Ignore, string>>(sp =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = configuration["Kafka:BootstrapServers"],
                    GroupId = "order-consumer-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                return new ConsumerBuilder<Ignore, string>(config).Build();
            });

            return services;
        }
    }
}

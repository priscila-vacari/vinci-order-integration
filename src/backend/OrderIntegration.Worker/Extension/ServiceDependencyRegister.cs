using OrderIntegration.Worker.Enums;
using OrderIntegration.Worker.Services;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Worker.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services) 
        {
            _ = Enum.TryParse(Environment.GetEnvironmentVariable("TYPE_SERVICE"), out ServiceType serviceType);

            services.AddHostedServiceWithCondition<CreateOrderWorker>(serviceType == ServiceType.CreateOrderService);

            return services;
        }
    }
}

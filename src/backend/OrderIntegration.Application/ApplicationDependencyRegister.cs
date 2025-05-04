using OrderIntegration.Application.Interfaces;
using OrderIntegration.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services) 
        {
            services.AddTransient<IOrderService, OrderService>();

            return services;
        }
    }
}

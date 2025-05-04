using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Worker.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceDependencyExtension
    {
        public static IServiceCollection AddHostedServiceWithCondition<T>(this IServiceCollection services, bool condition = true) where T : class, IHostedService
        {
            if (condition)
                services.AddHostedService<T>();

            return services;
        }
    }
}

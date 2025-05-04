using AspNetCoreRateLimit;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Application
{
    /// <summary>
    /// Injeção de dependência para o Rate Limiting
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RateLimitDependencyRegister
    {
        /// <summary>
        /// Método para registrar os serviços de Rate Limiting
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyServices(configuration);
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            return services;
        }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Application
{
    /// <summary>
    /// Injeção de dependência para configurações json
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ConfigJsonDependencyRegister
    {
        /// <summary>
        /// Método para registrar os serviços de configuração json
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    });

            return services;
        }
    }
}

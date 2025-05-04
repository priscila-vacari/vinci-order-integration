using OrderIntegration.API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.Application
{
    /// <summary>
    /// Injeção de dependência para o FluentValidation
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ValidatorDependencyRegister
    {
        /// <summary>
        /// Método para registrar os serviços de validação
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<OrderRequestModelValidator>();

            return services;
        }
    }
}

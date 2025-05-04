using Asp.Versioning;
using OrderIntegration.API.Extensions.Swagger;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OrderIntegration.Application
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SwaggerDependencyRegister
    {
        /// <summary>
        /// Método para registrar os serviços de Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(opt => opt.First());
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(opt => opt.ToString());

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
    }
}

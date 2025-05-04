using Serilog;
using OrderIntegration.Application;
using System.Diagnostics.CodeAnalysis;
using OrderIntegration.Worker.Extension;
using OrderIntegration.InfraEstructure;

namespace OrderIntegration.Worker
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Iniciando o serviço Worker...");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "O serviço Worker encontrou um erro e foi encerrado.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    #region Services DI
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                    ServiceDependencyRegister.RegisterServices(services, hostContext.Configuration);
                    ApplicationDependencyRegister.RegisterServices(services);
                    InfraDependencyRegister.RegisterServices(services, hostContext.Configuration);
                    #endregion
                });
    }
}
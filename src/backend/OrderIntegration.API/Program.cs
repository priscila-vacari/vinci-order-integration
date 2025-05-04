using OrderIntegration.API.Middlewares;
using System.Reflection;
using OrderIntegration.Application;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using Asp.Versioning;
using Serilog;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using OrderIntegration.API.Filters;
using OrderIntegration.API.Validators;
using AspNetCoreRateLimit;
using System.Diagnostics.CodeAnalysis;
using OrderIntegration.API.Extensions.Swagger;
using OrderIntegration.InfraEstructure.Context;
using OrderIntegration.InfraEstructure;

namespace OrderIntegration.API
{
    /// <summary>
    /// Classe principal do programa
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Log
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });
            #endregion

            try
            {
                #region Services DI
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddHttpContextAccessor();
                builder.Services.AddHealthChecks();
                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                builder.Services.AddControllers(opt => opt.Filters.Add(typeof(ExceptionFilter)));
                RateLimitDependencyRegister.RegisterServices(builder.Services, builder.Configuration);
                ConfigJsonDependencyRegister.RegisterServices(builder.Services);
                ValidatorDependencyRegister.RegisterServices(builder.Services);
                SwaggerDependencyRegister.RegisterServices(builder.Services);
                ApplicationDependencyRegister.RegisterServices(builder.Services);
                InfraDependencyRegister.RegisterServices(builder.Services, builder.Configuration);
                #endregion

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();
                }

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();

                app.UseCors("AllowFrontend");

                app.UseHealthChecks("/health",
                    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                    {
                        ResponseWriter = async (context, report) =>
                        {
                            var result = JsonSerializer.Serialize(new
                            {
                                status = report.Status.ToString(),
                                healthChecks = report.Entries.Select(entry => new
                                {
                                    check = entry.Key,
                                    ErrorMessage = entry.Value.Exception?.Message,
                                    status = Enum.GetName(typeof(HealthStatus), entry.Value.Status),
                                    description = entry.Value.Description
                                })
                            });
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(result);
                        }
                    });

                app.UseCorrelationId();
                app.UseBruteForceProtection();
                app.UseSerilogRequestLogging();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal error when starting app.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
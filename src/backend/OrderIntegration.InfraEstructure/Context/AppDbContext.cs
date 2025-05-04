using OrderIntegration.Domain.Entities;
using OrderIntegration.InfraEstructure.MapConfigs;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.InfraEstructure.Context
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderMapConfig());
        }
    }
}

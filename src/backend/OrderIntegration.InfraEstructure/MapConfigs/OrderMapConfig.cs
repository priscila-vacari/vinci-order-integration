using OrderIntegration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.InfraEstructure.MapConfigs
{
    [ExcludeFromCodeCoverage]
    public class OrderMapConfig: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(k => new { k.Id });

            builder.Property(p => p.Cliente)
                .HasColumnName("cliente")
                .IsRequired();

            builder.Property(p => p.DataPedido)
                .HasColumnName("data")
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnName("valor")
                .IsRequired();
        }
    }
}

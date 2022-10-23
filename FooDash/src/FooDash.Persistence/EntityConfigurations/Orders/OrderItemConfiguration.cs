using FooDash.Domain.Entities.Orders;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Orders
{
    public class OrderItemConfiguration : EntityBaseConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId);
            builder.HasOne(x => x.Order)
                .WithMany(y => y.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
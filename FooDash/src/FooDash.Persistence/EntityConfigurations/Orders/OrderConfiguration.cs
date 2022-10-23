using FooDash.Domain.Entities.Orders;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Orders
{
    public class OrderConfiguration : EntityBaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.PhoneNumber).IsRequired();
            builder.Property(x => x.AddressLine1).IsRequired();
            builder.Property(x => x.ZipCode).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.Latitude).IsRequired();
            builder.Property(x => x.Longitude).IsRequired();
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.DeliveryCost).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(x => x.OrderingUserId).IsRequired(false);
            builder.HasOne(x => x.OrderingUser)
                .WithMany()
                .HasForeignKey(x => x.OrderingUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(x => x.CourierId).IsRequired(false);
            builder.HasOne(x => x.Courier)
                .WithMany()
                .HasForeignKey(x => x.CourierId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
using FooDash.Domain.Entities.Orders;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Orders
{
    public class CartConfiguration : EntityBaseConfiguration<Cart>
    {
        public override void Configure(EntityTypeBuilder<Cart> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.Property(x => x.UserId).IsRequired(false);
            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId);
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
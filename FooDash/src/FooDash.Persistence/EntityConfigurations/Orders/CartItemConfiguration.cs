using FooDash.Domain.Entities.Orders;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Orders
{
    public class CartItemConfiguration : EntityBaseConfiguration<CartItem>
    {
        public override void Configure(EntityTypeBuilder<CartItem> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Cart)
                .WithMany(y => y.CartItems)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
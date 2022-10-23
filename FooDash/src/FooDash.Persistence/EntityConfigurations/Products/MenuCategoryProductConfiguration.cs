using FooDash.Domain.Entities.Products;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Products
{
    public class MenuCategoryProductConfiguration : EntityBaseConfiguration<MenuCategoryProduct>
    {
        public override void Configure(EntityTypeBuilder<MenuCategoryProduct> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.MenuCategoryId).IsRequired();
            builder
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(x => x.MenuCategory)
                .WithMany(y => y.MenuCategoryProducts)
                .HasForeignKey(x => x.MenuCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using FooDash.Domain.Entities.Products;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Products
{
    public class ProductProductAttributeConfiguration : EntityBaseConfiguration<ProductProductAttribute>
    {
        public override void Configure(EntityTypeBuilder<ProductProductAttribute> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.ProductAttributeId).IsRequired();
            builder
                .HasOne(x => x.Product)
                .WithMany(y => y.ProductAttributes)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(x => x.ProductAttribute)
                .WithMany(y => y.AttributeProducts)
                .HasForeignKey(x => x.ProductAttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
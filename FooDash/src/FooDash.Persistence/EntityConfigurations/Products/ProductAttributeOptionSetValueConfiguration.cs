using FooDash.Domain.Entities.Products;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Products
{
    public class ProductAttributeOptionSetValueConfiguration : EntityBaseConfiguration<ProductAttributeOptionSetValue>
    {
        public override void Configure(EntityTypeBuilder<ProductAttributeOptionSetValue> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired();
            builder.HasOne(x => x.ProductAttribute)
                .WithMany(x => x.ProductAttributeOptionSetValues)
                .HasForeignKey(x => x.ProductAttributeId);
        }
    }
}
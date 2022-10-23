using FooDash.Domain.Entities.Products;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Products
{
    public class ProductAttributeConfiguration : EntityBaseConfiguration<ProductAttribute>
    {
        public override void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Order).IsRequired();
        }
    }
}
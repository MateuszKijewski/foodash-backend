using FooDash.Domain.Entities.Products;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Products
{
    public class MenuCategoryConfiguration : EntityBaseConfiguration<MenuCategory>
    {
        public override void Configure(EntityTypeBuilder<MenuCategory> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Menu)
                .WithMany(x => x.MenuCategories)
                .HasForeignKey(x => x.MenuId);
        }
    }
}
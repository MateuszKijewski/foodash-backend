using FooDash.Domain.Entities.Metadata;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Metadata
{
    public class EntityConfiguration : EntityBaseConfiguration<Entity>
    {
        public override void Configure(EntityTypeBuilder<Entity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.CreatePermissionKey).IsRequired();
            builder.Property(x => x.ReadPermissionKey).IsRequired();
            builder.Property(x => x.UpdatePermissionKey).IsRequired();
            builder.Property(x => x.DeletePermissionKey).IsRequired();
            builder.Property(x => x.IsTranslated)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
using FooDash.Domain.Entities.Identity;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Identity
{
    public class PermissionConfiguration : EntityBaseConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.PermissionType).IsRequired();
            builder.Property(x => x.EntityId).IsRequired();
            builder.HasOne(x => x.Entity)
                .WithMany()
                .HasForeignKey(x => x.EntityId);
        }
    }
}
using FooDash.Domain.Entities.Identity;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Identity
{
    public class RoleConfiguration : EntityBaseConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
using FooDash.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.Common.EntityConfigurations
{
    public abstract class EntityBaseConfiguration<EntityType> : IEntityTypeConfiguration<EntityType>
        where EntityType : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<EntityType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Modifier)
                .WithMany()
                .HasForeignKey(x => x.ModifierId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
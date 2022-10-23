using FooDash.Domain.Entities.Translations;
using FooDash.Persistence.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooDash.Persistence.EntityConfigurations.Translations
{
    public class LabelConfiguration : EntityBaseConfiguration<Label>
    {
        public override void Configure(EntityTypeBuilder<Label> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.TranslatedValue).IsRequired();
            builder.Property(x => x.EntityId).IsRequired();
            builder.HasOne(x => x.Entity)
                .WithMany()
                .HasForeignKey(x => x.EntityId);
            builder.Property(x => x.LanguageId).IsRequired();
            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Metadata;

namespace FooDash.Domain.Entities.Translations
{
    [SystemEntity]
    public class Label : EntityBase
    {
        public Guid EntityId { get; set; }
        public Entity Entity { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
        public string Value { get; set; }
        public string TranslatedValue { get; set; }
    }
}
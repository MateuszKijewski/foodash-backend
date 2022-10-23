using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Translations.Dtos.Basic
{
    public class ReadLabelDto : ReadDtoBase
    {
        public Guid EntityId { get; set; }
        public Guid LanguageId { get; set; }
        public string Value { get; set; }
        public string TranslatedValue { get; set; }
    }
}
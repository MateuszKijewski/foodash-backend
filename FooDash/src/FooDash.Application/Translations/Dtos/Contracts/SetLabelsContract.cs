namespace FooDash.Application.Translations.Dtos.Contracts
{
    public class SetLabelsContract
    {
        public Guid EntityId { get; set; }
        public Guid LanguageId { get; set; }
        public IEnumerable<ValueWithTranslation> ValuesWithTranslations { get; set; }
    }

    public class ValueWithTranslation
    {
        public string Value { get; set; }
        public string TranslatedValue { get; set; }
    }
}
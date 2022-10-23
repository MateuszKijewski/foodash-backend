using FooDash.Application.Translations.Dtos.Basic;
using FooDash.Domain.Entities.Translations;
using Mapster;

namespace FooDash.Application.MapperConfigurations
{
    public static class TranslationsMapperConfiguration
    {
        public static TypeAdapterConfig AddTranslationsConfiguration(this TypeAdapterConfig config)
        {
            config
                .NewConfig<LabelDto, Label>();
            config
                .NewConfig<Label, ReadLabelDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<LanguageDto, Language>();
            config
                .NewConfig<Language, ReadLanguageDto>()
                .IgnoreNullValues(true);

            return config;
        }
    }
}
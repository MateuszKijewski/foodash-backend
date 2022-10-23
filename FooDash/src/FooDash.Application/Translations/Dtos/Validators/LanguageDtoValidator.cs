using FluentValidation;
using FooDash.Application.Translations.Dtos.Basic;

namespace FooDash.Application.Translations.Dtos.Validators
{
    public class LanguageDtoValidator : AbstractValidator<LanguageDto>
    {
        public LanguageDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty();
        }
    }
}

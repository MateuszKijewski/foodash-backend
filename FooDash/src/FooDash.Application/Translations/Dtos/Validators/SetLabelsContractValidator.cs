using FluentValidation;
using FooDash.Application.Translations.Dtos.Contracts;

namespace FooDash.Application.Translations.Dtos.Validators
{
    public class SetLabelsContractValidator : AbstractValidator<SetLabelsContract>
    {
        public SetLabelsContractValidator()
        {
            RuleFor(x => x.EntityId).NotEmpty();
            RuleFor(x => x.LanguageId).NotEmpty();
            RuleFor(x => x.ValuesWithTranslations).NotEmpty();
        }
    }
}
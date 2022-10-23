using FluentValidation;
using FooDash.Application.Metadata.Dtos.Contracts;

namespace FooDash.Application.Metadata.Dtos.Validators
{
    public class SetEntityTranslatabilityContractValidator : AbstractValidator<SetEntityTranslatabilityContract>
    {
        public SetEntityTranslatabilityContractValidator()
        {
            RuleFor(x => x.EntityId).NotEmpty();
            RuleFor(x => x.IsTranslatable).NotEmpty();
        }
    }
}
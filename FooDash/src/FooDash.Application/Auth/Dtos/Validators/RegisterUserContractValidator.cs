using FluentValidation;
using FooDash.Application.Auth.Dtos.Contracts;

namespace FooDash.Application.Auth.Dtos.Validators
{
    public class RegisterUserContractValidator : AbstractValidator<RegisterUserContract>
    {
        public RegisterUserContractValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
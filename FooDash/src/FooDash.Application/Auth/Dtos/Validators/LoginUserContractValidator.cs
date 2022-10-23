using Dawn;
using FluentValidation;
using FooDash.Application.Auth.Dtos.Contracts;

namespace FooDash.Application.Auth.Dtos.Validators
{
    public class LoginUserContractValidator : AbstractValidator<LoginUserContract>
    {
        public LoginUserContractValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
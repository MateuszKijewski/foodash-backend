using FluentValidation;
using FooDash.Application.Auth.Dtos.Contracts;

namespace FooDash.Application.Auth.Dtos.Validators
{
    public class RefreshTokenContractValidator : AbstractValidator<RefreshTokenContract>
    {
        public RefreshTokenContractValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
using FluentValidation;
using FooDash.Application.Users.Dtos.Basic;

namespace FooDash.Application.Users.Dtos.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.LanguageId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}
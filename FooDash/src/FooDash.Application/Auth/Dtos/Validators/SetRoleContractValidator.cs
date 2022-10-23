using FluentValidation;
using FooDash.Application.Auth.Dtos.Contracts;

namespace FooDash.Application.Auth.Dtos.Validators
{
    public class SetRoleContractValidator : AbstractValidator<SetRoleContract>
    {
        public SetRoleContractValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
using Dawn;
using FluentValidation;
using FooDash.Application.Auth.Dtos.Contracts;

namespace FooDash.Application.Auth.Dtos.Validators
{
    public class SetRolePermissionsContractValidator : AbstractValidator<SetRolePermissionsContract>
    {
        public SetRolePermissionsContractValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.PermissionIds).NotEmpty();
        }
    }
}
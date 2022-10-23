using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class MenuDtoValidator : AbstractValidator<MenuDto>
    {
        public MenuDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
        }
    }
}

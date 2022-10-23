using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class MenuCategoryDtoValidator : AbstractValidator<MenuCategoryDto>
    {
        public MenuCategoryDtoValidator()
        {
            RuleFor(x => x.MenuId).NotEmpty();
        }
    }
}
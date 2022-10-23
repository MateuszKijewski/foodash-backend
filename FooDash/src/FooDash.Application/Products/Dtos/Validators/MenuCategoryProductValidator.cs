using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class MenuCategoryProductValidator : AbstractValidator<MenuCategoryProductDto>
    {
        public MenuCategoryProductValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.MenuCategoryId).NotEmpty();
        }
    }
}
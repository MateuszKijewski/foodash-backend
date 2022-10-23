using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class ProductCategoryDtoValidator : AbstractValidator<ProductCategoryDto>
    {
        public ProductCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

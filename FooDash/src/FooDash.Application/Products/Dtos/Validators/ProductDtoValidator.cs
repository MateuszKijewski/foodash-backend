using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
        }
    }
}

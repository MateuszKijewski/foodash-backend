using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class ProductAttributeDtoValidator : AbstractValidator<ProductAttributeDto>
    {
        public ProductAttributeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AttributeType).NotEmpty();
        }
    }
}

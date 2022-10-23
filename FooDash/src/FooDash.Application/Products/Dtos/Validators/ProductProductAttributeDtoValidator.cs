using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class ProductProductAttributeDtoValidator : AbstractValidator<ProductProductAttributeDto>
    {
        public ProductProductAttributeDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductAttributeId).NotEmpty();
        }
    }
}
using FluentValidation;
using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class ProductAttributeOptionSetValueDtoValidator : AbstractValidator<ProductAttributeOptionSetValueDto>
    {
        public ProductAttributeOptionSetValueDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductAttributeId).NotEmpty();
        }
    }
}

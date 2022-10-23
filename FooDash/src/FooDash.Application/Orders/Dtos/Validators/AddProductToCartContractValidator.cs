using FluentValidation;
using FooDash.Application.Orders.Dtos.Contracts;

namespace FooDash.Application.Orders.Dtos.Validators
{
    public class AddProductToCartContractValidator : AbstractValidator<AddProductToCartContract>
    {
        public AddProductToCartContractValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
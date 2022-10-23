using FluentValidation;
using FooDash.Application.Products.Dtos.Contracts;

namespace FooDash.Application.Products.Dtos.Validators
{
    public class GetActiveWithProductsForGuestContractValidator : AbstractValidator<GetActiveWithProductsForGuestContract>
    {
        public GetActiveWithProductsForGuestContractValidator()
        {
            RuleFor(x => x.GuestCurrencyId).NotEmpty();
            RuleFor(x => x.GuestLanguageId).NotEmpty();
        }
    }
}
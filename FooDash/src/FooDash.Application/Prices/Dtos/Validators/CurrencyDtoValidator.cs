using FluentValidation;
using FooDash.Application.Prices.Dtos.Basic;

namespace FooDash.Application.Prices.Dtos.Validators
{
    public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
    {
        public CurrencyDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty();
            RuleFor(x => x.ExchangeRate).NotEmpty();
            RuleFor(x => x.IsBase).NotEmpty();
        }
    }
}
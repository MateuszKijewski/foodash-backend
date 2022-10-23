using FooDash.Application.Prices.Dtos.Basic;

namespace FooDash.Application.Common.Interfaces.Prices
{
    public interface ICurrencyService
    {
        Task ValidateIfOnlyOneCurrencyIsActive(List<CurrencyDto> currencyDtos);
    }
}
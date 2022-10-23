using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Prices;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Prices.Dtos.Basic;

namespace FooDash.Application.Prices.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = Guard.Argument(currencyRepository).NotNull().Value;
        }

        public async Task ValidateIfOnlyOneCurrencyIsActive(List<CurrencyDto> currencyDtos)
        {
            var addedBaseCurrenciesCount = currencyDtos.Where(x => x.IsBase).Count();
            if (addedBaseCurrenciesCount == 0)
                return;
            if (addedBaseCurrenciesCount > 1)
                throw new BadRequestException("There can only be one base currency at a time");

            var activeMenuExists = _currencyRepository.GetBase() != null;
            if (activeMenuExists)
                throw new BadRequestException("There can only be one base currency at a time");
        }
    }
}

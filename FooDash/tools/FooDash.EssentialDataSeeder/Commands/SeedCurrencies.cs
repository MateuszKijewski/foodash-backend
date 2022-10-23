using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Domain.Entities.Prices;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedCurrencies : SeedCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SeedCurrencies(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
        }

        public override void Execute()
        {
            AddCurrencies().GetAwaiter().GetResult();
        }

        private async Task AddCurrencies()
        {
            var currencyRepository = _baseRepositoryProvider.GetRepository<Currency>();

            var currencies = new List<Currency>
            {
                new Currency
                {
                    Name = "Złoty",
                    Code = "PLN",
                    Symbol = "zł",
                    ExchangeRate = 1,
                    IsBase = true
                },
                new Currency
                {
                    Name = "Dollar",
                    Code = "USD",
                    Symbol = "$",
                    ExchangeRate = 3.80m,
                    IsBase = false
                },
                new Currency
                {
                    Name = "Euro",
                    Code = "EUR",
                    Symbol = "€",
                    ExchangeRate = 4.40m,
                    IsBase = false
                }
            };

            await currencyRepository.AddRangeAsync(currencies);
        }
    }
}
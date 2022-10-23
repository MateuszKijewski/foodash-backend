using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.System;
using FooDash.Domain.Entities.Translations;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedSettings : SeedCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SeedSettings(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
        }

        public override void Execute()
        {
            AddDefaultSystemSettings().GetAwaiter().GetResult();
        }

        private async Task AddDefaultSystemSettings()
        {
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();
            var currencyRepository = _baseRepositoryProvider.GetRepository<Currency>();
            var languageRepository = _baseRepositoryProvider.GetRepository<Language>();

            var polishLanguage = (await languageRepository.FindAsync(x => x.Symbol == "pl-pl")).First();
            var polishCurrency = (await currencyRepository.FindAsync(x => x.Code == "PLN")).First();

            await settingsRepository.AddAsync(new Settings
            {
                Name = "SystemSettings",
                DefaultLanguageId = polishLanguage.Id,
                DefaultCurrencyId = polishCurrency.Id
            });
        }
    }
}
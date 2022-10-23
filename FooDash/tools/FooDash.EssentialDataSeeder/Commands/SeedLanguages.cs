using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Domain.Entities.Translations;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedLanguages : SeedCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SeedLanguages(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
        }

        public override void Execute()
        {
            AddLanguages().GetAwaiter().GetResult();
        }

        private async Task AddLanguages()
        {
            var languageRepository = _baseRepositoryProvider.GetRepository<Language>();

            var languages = new List<Language>
            {
                new Language
                {
                    Name = "Polish",
                    Symbol = "pl-pl"
                },
                new Language
                {
                    Name = "English",
                    Symbol = "en-us"
                }
            };

            await languageRepository.AddRangeAsync(languages);
        }
    }
}
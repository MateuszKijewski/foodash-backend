using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Security;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Translations;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedUsers : SeedCommand
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SeedUsers(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = Guard.Argument(configuration, nameof(configuration)).NotNull().Value;
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
        }

        public override void Execute()
        {
            AddSuperAdmin().GetAwaiter().GetResult();
        }

        private async Task AddSuperAdmin()
        {
            var roleRepository = _baseRepositoryProvider.GetRepository<Role>();
            var userRepository = _baseRepositoryProvider.GetRepository<User>();
            var languageRepository = _baseRepositoryProvider.GetRepository<Language>();
            var currencyRepository = _baseRepositoryProvider.GetRepository<Currency>();

            var superAdminRole = (await roleRepository.FindAsync(x => x.Name == "SuperAdmin")).First();
            var polishLanguage = (await languageRepository.FindAsync(x => x.Symbol == "pl-pl")).First();
            var polishCurrency = (await currencyRepository.FindAsync(x => x.Code == "PLN")).First();

            var hashingService = _serviceProvider.GetService<IHashingService>();
            var seederOptions = _configuration.GetSection(nameof(SeederOptions)).Get<SeederOptions>();

            byte[] salt = hashingService.GetSalt();
            string hashedPassword = hashingService.Hash(seederOptions.SuperAdminPassword, salt);

            await userRepository.AddAsync(new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "SuperAdmin",
                Email = "admin@foodash.com",
                Salt = salt,
                HashedPassword = hashedPassword,
                RoleId = superAdminRole.Id,
                LanguageId = polishLanguage.Id,
                CurrencyId = polishCurrency.Id
            });
        }
    }
}
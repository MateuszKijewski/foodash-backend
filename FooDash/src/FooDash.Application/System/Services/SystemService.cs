using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.System;
using FooDash.Application.System.Dtos.Contracts;
using FooDash.Application.System.Dtos.Responses;
using FooDash.Domain.Entities.System;
using Mapster;

namespace FooDash.Application.System.Services
{
    public class SystemService : ISystemService
    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SystemService(IBaseRepositoryProvider baseRepositoryProvider)
        {
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider).NotNull().Value;
        }

        public async Task ChangeSystemSettings(ChangeSettingsContract changeSettingsContract)
        {
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();

            var systemSettings = (await settingsRepository.GetAllAsync()).First();

            if (changeSettingsContract.DefaultLanguageId.HasValue)
                systemSettings.DefaultLanguageId = changeSettingsContract.DefaultLanguageId.Value;

            if (changeSettingsContract.DefaultCurrencyId.HasValue)
                systemSettings.DefaultCurrencyId = changeSettingsContract.DefaultCurrencyId.Value;

            await settingsRepository.UpdateAsync(systemSettings);
        }

        public async Task<GetSettingsResponse> GetSystemSettings()
        {
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();

            var systemSettings = (await settingsRepository.GetAllAsync(nameof(Settings.DefaultLanguage), nameof(Settings.DefaultCurrency))).First();

            return systemSettings.Adapt<GetSettingsResponse>();
        }
    }
}
using FooDash.Application.System.Dtos.Contracts;
using FooDash.Application.System.Dtos.Responses;

namespace FooDash.Application.Common.Interfaces.System
{
    public interface ISystemService
    {
        Task ChangeSystemSettings(ChangeSettingsContract changeSettingsContract);

        Task<GetSettingsResponse> GetSystemSettings();
    }
}
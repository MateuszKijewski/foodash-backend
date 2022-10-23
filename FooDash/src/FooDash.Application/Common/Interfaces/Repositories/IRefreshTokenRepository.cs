using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken> GetByTokenWithUser(string token);
    }
}
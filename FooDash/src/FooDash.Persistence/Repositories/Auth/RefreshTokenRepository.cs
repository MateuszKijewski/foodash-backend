using Dawn;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Identity;
using FooDash.Persistence.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FooDash.Persistence.Repositories.Auth
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly FooDashDbContext _dbContext;

        public RefreshTokenRepository(FooDashDbContext dbContext) : base(dbContext)
        {
            _dbContext = Guard.Argument(dbContext, nameof(dbContext)).NotNull();
        }

        public async Task<RefreshToken> GetByTokenWithUser(string token)
        {
            return await _dbContext.RefreshToken
                .Include(x => x.User)
                .SingleAsync(x => x.Token == token);
        }
    }
}
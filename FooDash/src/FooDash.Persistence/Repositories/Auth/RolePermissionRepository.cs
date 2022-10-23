using Dawn;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Identity;
using FooDash.Persistence.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FooDash.Persistence.Repositories.Auth
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly FooDashDbContext _dbContext;

        public RolePermissionRepository(FooDashDbContext dbContext) : base(dbContext)
        {
            _dbContext = Guard.Argument(dbContext, nameof(dbContext)).NotNull();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleId(Guid? roleId)
        {
            return await _dbContext.RolePermission
                .Where(x => x.RoleId == roleId)
                .Include(x => x.Permission)
                .Select(x => x.Permission).ToListAsync();
        }
    }
}
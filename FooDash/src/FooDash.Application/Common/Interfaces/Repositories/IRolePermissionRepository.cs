using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        Task<IEnumerable<Permission>> GetPermissionsByRoleId(Guid? roleId);
    }
}
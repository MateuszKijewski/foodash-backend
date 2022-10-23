using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Application.Auth.Dtos.Responses;

namespace FooDash.Application.Common.Interfaces.Auth
{
    public interface IPermissionService
    {
        Task SetPermissionsForRole(SetRolePermissionsContract setRolePermissionsContract);

        Task<IEnumerable<GetAllPermissionsResponse>> GetAllPermissions();

        Task<IEnumerable<GetAllRolesResponse>> GetAllRoles();
    }
}
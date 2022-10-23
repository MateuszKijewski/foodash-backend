using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Domain.Common.Statics;
using FooDash.Domain.Entities.Identity;
using FooDash.Application.Auth.Dtos.Responses;

namespace FooDash.Application.Auth.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public PermissionService(IBaseRepositoryProvider baseRepositoryProvider)
        {
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider, nameof(baseRepositoryProvider)).NotNull().Value;
        }

        public async Task SetPermissionsForRole(SetRolePermissionsContract setRolePermissionsContract)
        {
            var rolePermissionRepository = _baseRepositoryProvider.GetRepository<RolePermission>();

            var role = await GetRole(setRolePermissionsContract.RoleId);
            if (role.Name == PredefinedRoles.SuperAdmin)
                throw new BadRequestException("Modifying SuperAdmin permissions is forbidden");

            var permissionsToAdd = await GetPermissions(setRolePermissionsContract.PermissionIds);
            var userPermissionsToDelete = await rolePermissionRepository.FindAsync(x => x.RoleId == role.Id);
            await rolePermissionRepository.RemoveRangeAsync(userPermissionsToDelete);


            await rolePermissionRepository.AddRangeAsync(permissionsToAdd.Select(x => new RolePermission
            {
                RoleId = setRolePermissionsContract.RoleId,
                PermissionId = x.Id
            }));
        }

        public async Task<IEnumerable<GetAllRolesResponse>> GetAllRoles()
        {
            var rolePermissionRepository = _baseRepositoryProvider.GetRepository<RolePermission>();
            var allRolesPermissions = await rolePermissionRepository.GetAllAsync(nameof(RolePermission.Role), nameof(RolePermission.Permission));

            var results = new List<GetAllRolesResponse>();

            var allRoles = allRolesPermissions.Select(x => x.Role).Distinct();

            foreach (var role in allRoles)
            {
                results.Add(new GetAllRolesResponse
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Permissions = allRolesPermissions.Where(x => x.RoleId == role.Id).Select(y => new BasicPermissionInfo
                    {
                        Id = y.Permission.Id,
                        Name = y.Permission.Name,
                        Type = y.Permission.PermissionType
                    })
                });
            }

            return results;
        }

        public async Task<IEnumerable<GetAllPermissionsResponse>> GetAllPermissions()
        {
            var permissionRepository = _baseRepositoryProvider.GetRepository<Permission>();

            var allPermissions = await permissionRepository.GetAllAsync(nameof(Permission.Entity));
            var result = allPermissions.Select(x => new GetAllPermissionsResponse
            {
                PermissionId = x.Id,
                PermissionType = x.PermissionType,
                PermissionName = x.Name,
                Entity = new BasicEntityInfo
                {
                    Id = x.Entity.Id,
                    Name = x.Entity.Name
                }
            });

            return result;
        }

        private async Task<Role> GetRole(Guid roleId)
        {
            var roleRepository = _baseRepositoryProvider.GetRepository<Role>();

            try
            {
                return await roleRepository.GetAsync(roleId);
            }
            catch
            {
                throw new BadRequestException($"Role with Id: {roleId} was not found");
            }
        }

        private async Task<List<Permission>> GetPermissions(Guid[] permissionIds)
        {
            var permissionRepository = _baseRepositoryProvider.GetRepository<Permission>();

            try
            {
                return (await permissionRepository.GetAsync(permissionIds)).ToList();
            }
            catch
            {
                throw new BadRequestException($"One or more specified permissions were not found");
            }
        }
    }
}
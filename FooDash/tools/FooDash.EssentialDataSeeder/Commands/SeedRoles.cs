using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Common.Statics;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Metadata;
using FooDash.Domain.Entities.Products;
using FooDash.Domain.Entities.Translations;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedRoles : SeedCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;
        private IBaseRepository<Role> _roleRepository;
        private IBaseRepository<Permission> _permissionRepository;
        private IBaseRepository<RolePermission> _rolePermissionRepository;
        private IBaseRepository<Entity> _entityRepository;

        public SeedRoles(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
            _roleRepository = _baseRepositoryProvider.GetRepository<Role>();
            _permissionRepository = _baseRepositoryProvider.GetRepository<Permission>();
            _rolePermissionRepository = _baseRepositoryProvider.GetRepository<RolePermission>();
            _entityRepository = _baseRepositoryProvider.GetRepository<Entity>();
        }

        private Dictionary<string, List<PermissionType>> _permissionsForEntities = new Dictionary<string, List<PermissionType>>();

        public override void Execute()
        {
            AddPredefinedRole(PredefinedRoles.SuperAdmin).GetAwaiter().GetResult();
            AddPredefinedRole(PredefinedRoles.Administrator).GetAwaiter().GetResult();
            AddPredefinedRole(PredefinedRoles.Manager).GetAwaiter().GetResult();
            AddPredefinedRole(PredefinedRoles.Employee).GetAwaiter().GetResult();
        }

        private async Task AddPredefinedRole(string roleName)
        {
            var role = await _roleRepository.AddAsync(new Role {  Name = roleName });
            var permissionsForRole = await GetPermissionsForRole(role);

            await _rolePermissionRepository.AddRangeAsync(permissionsForRole.Select(x => new RolePermission { RoleId = role.Id, PermissionId = x.Id }));
        }
        private async Task<List<Permission>> GetPermissionsForRole(Role role)
        {
            var allEntities = await _entityRepository.GetAllAsync();
            var allPermissions = await _permissionRepository.GetAllAsync();
            foreach (var e in allEntities)
                _permissionsForEntities.Add(e.Name, new List<PermissionType>());


            switch (role.Name)
            {
                case PredefinedRoles.SuperAdmin:
                case PredefinedRoles.Administrator:
                    foreach (var entityName in allEntities.Select(x => x.Name))
                        AddAdminPermissionsForEntity(entityName);
                    break;

                case PredefinedRoles.Manager:
                    AddReaderPermissionsForEntity(nameof(User));
                    AddReaderPermissionsForEntity(nameof(Language));
                    AddAdminPermissionsForEntity(nameof(Label));
                    AddAdminPermissionsForEntity(nameof(Product));
                    break;

                case PredefinedRoles.Employee:
                    AddReaderPermissionsForEntity(nameof(User));
                    AddReaderPermissionsForEntity(nameof(Language));
                    AddReaderPermissionsForEntity(nameof(Product));
                    break;

                default:
                    throw new Exception("Given role was not predefined");
            }

            var permissionsToAdd = new List<Permission>();
            Entity entity;
            foreach (var permissionsForEntity in _permissionsForEntities)
            {
                entity = allEntities.First(x => x.Name == permissionsForEntity.Key);
                permissionsToAdd.AddRange(allPermissions.Where(x => x.EntityId == entity.Id && permissionsForEntity.Value.Contains(x.PermissionType)).ToList());
            }

            _permissionsForEntities.Clear();
            return permissionsToAdd;
        }

        private void AddReaderPermissionsForEntity(string entityName)
        {
            _permissionsForEntities[entityName].Add(PermissionType.Read);
        }

        private void AddModifierPermissionsForEntity(string entityName)
        {
            _permissionsForEntities[entityName].Add(PermissionType.Create);
            _permissionsForEntities[entityName].Add(PermissionType.Read);
            _permissionsForEntities[entityName].Add(PermissionType.Update);
        }

        private void AddAdminPermissionsForEntity(string entityName)
        {
            _permissionsForEntities[entityName].Add(PermissionType.Create);
            _permissionsForEntities[entityName].Add(PermissionType.Read);
            _permissionsForEntities[entityName].Add(PermissionType.Update);
            _permissionsForEntities[entityName].Add(PermissionType.Delete);
        }
    }
}
using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Metadata;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedPermissions : SeedCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SeedPermissions(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
        }

        public override void Execute()
        {
            AddAllEntityPermissions().GetAwaiter().GetResult();
        }

        private async Task AddAllEntityPermissions()
        {
            var entityRepository = _baseRepositoryProvider.GetRepository<Entity>();
            var permissionRepository = _baseRepositoryProvider.GetRepository<Permission>();

            var allEntities = await entityRepository.GetAllAsync();
            var permissions = GeneratePermissions(allEntities);

            await permissionRepository.AddRangeAsync(permissions);
        }

        private List<Permission> GeneratePermissions(IEnumerable<Entity> entities)
        {
            var permissions = new List<Permission>();

            foreach (var entity in entities)
            {
                permissions.AddRange(new List<Permission>
                {
                    new Permission{ Id = entity.CreatePermissionKey, EntityId = entity.Id, Name = $"{entity.Name}_{PermissionType.Create}_Permission", PermissionType = PermissionType.Create },
                    new Permission{ Id = entity.ReadPermissionKey, EntityId = entity.Id, Name = $"{entity.Name}_{PermissionType.Read}_Permission", PermissionType = PermissionType.Read },
                    new Permission{ Id = entity.UpdatePermissionKey, EntityId = entity.Id, Name = $"{entity.Name}_{PermissionType.Update}_Permission", PermissionType = PermissionType.Update},
                    new Permission{ Id = entity.DeletePermissionKey, EntityId = entity.Id, Name = $"{entity.Name}_{PermissionType.Delete}_Permission", PermissionType = PermissionType.Delete }
                });
            }

            return permissions;
        }
    }
}
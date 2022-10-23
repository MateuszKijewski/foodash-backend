using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Metadata;
using FooDash.EssentialDataSeeder.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Commands
{
    public class SeedEntities : SeedCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public SeedEntities(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
            _baseRepositoryProvider = _serviceProvider.GetService<IBaseRepositoryProvider>();
        }

        public override void Execute()
        {
            AddAllDomainEntities().GetAwaiter().GetResult();
        }

        private async Task AddAllDomainEntities()
        {
            var entityRepository = _baseRepositoryProvider.GetRepository<Entity>();
            var domainEntitiesTypes = GetDomainEntitiesTypes();

            var entities = domainEntitiesTypes.Select(x => new Entity { Name = x.Name }).ToList();
            GeneratePermissionKeys(entities);

            await entityRepository.AddRangeAsync(entities);
        }


        private static IEnumerable<Type> GetDomainEntitiesTypes()
        {
            return typeof(EntityBase).Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(EntityBase)));
        }

        private static void GeneratePermissionKeys(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatePermissionKey = Guid.NewGuid();
                entity.ReadPermissionKey = Guid.NewGuid();
                entity.UpdatePermissionKey = Guid.NewGuid();
                entity.DeletePermissionKey = Guid.NewGuid();
            }
        }
    }
}
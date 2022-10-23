using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Metadata;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Metadata.Dtos.Contracts;
using FooDash.Application.Metadata.Dtos.Responses;
using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Metadata;
using Mapster;

namespace FooDash.Application.Metadata.Services
{
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository _entityRepository;

        public EntityService(IEntityRepository entityRepository)
        {
            _entityRepository = Guard.Argument(entityRepository, nameof(entityRepository)).NotNull().Value;
        }

        public async Task SetEntityTranslatability(SetEntityTranslatabilityContract setEntityTranslatabilityContract)
        {
            var entity = await _entityRepository.GetAsync(setEntityTranslatabilityContract.EntityId);

            var systemEntities = GetEntitiesByType(RequestedEntitiesType.SystemOnly);

            var isSystemEntity = systemEntities.Contains(entity);

            if (isSystemEntity)
                throw new BadRequestException("Cannot set translatability of system entity");


            entity.IsTranslated = setEntityTranslatabilityContract.IsTranslatable;
            await _entityRepository.UpdateAsync(entity);
        }

        public IEnumerable<GetEntitiesResponse> GetEntities(RequestedEntitiesType requestedEntitiesType)
        {
            var requestedEntities = GetEntitiesByType(requestedEntitiesType);

            return requestedEntities.Select(x => x.Adapt<GetEntitiesResponse>());
        }

        private IEnumerable<Entity> GetEntitiesByType(RequestedEntitiesType requestedEntitiesType)
        {
            var systemEntitiesTypes = GetEntityTypes(requestedEntitiesType);
            var systemEntities = _entityRepository.GetByNames(systemEntitiesTypes.Select(x => x.Name).ToArray()).ToList();

            return systemEntities;
        }

        private static IEnumerable<Type> GetEntityTypes(RequestedEntitiesType requestedEntitiesType)
        {
            var assembly = typeof(EntityBase).Assembly;
            switch (requestedEntitiesType)
            {
                case RequestedEntitiesType.All:
                    foreach (Type type in assembly.GetTypes())
                    {
                        yield return type;
                    }
                    break;

                case RequestedEntitiesType.SystemOnly:
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttributes(typeof(SystemEntityAttribute), true).Length > 0)
                        {
                            yield return type;
                        }
                    }
                    break;

                case RequestedEntitiesType.NonSystemOnly:
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttributes(typeof(SystemEntityAttribute), true).Length <= 0)
                        {
                            yield return type;
                        }
                    }
                    break;
            }
        }        
    }
}

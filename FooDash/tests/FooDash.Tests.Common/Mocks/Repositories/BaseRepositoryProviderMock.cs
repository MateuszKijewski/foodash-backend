using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Common.Entities;
using Moq;
using System.Linq.Expressions;

namespace FooDash.Tests.Common.Mocks.Repositories
{
    public class BaseRepositoryProviderMock
    {
        public virtual TRepository GetMockRepository<TEntity, TRepository>(ICollection<TEntity> storage)
            where TEntity : EntityBase
            where TRepository : class, IBaseRepository<TEntity>

            => GetMock<TEntity, TRepository>(storage).Object;

        protected Mock<TRepository> GetMock<TEntity, TRepository>(ICollection<TEntity> storage)
            where TEntity : EntityBase
            where TRepository : class, IBaseRepository<TEntity>
        {
            var mockRepository = new Mock<TRepository>();
            mockRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(storage);

            mockRepository
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .Returns<Guid>((id) =>
                {
                    return Task.FromResult(storage.SingleOrDefault(x => x.Id == id));
                });

            mockRepository
                .Setup(x => x.GetAsync(It.IsAny<Guid[]>()))
                .Returns<Guid[]>((ids) =>
                {
                    return Task.FromResult(storage.Where(x => ids.Contains(x.Id)));
                });

            mockRepository
                .Setup(x => x.UpdateAsync(It.IsAny<TEntity>()))
                .Returns<TEntity>((entity) =>
                {
                    var existingEntity = storage.SingleOrDefault(e => e.Id == entity.Id);
                    var unboxedEntity = (TEntity)((object)entity);
                    if (existingEntity != null)
                    {
                        storage.Remove(existingEntity);
                        storage.Add(unboxedEntity);
                    }
                    return Task.FromResult(unboxedEntity);
                });

            mockRepository
                .Setup(x => x.UpdateRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                .Returns<IEnumerable<TEntity>>((entities) =>
                {
                    var unboxedEntities = new List<TEntity>();
                    foreach(var entity in entities)
                    {
                        var existingEntity = storage.SingleOrDefault(e => e.Id == entity.Id);
                        var unboxedEntity = (TEntity)((object)entity);
                        if (existingEntity != null)
                        {
                            storage.Remove(existingEntity);
                            storage.Add(unboxedEntity);
                            unboxedEntities.Add(unboxedEntity);
                        }
                    }
                    return Task.FromResult(unboxedEntities.AsEnumerable());
                });

            mockRepository
                .Setup(x => x.RemoveAsync(It.IsAny<Guid>()))
                .Returns<Guid>((id) =>
                {
                    var entity = storage.First(x => x.Id == id);
                    storage.Remove(entity);

                    return Task.CompletedTask;
                });

            mockRepository
                .Setup(x => x.RemoveAsync(It.IsAny<TEntity>()))
                .Returns<TEntity>((entity) =>
                {
                    var entityToDelete = storage.First(x => x.Id == entity.Id);
                    storage.Remove(entityToDelete);

                    return Task.CompletedTask;
                });

            mockRepository
                .Setup(x => x.RemoveRangeAsync(It.IsAny<Guid[]>()))
                .Returns<Guid[]>((ids) =>
                {
                    var entities = storage.Where(x => ids.Contains(x.Id));
                    entities.Select(x => storage.Remove(x));

                    return Task.CompletedTask;
                });

            mockRepository
                .Setup(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                .Returns<IEnumerable<TEntity>>((entities) =>
                {
                    var entitiesToDelete = storage.Where(x => entities.Select(x => x.Id).Contains(x.Id)).ToArray();
                    for (int i = 0; i < entitiesToDelete.Count(); i++)
                    {
                        storage.Remove(entitiesToDelete[i]);
                    }

                    return Task.CompletedTask;
                });

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<TEntity>()))
                .Returns<TEntity>((entity) =>
                {
                    entity.Id = Guid.NewGuid();
                    storage.Add(entity);

                    return Task.FromResult(entity);
                });

            mockRepository
                .Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                .Returns<IEnumerable<TEntity>>((entities) =>
                {
                    foreach (var entity in entities)
                    {
                        entity.Id = Guid.NewGuid();
                        storage.Add(entity);
                    }
                    return Task.FromResult(entities.AsEnumerable());
                });

            mockRepository.Setup(x => x.FindAsync(It.IsNotNull<Expression<Func<TEntity, bool>>>(), It.IsAny<string[]>())).Returns<Expression<Func<TEntity, bool>>, string[]>((predicate, expandProperties) =>
            {
                var entities = storage.Where(predicate.Compile());

                return Task.FromResult(entities);
            });

            return mockRepository;
        }
    }
}
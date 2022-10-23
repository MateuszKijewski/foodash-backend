using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Common.Entities;
using FooDash.Tests.Common.Mocks.Repositories;

namespace FooDash.Tests.Common.Mocks
{
    public class MockedRepositoryProvider : BaseRepositoryProviderMock, IBaseRepositoryProvider

    {
        public Dictionary<Type, object> Storage = new Dictionary<Type, object>();

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        {
            return GetType()
               .GetMethod(nameof(GetMockedRepository))
               .MakeGenericMethod(typeof(TEntity))
               .Invoke(this, null) as IBaseRepository<TEntity>;
        }

        public object GetMockedRepository<TEntity>() where TEntity : EntityBase
        {
            if (Storage.ContainsKey(typeof(TEntity)))
            {
                return Storage[typeof(TEntity)];
            }

            Storage[typeof(TEntity)] = GetMockRepository<TEntity, IBaseRepository<TEntity>>(new List<TEntity>());

            return Storage[typeof(TEntity)];
        }
    }
}
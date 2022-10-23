using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Common.Entities;

namespace FooDash.Application.Common.Interfaces.Providers
{
    public interface IBaseRepositoryProvider
    {
        IBaseRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase;
    }
}
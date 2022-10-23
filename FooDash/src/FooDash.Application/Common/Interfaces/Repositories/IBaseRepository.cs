using FooDash.Domain.Common.Entities;
using System.Linq.Expressions;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity> GetAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAsync(Guid[] ids);

        Task<IEnumerable<TEntity>> GetAllAsync(params string[] expandProperties);

        Task<TEntity> AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(Guid id);

        Task RemoveAsync(TEntity entity);

        Task RemoveRangeAsync(Guid[] ids);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params string[] expandProperties);
    }
}
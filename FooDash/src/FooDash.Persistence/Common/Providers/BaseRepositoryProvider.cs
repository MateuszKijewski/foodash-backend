using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Common.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.Persistence.Common.Providers
{
    public class BaseRepositoryProvider : IBaseRepositoryProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public BaseRepositoryProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = Guard.Argument(serviceProvider, nameof(serviceProvider)).NotNull().Value;
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase
        {
            return _serviceProvider.GetRequiredService<IBaseRepository<TEntity>>();
        }
    }
}
using Dawn;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Prices;
using FooDash.Persistence.Common.Repositories;

namespace FooDash.Persistence.Repositories.Prices
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        private readonly FooDashDbContext _dbContext;
        public CurrencyRepository(FooDashDbContext dbContext) : base(dbContext)
        {
            _dbContext = Guard.Argument(dbContext).NotNull().Value;
        }

        public Currency? GetBase()
        {
            return _dbContext.Set<Currency>()
                .FirstOrDefault(x => x.IsBase);
        }
    }
}

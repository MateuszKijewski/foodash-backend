using FooDash.Domain.Entities.Prices;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        Currency? GetBase();
    }
}
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Metadata;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface IEntityRepository : IBaseRepository<Entity>
    {
        Entity GetByName(string name);

        IEnumerable<Entity> GetByNames(string[] names);
    }
}
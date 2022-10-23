using Dawn;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Metadata;
using FooDash.Persistence.Common.Repositories;

namespace FooDash.Persistence.Repositories.Metadata
{
    public class EntityRepository : BaseRepository<Entity>, IEntityRepository
    {
        private readonly FooDashDbContext _dbContext;

        public EntityRepository(FooDashDbContext dbContext) : base(dbContext)
        {
            _dbContext = Guard.Argument(dbContext, nameof(dbContext)).NotNull();
        }

        public Entity GetByName(string name)
        {
            return _dbContext.Entity.SingleOrDefault(x => x.Name == name);
        }

        public IEnumerable<Entity> GetByNames(string[] names)
        {
            return _dbContext.Entity.Where(x => names.Contains(x.Name));
        }
    }
}
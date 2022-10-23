using Dawn;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Identity;
using FooDash.Persistence.Common.Repositories;

namespace FooDash.Persistence.Repositories.Auth
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly FooDashDbContext _dbContext;

        public UserRepository(FooDashDbContext dbContext) : base(dbContext)
        {
            _dbContext = Guard.Argument(dbContext, nameof(dbContext)).NotNull();
        }

        public User GetByEmail(string email)
        {            
            return _dbContext.User.SingleOrDefault(x => x.Email == email); ;
        }
    }
}
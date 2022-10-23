using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetByEmail(string email);
    }
}
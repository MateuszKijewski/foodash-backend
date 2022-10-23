using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Common.Interfaces.Auth
{
    public interface ICurrentUserService
    {
        Guid Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string Name { get; }
        string Email { get; }

        Guid? RoleId { get; }
        Guid CurrencyId { get; }
        Guid LanguageId { get; }

        User User { get; }
    }
}
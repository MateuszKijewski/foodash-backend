using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Application.Translations.Dtos.Basic;

namespace FooDash.Application.Auth.Dtos.Responses
{
    public class GetCurrentUserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleBasicInfo Role { get; set; }
        public Guid LanguageId { get; set; }
        public ReadLanguageDto Language { get; set; }
        public Guid CurrencyId { get; set; }
        public ReadCurrencyDto Currency { get; set; }

        public IEnumerable<string?> Permissions { get; set; }
    }

    public class RoleBasicInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
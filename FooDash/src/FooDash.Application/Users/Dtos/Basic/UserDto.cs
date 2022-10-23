using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Users.Dtos.Basic
{
    public class UserDto : IDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public Guid LanguageId { get; set; }
        public Guid CurrencyId { get; set; }
    }
}
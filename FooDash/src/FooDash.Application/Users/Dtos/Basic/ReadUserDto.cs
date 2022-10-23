using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Users.Dtos.Basic
{
    public class ReadUserDto : ReadDtoBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? LanguageId { get; set; }
        public Guid? CurrencyId { get; set; }
    }
}
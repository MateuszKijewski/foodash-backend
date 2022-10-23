using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Translations;

namespace FooDash.Domain.Entities.Identity
{
    [SystemEntity]
    public class User : EntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public byte[]? Salt { get; set; }
        public string? HashedPassword { get; set; }
        public Guid? RoleId { get; set; }
        public Role Role { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public void ComputeName()
        {
            var isFullNameComputable = (FirstName != null && LastName != null);

            if (isFullNameComputable)
            {
                Name = $"{FirstName} {LastName}".Trim();
                return;
            }
            Name = Email;
        }
    }
}
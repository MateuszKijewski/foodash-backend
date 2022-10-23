using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Identity
{
    [SystemEntity]
    public class RefreshToken : EntityBase
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
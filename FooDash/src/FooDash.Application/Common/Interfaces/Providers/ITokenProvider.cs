using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Common.Interfaces.Providers
{    
    public interface ITokenProvider
    {
        Task<JwtToken> GetTokenAsync(User user);
    }

    public class SecurityToken
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class JwtToken
    {
        public SecurityToken AuthToken { get; set; }
        public SecurityToken RefreshToken { get; set; }
    }
}
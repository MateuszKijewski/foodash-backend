using FooDash.Application.Common.Interfaces.Providers;

namespace FooDash.Application.Auth.Dtos.Responses
{
    public class RefreshTokenResponse
    {
        public SecurityToken AuthToken { get; set; }
        public SecurityToken RefreshToken { get; set; }
    }
}
using FooDash.Application.Common.Interfaces.Providers;

namespace FooDash.Application.Auth.Dtos.Responses
{
    public class LoginUserResponse
    {
        public SecurityToken AuthToken { get; set; }
        public SecurityToken RefreshToken { get; set; }
    }
}
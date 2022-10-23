using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Application.Auth.Dtos.Responses;

namespace FooDash.Application.Common.Interfaces.Auth
{
    public interface IAuthorizationService
    {
        Task<LoginUserResponse> LoginUser(LoginUserContract loginUserContract);

        Task<RegisterUserResponse> RegisterUser(RegisterUserContract registerUserContract);

        Task<RefreshTokenResponse> RefreshToken(RefreshTokenContract refreshTokenContract);

        Task SetRole(SetRoleContract roleContract);

        Task<GetCurrentUserResponse> GetCurrentUser();
    }
}
using Dawn;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Application.Auth.Dtos.Responses;
using FooDash.WebApi.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly Application.Common.Interfaces.Auth.IAuthorizationService _authorizationService;
        public AuthController(Application.Common.Interfaces.Auth.IAuthorizationService authorizationService)
        {
            _authorizationService = Guard.Argument(authorizationService, nameof(authorizationService)).NotNull().Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserContract contract)
        {
            var jwtToken = await _authorizationService.LoginUser(contract);

            return Ok(jwtToken);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserContract contract)
        {
            var registeredUser = await _authorizationService.RegisterUser(contract);

            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenContract contract)
        {
            var refreshToken = await _authorizationService.RefreshToken(contract);

            return Ok(refreshToken);
        }

        [HttpGet]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser()
        {
            var currentUser = await _authorizationService.GetCurrentUser();

            return Ok(currentUser);
        }
    }
}
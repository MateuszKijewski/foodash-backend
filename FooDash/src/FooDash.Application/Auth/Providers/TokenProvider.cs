using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Common.Interfaces.Security;
using FooDash.Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FooDash.Application.Auth.Providers
{
    public class TokenValidationOptions
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? IssuerSigningKey { get; set; }

        public int ExpirationTimeOfAuthTokenInHours { get; set; }
        public int ExpirationTimeOfRefreshTokenInDays { get; set; }
    }

    public class TokenProvider : ITokenProvider
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IHashingService _hashingService;
        private readonly TokenValidationOptions _tokenValidationOptions;

        public TokenProvider(IRefreshTokenRepository refreshTokenRepository,
            IHashingService hashingService,
            IOptions<TokenValidationOptions> tokenValidationOptions,
            IRolePermissionRepository rolePermissionRepository)
        {
            _refreshTokenRepository = Guard.Argument(refreshTokenRepository, nameof(refreshTokenRepository)).NotNull().Value;
            _hashingService = Guard.Argument(hashingService, nameof(hashingService)).NotNull().Value;
            _tokenValidationOptions = Guard.Argument(tokenValidationOptions.Value, nameof(tokenValidationOptions.Value)).NotNull().Value;
            _rolePermissionRepository = Guard.Argument(rolePermissionRepository, nameof(rolePermissionRepository)).NotNull().Value;
        }

        public async Task<JwtToken> GetTokenAsync(User user)
        {
            var authToken = await CreateSecurityToken(user);
            var refreshToken = await CreateRefreshToken(user);

            return new JwtToken
            {
                AuthToken = new Common.Interfaces.Providers.SecurityToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(authToken),
                    ExpiryDate = authToken.ValidTo,
                },
                RefreshToken = new Common.Interfaces.Providers.SecurityToken
                {
                    Token = refreshToken.Token,
                    ExpiryDate = refreshToken.ExpiryDate
                }
            };
        }

        private async Task<JwtSecurityToken> CreateSecurityToken(User user)
        {
            IEnumerable<Claim> claims = await GetUserClaims(user);
            return CreateTokenBasedOnClaims(claims);
        }

        private JwtSecurityToken CreateTokenBasedOnClaims(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenValidationOptions.IssuerSigningKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _tokenValidationOptions.Issuer,
                audience: _tokenValidationOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_tokenValidationOptions.ExpirationTimeOfAuthTokenInHours),
                signingCredentials: credentials
            );
        }

        private async Task<IEnumerable<Claim>> GetUserClaims(User user)
        {
            var userRolePermissions = (await _rolePermissionRepository.GetPermissionsByRoleId(user.RoleId));

            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.Email));

            foreach (var userRolePermission in userRolePermissions)
                claims.Add(new Claim("PermissionKey", userRolePermission.Id.ToString()));

            return claims;
        }

        public async Task<RefreshToken> CreateRefreshToken(User user)
        {
            var token = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = _hashingService.Hash(Guid.NewGuid().ToString(), _hashingService.GetSalt()),
                ExpiryDate = DateTime.Now.AddDays(_tokenValidationOptions.ExpirationTimeOfRefreshTokenInDays),
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(token);
            return token;
        }
    }
}
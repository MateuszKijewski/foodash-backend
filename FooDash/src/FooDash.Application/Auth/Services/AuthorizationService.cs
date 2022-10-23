using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Common.Interfaces.Security;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Application.Auth.Dtos.Responses;
using FooDash.Domain.Common.Statics;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.System;
using Mapster;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Translations;
using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Application.Translations.Dtos.Basic;

namespace FooDash.Application.Auth.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationService(IUserRepository userRepository, IHashingService hashingService, ITokenProvider tokenProvider, IRefreshTokenRepository refreshTokenRepository, IBaseRepositoryProvider baseRepositoryProvider, ICurrentUserService currentUserService)
        {
            _userRepository = Guard.Argument(userRepository).NotNull().Value;
            _hashingService = Guard.Argument(hashingService).NotNull().Value;
            _tokenProvider = Guard.Argument(tokenProvider).NotNull().Value;
            _refreshTokenRepository = Guard.Argument(refreshTokenRepository).NotNull().Value;
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider).NotNull().Value;
            _currentUserService = Guard.Argument(currentUserService).NotNull().Value;
        }

        public async Task<GetCurrentUserResponse> GetCurrentUser()
        {
            var rolePermissionRepository = _baseRepositoryProvider.GetRepository<RolePermission>();
            var roleRepository = _baseRepositoryProvider.GetRepository<Role>();
            var currencyRepository = _baseRepositoryProvider.GetRepository<Currency>();
            var languageRepository = _baseRepositoryProvider.GetRepository<Language>();

            var roleId = _currentUserService.RoleId;
            var role = await roleRepository.GetAsync(roleId.Value);
            var permissions = (await rolePermissionRepository
                .FindAsync(x => x.RoleId == roleId, nameof(RolePermission.Permission)))
                .ToList();

            var userCurrency = (await currencyRepository.GetAsync(_currentUserService.CurrencyId)).Adapt<ReadCurrencyDto>();
            var userLanguage = (await languageRepository.GetAsync(_currentUserService.LanguageId)).Adapt<ReadLanguageDto>();

            var user = new GetCurrentUserResponse()
            {
                Id = _currentUserService.Id,
                Email = _currentUserService.Email,
                FirstName = _currentUserService.FirstName,
                LastName = _currentUserService.LastName,
                Name = _currentUserService.Name,
                LanguageId = userLanguage.Id,
                Language = userLanguage,
                CurrencyId = userCurrency.Id,
                Currency = userCurrency,
                Permissions = permissions.Select(x => x.Permission.Name),
                Role = new RoleBasicInfo
                {
                    Id = role.Id,
                    Name = role.Name
                }
            };

            return await Task.FromResult(user);
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserContract loginUserContract)
        {
            var user = _userRepository.GetByEmail(loginUserContract.Email);
            if (user is null)
                throw new BadRequestException("User for this email was not found or password was incorrect");

            string hashedPassword = _hashingService.Hash(loginUserContract.Password, user.Salt);
            if (hashedPassword != user.HashedPassword)
                throw new BadRequestException("User for this email was not found or password was incorrect");

            var jwtToken = await _tokenProvider.GetTokenAsync(user);
            return new LoginUserResponse
            {
                AuthToken = jwtToken.AuthToken,
                RefreshToken = jwtToken.RefreshToken
            };
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenContract refreshTokenContract)
        {
            var token = await _refreshTokenRepository.GetByTokenWithUser(refreshTokenContract.RefreshToken);
            if (token != null)
                await _refreshTokenRepository.RemoveAsync(token.Id);

            if (token is null || token.ExpiryDate < DateTime.Now)
                throw new BadRequestException("Invalid refresh token");

            var jwtToken = await _tokenProvider.GetTokenAsync(token.User);
            return new RefreshTokenResponse
            {
                AuthToken = jwtToken.AuthToken,
                RefreshToken = jwtToken.RefreshToken,
            };
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserContract registerUserContract)
        {
            if (_userRepository.GetByEmail(registerUserContract.Email) != null)
                throw new BadRequestException("User with this email already exists");

            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();
            var roleRepository = _baseRepositoryProvider.GetRepository<Role>();

            var defaultSettings = (await settingsRepository.GetAllAsync()).First();
            var employeeRole = (await roleRepository.FindAsync(x => x.Name == PredefinedRoles.Employee)).First();

            byte[] salt = _hashingService.GetSalt();
            string hashedPassword = _hashingService.Hash(registerUserContract.Password, salt);

            var user = new User
            {
                Email = registerUserContract.Email,
                FirstName = registerUserContract.FirstName,
                LastName = registerUserContract.LastName,
                HashedPassword = hashedPassword,
                Salt = salt,
                LanguageId = defaultSettings.DefaultLanguageId,
                CurrencyId = defaultSettings.DefaultCurrencyId,
                RoleId = employeeRole.Id
            };
            user.ComputeName();

            var addedUser = await _userRepository.AddAsync(user);

            return addedUser.Adapt<RegisterUserResponse>();
        }

        public async Task SetRole(SetRoleContract roleContract)
        {
            var roleRepository = _baseRepositoryProvider.GetRepository<Role>();
            var userRepository = _baseRepositoryProvider.GetRepository<User>();

            var superAdminRole = (await roleRepository.FindAsync(x => x.Name == PredefinedRoles.SuperAdmin)).Single();

            var user = await userRepository.GetAsync(roleContract.UserId);
            if (user.RoleId == superAdminRole.Id)
                throw new BadRequestException($"{PredefinedRoles.SuperAdmin} role cannot be changed");

            var role = await roleRepository.GetAsync(roleContract.RoleId);
            if (role.Id == superAdminRole.Id)
                throw new BadRequestException($"{PredefinedRoles.SuperAdmin} role cannot be assigned");

            user.RoleId = roleContract.RoleId;
            await userRepository.UpdateAsync(user);
        }
    }
}
using FluentAssertions;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Application.Auth.Services;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Common.Interfaces.Security;
using FooDash.Domain.Common.Statics;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.System;
using FooDash.Domain.Entities.Translations;
using FooDash.Tests.Common.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace FooDash.Application.Tests.Auth.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        private IBaseRepositoryProvider _repositoryProvider;
        private Mock<IUserRepository> _userRepository;
        private Mock<IHashingService> _hashingService;
        private Mock<ITokenProvider> _tokenProvider;
        private Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private Mock<ICurrentUserService> _currentUserService;

        [TestInitialize]
        public void TestInit()
        {
            _repositoryProvider = new MockedRepositoryProvider();
            _userRepository = new Mock<IUserRepository>();
            _hashingService = new Mock<IHashingService>();
            _tokenProvider = new Mock<ITokenProvider>();
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        [TestMethod]
        public async Task RegisterUser_ShouldAddNewUser()
        {
            _userRepository
                .Setup(x => x.GetByEmail(It.IsAny<string>()))
                .Verifiable();
            _userRepository
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Returns<User>((user) =>
                {
                    user.Id = Guid.NewGuid();
                    return Task.FromResult(user);
                });
            _hashingService
                .Setup(x => x.GetSalt())
                .Returns(Array.Empty<byte>())
                .Verifiable();
            _hashingService
                .Setup(x => x.Hash(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(string.Empty)
                .Verifiable();

            var languageRepositoryMock = _repositoryProvider.GetRepository<Language>();
            var currencyRepositoryMock = _repositoryProvider.GetRepository<Currency>();
            var roleRepositoryMock = _repositoryProvider.GetRepository<Role>();
            var settingsRepositoryMock = _repositoryProvider.GetRepository<Settings>();

            var languageMock = await languageRepositoryMock.AddAsync(new Language
            {
                Symbol = "pl-pl"
            });
            var currencyMock =await currencyRepositoryMock.AddAsync(new Currency
            {
                Name = "Zloty",
                IsBase = true
            });
            await roleRepositoryMock.AddAsync(new Role
            {
                Name = "Employee"
            });
            await settingsRepositoryMock.AddAsync(new Settings
            {
                Name = "SystemSettings",
                DefaultLanguageId = languageMock.Id,
                DefaultCurrencyId = currencyMock.Id
            });

            var authorizationService = new AuthorizationService(
                _userRepository.Object,
                _hashingService.Object,
                _tokenProvider.Object,
                _refreshTokenRepository.Object,
                _repositoryProvider,
                _currentUserService.Object);

            var createdUser = await authorizationService.RegisterUser(new RegisterUserContract
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "TestEmail",
                Password = "TestPassword"
            });
            var expectedName = "TestFirstName TestLastName";

            _userRepository.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
            _userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _hashingService.Verify(x => x.GetSalt(), Times.Once);
            _hashingService.Verify(x => x.Hash(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);

            createdUser.Should().NotBeNull();
            createdUser.Name.Should().Be(expectedName);
        }

        [TestMethod]
        public async Task RegisterUser_ShouldThrowExceptionBecauseUserExists()
        {
            _userRepository
                .Setup(x => x.GetByEmail(It.IsAny<string>()))
                .Returns(new User())
                .Verifiable();

            var authorizationService = new AuthorizationService(
                _userRepository.Object,
                _hashingService.Object,
                _tokenProvider.Object,
                _refreshTokenRepository.Object,
                _repositoryProvider,
                _currentUserService.Object);

            Func<Task> userRegistration = async () => await authorizationService.RegisterUser(new RegisterUserContract
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "TestEmail",
                Password = "TestPassword"
            });

            await userRegistration.Should().ThrowAsync<BadRequestException>();
            _userRepository.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task SetRole_ShouldThrowExceptionBecauseChangingSuperAdmin()
        {
            var userRepository = _repositoryProvider.GetRepository<User>();
            var roleRepository = _repositoryProvider.GetRepository<Role>();

            var superAdminRole = await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.SuperAdmin
            });
            var employeeRole = await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.Employee
            });
            var user = await userRepository.AddAsync(new User
            {
                RoleId = superAdminRole.Id,
                Role = superAdminRole
            });

            var authorizationService = new AuthorizationService(
                _userRepository.Object,
                _hashingService.Object,
                _tokenProvider.Object,
                _refreshTokenRepository.Object,
                _repositoryProvider,
                _currentUserService.Object);

            Func<Task> settingRole = async () => await authorizationService.SetRole(new SetRoleContract
            {
                RoleId = employeeRole.Id,
                UserId = user.Id
            });

            await settingRole.Should().ThrowAsync<BadRequestException>();
        }

        [TestMethod]
        public async Task SetRole_ShouldThrowExceptionBecauseSettingSuperAdmin()
        {
            var userRepository = _repositoryProvider.GetRepository<User>();
            var roleRepository = _repositoryProvider.GetRepository<Role>();

            var superAdminRole = await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.SuperAdmin
            });
            var employeeRole = await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.Employee
            });
            var user = await userRepository.AddAsync(new User
            {
                RoleId = employeeRole.Id,
                Role = employeeRole
            });

            var authorizationService = new AuthorizationService(
                _userRepository.Object,
                _hashingService.Object,
                _tokenProvider.Object,
                _refreshTokenRepository.Object,
                _repositoryProvider,
                _currentUserService.Object);

            Func<Task> settingRole = async () => await authorizationService.SetRole(new SetRoleContract
            {
                RoleId = superAdminRole.Id,
                UserId = user.Id
            });

            await settingRole.Should().ThrowAsync<BadRequestException>();
        }

        [TestMethod]
        public async Task SetRole_ShouldChangeRole()
        {
            var userRepository = _repositoryProvider.GetRepository<User>();
            var roleRepository = _repositoryProvider.GetRepository<Role>();

            await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.SuperAdmin
            });
            var employeeRole = await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.Employee
            });
            var managerRole = await roleRepository.AddAsync(new Role
            {
                Name = PredefinedRoles.Manager
            });
            var user = await userRepository.AddAsync(new User
            {
                RoleId = employeeRole.Id,
                Role = employeeRole
            });

            var authorizationService = new AuthorizationService(
                _userRepository.Object,
                _hashingService.Object,
                _tokenProvider.Object,
                _refreshTokenRepository.Object,
                _repositoryProvider,
                _currentUserService.Object);

            await authorizationService.SetRole(new SetRoleContract
            {
                RoleId = managerRole.Id,
                UserId = user.Id
            });

            user.RoleId.Should().Be(managerRole.Id);
        }
    }
}
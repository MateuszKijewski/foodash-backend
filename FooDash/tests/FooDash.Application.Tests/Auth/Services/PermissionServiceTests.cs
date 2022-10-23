using FluentAssertions;
using FooDash.Application.Auth.Services;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Domain.Entities.Identity;
using FooDash.Tests.Common.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FooDash.Application.Tests.Auth.Services
{
    [TestClass]
    public class PermissionServiceTests
    {
        private IBaseRepositoryProvider _repositoryProvider;

        [TestInitialize]
        public void TestInit()
        {
            _repositoryProvider = new MockedRepositoryProvider();
        }

        [TestMethod]
        public async Task SetPermissionsForRole_ThrowsExceptionForInvalidRole()
        {
            var roleRepositoryMock = _repositoryProvider.GetRepository<Role>();

            var superAdminRole = await roleRepositoryMock.AddAsync(new Role
            {
                Name = "SuperAdmin"
            });

            var permissionService = new PermissionService(_repositoryProvider);

            Func<Task> settingPermissionsForRole = async () => await permissionService.SetPermissionsForRole(new SetRolePermissionsContract
            {
                RoleId = superAdminRole.Id,
                PermissionIds = Array.Empty<Guid>()
            });

            await settingPermissionsForRole.Should().ThrowAsync<BadRequestException>();
        }

        [TestMethod]
        public async Task SetPermissionsForRole_SetsNewPermissionsForRole()
        {
            var roleRepositoryMock = _repositoryProvider.GetRepository<Role>();
            var rolePermissionRepositoryMock = _repositoryProvider.GetRepository<RolePermission>();
            var permissionRepositoryMock = _repositoryProvider.GetRepository<Permission>();

            var mockRole = await roleRepositoryMock.AddAsync(new Role());
            await rolePermissionRepositoryMock.AddRangeAsync(new List<RolePermission>
            {
                new RolePermission
                {
                    RoleId = mockRole.Id,
                    PermissionId = Guid.NewGuid()
                },
                new RolePermission
                {
                    RoleId = mockRole.Id,
                    PermissionId = Guid.NewGuid()
                }
            });

            var mockPermissions = await permissionRepositoryMock.AddRangeAsync(new List<Permission>
            {
                new Permission(),
                new Permission(),
                new Permission()
            });
            var mockPermissionsIds = mockPermissions.Select(x => x.Id).ToArray();

            var permissionService = new PermissionService(_repositoryProvider);

            await permissionService.SetPermissionsForRole(new SetRolePermissionsContract
            {
                RoleId = mockRole.Id,
                PermissionIds = mockPermissionsIds
            });

            (await rolePermissionRepositoryMock.GetAllAsync()).Count().Should().Be(3);
        }
    }
}
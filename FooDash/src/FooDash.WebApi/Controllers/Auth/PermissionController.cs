using Dawn;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Metadata;
using FooDash.WebApi.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FooDash.Application.Auth.Dtos.Responses;

namespace FooDash.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = Guard.Argument(permissionService).NotNull().Value;
        }

        [HttpGet]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Permission)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Entity)}")]
        public async Task<ActionResult<IEnumerable<GetAllPermissionsResponse>>> GetAllPermissions()
        {
            var allPermissions = await _permissionService.GetAllPermissions();

            return Ok(allPermissions);
        }

        [HttpGet]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Role)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(RolePermission)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Permission)}")]
        public async Task<ActionResult<IEnumerable<GetAllRolesResponse>>> GetAllRoles()
        {
            var allRoles = await _permissionService.GetAllRoles();

            return Ok(allRoles);
        }

        [HttpPut]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Role)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(RolePermission)}")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(RolePermission)}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(RolePermission)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Permission)}")]
        public async Task<ActionResult> SetRolePermissions([FromBody] SetRolePermissionsContract contract)
        {
            await _permissionService.SetPermissionsForRole(contract);

            return Ok();
        }
    }
}
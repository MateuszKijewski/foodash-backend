using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Auth.Dtos.Contracts;
using FooDash.Domain.Entities.Identity;
using FooDash.WebApi.Common.Enums;
using FooDash.WebApi.Common.Interfaces;
using FooDash.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FooDash.Application.Users.Dtos.Basic;

namespace FooDash.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase, IBaseCrudController<User, ReadUserDto, UserDto>
    {
        private readonly IBasicOperationsService<User, ReadUserDto, UserDto> _basicOperationsService;
        private readonly Application.Common.Interfaces.Auth.IAuthorizationService _authorizationService;

        public UserController(IBasicOperationsService<User, ReadUserDto, UserDto> basicOperationsService, Application.Common.Interfaces.Auth.IAuthorizationService authorizationService)
        {
            _basicOperationsService = Guard.Argument(basicOperationsService).NotNull().Value;
            _authorizationService = Guard.Argument(authorizationService).NotNull().Value;
        }

        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Role)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Domain.Entities.Identity.User)}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult> SetRole([FromBody] SetRoleContract contract)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }
            await _authorizationService.SetRole(contract);

            return Ok();
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(Domain.Entities.Identity.User)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult<IEnumerable<ReadUserDto>>> AddRange([FromBody] IEnumerable<UserDto> dtos)
        {
            var result = await _basicOperationsService.AddRange(dtos);
            var resultIds = result.Select(x => x.Id).ToString();

            return Created(resultIds, result);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult<ReadUserDto>> Get([FromRoute] Guid id)
        {
            var result = await _basicOperationsService.Get(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult<IEnumerable<ReadUserDto>>> GetAll()
        {
            var result = await _basicOperationsService.GetAll();

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id)
        {
            await _basicOperationsService.Remove(id);

            return Ok();
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Domain.Entities.Identity.User)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Domain.Entities.Identity.User)}")]
        public async Task<ActionResult<ReadUserDto>> Update([FromBody] UserDto dto, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }
            var result = await _basicOperationsService.Update(dto, id);

            return Ok(result);
        }
    }
}

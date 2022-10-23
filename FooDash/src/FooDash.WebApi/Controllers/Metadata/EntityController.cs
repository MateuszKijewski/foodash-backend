using Dawn;
using FooDash.Application.Common.Interfaces.Metadata;
using FooDash.Application.Metadata.Dtos.Contracts;
using FooDash.Application.Metadata.Dtos.Responses;
using FooDash.Domain.Entities.Metadata;
using FooDash.WebApi.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Metadata
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = Guard.Argument(entityService).NotNull().Value;
        }

        [HttpPut]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Entity)}")]
        public async Task<ActionResult> SetEntityTranslatability([FromBody] SetEntityTranslatabilityContract contract)
        {
            await _entityService.SetEntityTranslatability(contract);

            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Entity)}")]
        public ActionResult<IEnumerable<GetEntitiesResponse>> GetEntities([FromQuery] GetEntitiesContract query)
        {
            var requestedEntities = _entityService.GetEntities(query.requestedEntitiesType);

            return Ok(requestedEntities);
        }
    }
}
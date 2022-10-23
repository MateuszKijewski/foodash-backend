using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Translations.Dtos.Basic;
using FooDash.Domain.Entities.Translations;
using FooDash.WebApi.Common.Enums;
using FooDash.WebApi.Common.Interfaces;
using FooDash.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Translations
{
    public class LanguageController : ControllerBase, IBaseCrudController<Language, ReadLanguageDto, LanguageDto>
    {
        private readonly IBasicOperationsService<Language, ReadLanguageDto, LanguageDto> _basicOperationsService;

        public LanguageController(IBasicOperationsService<Language, ReadLanguageDto, LanguageDto> basicOperationsService)
        {
            _basicOperationsService = Guard.Argument(basicOperationsService, nameof(basicOperationsService)).NotNull().Value;
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(Language)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Language)}")]
        public async Task<ActionResult<IEnumerable<ReadLanguageDto>>> AddRange([FromBody] IEnumerable<LanguageDto> dtos)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }
            var result = await _basicOperationsService.AddRange(dtos);
            var resultIds = result.Select(x => x.Id).ToString();

            return Created(resultIds, result);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult<ReadLanguageDto>> Get([FromRoute] Guid id)
        {
            var result = await _basicOperationsService.Get(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<ReadLanguageDto>>> GetAll()
        {
            var result = await _basicOperationsService.GetAll();

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(Language)}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id)
        {
            await _basicOperationsService.Remove(id);

            return Ok();
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Language)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Language)}")]
        public async Task<ActionResult<ReadLanguageDto>> Update([FromBody] LanguageDto dto, [FromRoute] Guid id)
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

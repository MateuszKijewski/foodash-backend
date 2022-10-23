using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Translations.Dtos.Basic;
using FooDash.Application.Translations.Dtos.Contracts;
using FooDash.Domain.Entities.Translations;
using FooDash.WebApi.Common.Enums;
using FooDash.WebApi.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Translations
{
    [Authorize]
    [ApiController]
    public class LabelController : ControllerBase, IBaseCrudController<Label, ReadLabelDto, LabelDto>
    {
        private readonly IBasicOperationsService<Label, ReadLabelDto, LabelDto> _basicOperationsService;
        private readonly ILabelService _labelService;

        public LabelController(IBasicOperationsService<Label, ReadLabelDto, LabelDto> basicOperationsService, ILabelService labelService)
        {
            _basicOperationsService = Guard.Argument(basicOperationsService).NotNull().Value;
            _labelService = labelService;
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(Label)}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Label)}")]
        public async Task<ActionResult> SetLabels([FromBody] SetLabelsContract contract)
        {
            await _labelService.SetLabels(contract);

            return Ok();
        }

        [HttpPost]
        [Route("api/[controller]")]
        public Task<ActionResult<IEnumerable<ReadLabelDto>>> AddRange([FromBody] IEnumerable<LabelDto> dtos)
        {
            throw new BadRequestException($"Labels should be set with {nameof(SetLabelsContract)}");
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Label)}")]
        public async Task<ActionResult<ReadLabelDto>> Get([FromRoute] Guid id)
        {
            var result = await _basicOperationsService.Get(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Label)}")]
        public async Task<ActionResult<IEnumerable<ReadLabelDto>>> GetAll()
        {
            var result = await _basicOperationsService.GetAll();

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(Label)}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id)
        {
            await _basicOperationsService.Remove(id);

            return Ok();
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Label)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Label)}")]
        public async Task<ActionResult<ReadLabelDto>> Update([FromBody] LabelDto dto, [FromRoute] Guid id)
        {
            throw new BadRequestException($"Labels should be set with {nameof(SetLabelsContract)}");
        }
    }
}

using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Common.Interfaces.Prices;
using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Domain.Entities.Prices;
using FooDash.WebApi.Common.Enums;
using FooDash.WebApi.Common.Interfaces;
using FooDash.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Prices
{
    public class CurrencyController : ControllerBase, IBaseCrudController<Currency, ReadCurrencyDto, CurrencyDto>
    {
        private readonly IBasicOperationsService<Currency, ReadCurrencyDto, CurrencyDto> _basicOperationsService;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(IBasicOperationsService<Currency, ReadCurrencyDto, CurrencyDto> basicOperationsService, ICurrencyService currencyService)
        {
            _basicOperationsService = Guard.Argument(basicOperationsService).NotNull().Value;
            _currencyService = Guard.Argument(currencyService).NotNull().Value;
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(Currency)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Currency)}")]
        public async Task<ActionResult<IEnumerable<ReadCurrencyDto>>> AddRange([FromBody] IEnumerable<CurrencyDto> dtos)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }

            await _currencyService.ValidateIfOnlyOneCurrencyIsActive(dtos.ToList());

            var result = await _basicOperationsService.AddRange(dtos);
            var resultIds = result.Select(x => x.Id).ToString();

            return Created(resultIds, result);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult<ReadCurrencyDto>> Get([FromRoute] Guid id)
        {
            var result = await _basicOperationsService.Get(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<ReadCurrencyDto>>> GetAll()
        {
            var result = await _basicOperationsService.GetAll();

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(Currency)}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id)
        {
            await _basicOperationsService.Remove(id);

            return Ok();
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Currency)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Currency)}")]
        public async Task<ActionResult<ReadCurrencyDto>> Update([FromBody] CurrencyDto dto, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }

            await _currencyService.ValidateIfOnlyOneCurrencyIsActive(new List<CurrencyDto> { dto });

            var result = await _basicOperationsService.Update(dto, id);

            return Ok(result);
        }
    }
}

using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Products.Dtos.Basic;
using FooDash.Domain.Entities.Products;
using FooDash.WebApi.Common.Enums;
using FooDash.WebApi.Common.Interfaces;
using FooDash.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Products
{
    public class ProductCategoryController : ControllerBase, IBaseCrudController<ProductCategory, ReadProductCategoryDto, ProductCategoryDto>
    {
        private readonly IBasicOperationsService<ProductCategory, ReadProductCategoryDto, ProductCategoryDto> _basicOperationsService;

        public ProductCategoryController(IBasicOperationsService<ProductCategory, ReadProductCategoryDto, ProductCategoryDto> basicOperationsService)
        {
            _basicOperationsService = Guard.Argument(basicOperationsService, nameof(basicOperationsService)).NotNull().Value;
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(ProductCategory)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(ProductCategory)}")]
        public async Task<ActionResult<IEnumerable<ReadProductCategoryDto>>> AddRange([FromBody] IEnumerable<ProductCategoryDto> dtos)
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
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(ProductCategory)}")]
        public async Task<ActionResult<ReadProductCategoryDto>> Get([FromRoute] Guid id)
        {
            var result = await _basicOperationsService.Get(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(ProductCategory)}")]
        public async Task<ActionResult<IEnumerable<ReadProductCategoryDto>>> GetAll()
        {
            var result = await _basicOperationsService.GetAll();

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(ProductCategory)}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id)
        {
            await _basicOperationsService.Remove(id);

            return Ok();
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(ProductCategory)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(ProductCategory)}")]
        public async Task<ActionResult<ReadProductCategoryDto>> Update([FromBody] ProductCategoryDto dto, [FromRoute] Guid id)
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

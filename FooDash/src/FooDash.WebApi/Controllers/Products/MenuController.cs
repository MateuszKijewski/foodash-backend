using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Common.Interfaces.Products;
using FooDash.Application.Products.Dtos.Basic;
using FooDash.Application.Products.Dtos.Contracts;
using FooDash.Domain.Entities.Products;
using FooDash.WebApi.Common.Enums;
using FooDash.WebApi.Common.Interfaces;
using FooDash.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Products
{
    public class MenuController : ControllerBase, IBaseCrudController<Menu, ReadMenuDto, MenuDto>
    {
        private readonly IBasicOperationsService<Menu, ReadMenuDto, MenuDto> _basicOperationsService;
        private readonly IMenuService _menuService;

        public MenuController(IBasicOperationsService<Menu, ReadMenuDto, MenuDto> basicOperationsService, IMenuService menuService)
        {
            _basicOperationsService = Guard.Argument(basicOperationsService).NotNull().Value;
            _menuService = Guard.Argument(menuService).NotNull().Value;
        }

        [HttpGet]
        [Route("api/[controller]/GetActiveWithProducts")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Menu)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(MenuCategory)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(MenuCategoryProduct)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Product)}")]
        public async Task<ActionResult<ReadMenuDto>> GetActiveWithProducts()
        {
            var result = await _menuService.GetActiveMenuWithUserCurrencyProducts();

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]/GetActiveWithProductsForGuest")]
        public async Task<ActionResult<ReadMenuDto>> GetActiveWithProductsForGuest(GetActiveWithProductsForGuestContract getActiveWithProductsForGuestContract)
        {
            var result = await _menuService.GetActiveMenuWithUserCurrencyProducts(getActiveWithProductsForGuestContract.GuestCurrencyId, getActiveWithProductsForGuestContract.GuestLanguageId);

            return Ok(result);
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanCreate}{nameof(Menu)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Menu)}")]
        public async Task<ActionResult<IEnumerable<ReadMenuDto>>> AddRange([FromBody] IEnumerable<MenuDto> dtos)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }

            await _menuService.ValidateIfOnlyOneMenuIsActive(dtos.ToList());

            var result = await _basicOperationsService.AddRange(dtos);
            var resultIds = result.Select(x => x.Id).ToString();

            return Created(resultIds, result);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Menu)}")]
        public async Task<ActionResult<ReadMenuDto>> Get([FromRoute] Guid id)
        {
            var result = await _basicOperationsService.Get(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Menu)}")]
        public async Task<ActionResult<IEnumerable<ReadMenuDto>>> GetAll()
        {
            var result = await _basicOperationsService.GetAll();

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanDelete}{nameof(Menu)}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id)
        {
            await _basicOperationsService.Remove(id);

            return Ok();
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Menu)}")]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Menu)}")]
        public async Task<ActionResult<ReadMenuDto>> Update([FromBody] MenuDto dto, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ControllerHelper.GetErrorsFromModelState(ModelState));
            }

            await _menuService.ValidateIfOnlyOneMenuIsActive(new List<MenuDto> { dto });

            var result = await _basicOperationsService.Update(dto, id);

            return Ok(result);
        }
    }
}

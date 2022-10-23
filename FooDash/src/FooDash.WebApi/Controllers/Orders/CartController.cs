using Dawn;
using FooDash.Application.Common.Interfaces.Orders;
using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Dtos.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Orders
{
    [ApiController]    
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = Guard.Argument(cartService).NotNull().Value;
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<ReadCartDto>> GetCart([FromBody] GetCartContract getCartContract)
        {
            var cart = await _cartService.GetCart(getCartContract);

            return Ok(cart);
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<Guid>> AddProductToCart([FromBody] AddProductToCartContract addProductToCartContract)
        {
            var cartId = await _cartService.AddProductToCart(addProductToCartContract);

            return Ok(cartId);
        }

        [HttpDelete]
        [Route("api/[controller]/[action]/{cartItemId}")]
        public async Task<ActionResult> RemoveFromCart([FromRoute] Guid cartItemId)
        {
            await _cartService.RemoveFromCart(cartItemId);

            return Ok();
        }
    }
}
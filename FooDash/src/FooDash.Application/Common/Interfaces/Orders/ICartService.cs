using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Dtos.Contracts;

namespace FooDash.Application.Common.Interfaces.Orders
{
    public interface ICartService
    {
        Task<ReadCartDto> GetCart(GetCartContract getCartContract);

        Task<Guid> AddProductToCart(AddProductToCartContract addProductToCartContract);

        Task RemoveFromCart(Guid cartItemId);
    }
}
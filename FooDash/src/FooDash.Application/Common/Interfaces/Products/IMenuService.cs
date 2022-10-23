using FooDash.Application.Products.Dtos.Basic;

namespace FooDash.Application.Common.Interfaces.Products
{
    public interface IMenuService
    {
        Task<ReadMenuDto> GetActiveMenuWithUserCurrencyProducts();

        Task<ReadMenuDto> GetActiveMenuWithUserCurrencyProducts(Guid guestCurrencyId, Guid guestLanguageId);

        Task ValidateIfOnlyOneMenuIsActive(List<MenuDto> menuDtos);
    }
}
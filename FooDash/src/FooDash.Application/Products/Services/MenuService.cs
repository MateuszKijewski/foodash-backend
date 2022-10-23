using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Products;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Products.Dtos.Basic;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Products;
using FooDash.Domain.Services.Prices;
using Mapster;

namespace FooDash.Application.Products.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrencyExchangeService _currencyExchangeService;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;
        private readonly IEntityRepository _entityRepository;
        private readonly ITranslationService _translationService;
        

        public MenuService(IMenuRepository menuRepository, ICurrentUserService currentUserService, ICurrencyExchangeService currencyExchangeService, IBaseRepositoryProvider baseRepositoryProvider, ICurrencyRepository currencyRepository, IEntityRepository entityRepository, ITranslationService translationService)
        {
            _menuRepository = Guard.Argument(menuRepository).NotNull().Value;
            _currencyRepository = Guard.Argument(currencyRepository).NotNull().Value;
            _currentUserService = Guard.Argument(currentUserService).NotNull().Value;
            _currencyExchangeService = Guard.Argument(currencyExchangeService).NotNull().Value;
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider).NotNull().Value;
            _entityRepository = Guard.Argument(entityRepository).NotNull().Value;
            _translationService = Guard.Argument(translationService).NotNull().Value;
        }

        public async Task ValidateIfOnlyOneMenuIsActive(List<MenuDto> menuDtos)
        {
            var addedActiveMenusCount = menuDtos.Where(x => x.IsActive).Count();
            if (addedActiveMenusCount == 0)
                return;
            if (addedActiveMenusCount > 1)
                throw new BadRequestException("Only one menu can be active at a time");

            var activeMenuExists = (await _menuRepository.FindAsync(x => x.IsActive)).Any();
            if (activeMenuExists)
                throw new BadRequestException("Only one menu can be active at a time");
        }

        public async Task<ReadMenuDto> GetActiveMenuWithUserCurrencyProducts()
        {

            var activeMenu = await _menuRepository.GetActiveMenuWithCategoriesAndProducts();
            if (activeMenu == null)
                throw new BadRequestException("There's no active menu");

            var baseCurrency = _currencyRepository.GetBase();
            if (baseCurrency == null)
                throw new BadRequestException("There's no base currency");

            var userCurrency = await GetUserCurrency();
            if (userCurrency != baseCurrency)
                ExchangeProductPricesByCurrency(activeMenu, userCurrency);

            var menuDto = await ConvertMenuEntitiesToTranslatedDtos(activeMenu, _currentUserService.LanguageId);

            return menuDto;
        }

        public async Task<ReadMenuDto> GetActiveMenuWithUserCurrencyProducts(Guid guestCurrencyId, Guid guestLanguageId)
        {
            var activeMenu = await _menuRepository.GetActiveMenuWithCategoriesAndProducts();
            if (activeMenu == null)
                throw new BadRequestException("There's no active menu");

            var baseCurrency = _currencyRepository.GetBase();
            if (baseCurrency == null)
                throw new BadRequestException("There's no base currency");

            var userCurrency = await _currencyRepository.GetAsync(guestCurrencyId);
            if (userCurrency != baseCurrency)
                ExchangeProductPricesByCurrency(activeMenu, userCurrency);

            var menuDto = await ConvertMenuEntitiesToTranslatedDtos(activeMenu, guestLanguageId);

            return menuDto;
        }

        private async Task<Currency> GetUserCurrency()
        {
            var currencyRepository = _baseRepositoryProvider.GetRepository<Currency>();
            var userCurrency = await currencyRepository.GetAsync(_currentUserService.CurrencyId);

            return userCurrency;
        }

        private void ExchangeProductPricesByCurrency(Menu activeMenu, Currency userCurrency)
        {
            var menuProducts = activeMenu.MenuCategories
                .SelectMany(x => x.MenuCategoryProducts
                .Select(y => y.Product));

            foreach (var product in menuProducts)
                _currencyExchangeService.CalculatePricesAfterExchange(product, userCurrency);
        }

        private async Task<ReadMenuDto> ConvertMenuEntitiesToTranslatedDtos(Menu menu, Guid userLanguageId)
        {
            var isMenuTranslated = _entityRepository.GetByName(nameof(Menu)).IsTranslated;
            var isMenuCategoryTranslated = _entityRepository.GetByName(nameof(MenuCategory)).IsTranslated;
            var isProductTranslated = _entityRepository.GetByName(nameof(Product)).IsTranslated;

            var menuDto = isMenuTranslated
                ? await _translationService.Translate(menu.Adapt<ReadMenuDto>(), userLanguageId)
                : menu.Adapt<ReadMenuDto>();

            menuDto.Categories = menu.MenuCategories.Select(menuCategory =>
            {
                var readMenuCategoryDto = isMenuCategoryTranslated
                    ? _translationService.Translate(menuCategory.Adapt<ReadMenuCategoryDto>(), userLanguageId).GetAwaiter().GetResult()
                    : menuCategory.Adapt<ReadMenuCategoryDto>();
                readMenuCategoryDto.Products = menuCategory.MenuCategoryProducts.Select(menuCategoryProduct => 
                {
                    var productDto = isProductTranslated
                        ? _translationService.Translate(menuCategoryProduct.Product.Adapt<ReadProductDto>(), userLanguageId).GetAwaiter().GetResult()
                        : menuCategoryProduct.Product.Adapt<ReadProductDto>();

                    return productDto;
                });

                return readMenuCategoryDto;
            });

            return menuDto;
        }
    }
}

using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Orders;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Dtos.Contracts;
using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Domain.Entities.Orders;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Products;
using FooDash.Domain.Entities.System;
using FooDash.Domain.Entities.Translations;
using FooDash.Domain.Services.Prices;
using Mapster;

namespace FooDash.Application.Orders.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;
        private readonly ITranslationService _translationService;
        private readonly ICurrencyExchangeService _currencyExchangeService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEntityRepository _entityRepository;

        public CartService(IBaseRepositoryProvider baseRepositoryProvider, ITranslationService translationService, ICurrencyExchangeService currencyExchangeService, ICurrentUserService currentUserService, IEntityRepository entityRepository)
        {
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider).NotNull().Value;
            _translationService = Guard.Argument(translationService).NotNull().Value;
            _currencyExchangeService = Guard.Argument(currencyExchangeService).NotNull().Value;
            _currentUserService = Guard.Argument(currentUserService).NotNull().Value;
            _entityRepository = Guard.Argument(entityRepository).NotNull().Value;
        }

        public async Task<ReadCartDto> GetCart(GetCartContract getCartContract)
        {
            var cartRepository = _baseRepositoryProvider.GetRepository<Cart>();
            var languageRepository = _baseRepositoryProvider.GetRepository<Language>();
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();

            var systemSettings = (await settingsRepository.GetAllAsync()).First();

            var user = _currentUserService.User;

            if (user == null && !getCartContract.CartId.HasValue)
                throw new BadRequestException("If user is not logged in, CartId has to be provided");

            var languageId = user?.LanguageId;
            if (!languageId.HasValue)
            {
                languageId = getCartContract.UserLanguageId.HasValue
                    ? getCartContract.UserLanguageId.Value
                    : systemSettings.DefaultLanguageId;
            }

            var cart = user != null
                ? (await cartRepository.FindAsync(x => x.UserId == user.Id, nameof(Cart.Currency), nameof(Cart.CartItems))).FirstOrDefault()
                : (await cartRepository.FindAsync(x => x.Id == getCartContract.CartId, nameof(Cart.Currency), nameof(Cart.CartItems))).FirstOrDefault();

            if (cart == null)
                throw new BadRequestException($"Cart for logged in user or for provided CartId doesn't exist");

            var translatedCart = await ConvertCartEntitiesToTranslatedDtos(cart, languageId.Value);

            return translatedCart;
        }

        public async Task<Guid> AddProductToCart(AddProductToCartContract addProductToCartContract)
        {
            var productRepository = _baseRepositoryProvider.GetRepository<Product>();
            var cartItemRepository = _baseRepositoryProvider.GetRepository<CartItem>();
            var currencyRepository = _baseRepositoryProvider.GetRepository<Currency>();
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();
            var cartRepository = _baseRepositoryProvider.GetRepository<Cart>();

            var systemSettings = (await settingsRepository.GetAllAsync()).First();

            var product = await productRepository.GetAsync(addProductToCartContract.ProductId);
            var user = _currentUserService.User;

            var currencyId = user?.CurrencyId;
            if (!currencyId.HasValue)
            {
                currencyId = addProductToCartContract.UserCurrencyId.HasValue
                    ? addProductToCartContract.UserCurrencyId.Value
                    : systemSettings.DefaultCurrencyId;
            }

            var userCurrency = await currencyRepository.GetAsync(currencyId.Value);

            var cartItem = new CartItem
            {
                Name = product.Name,
                CurrencyId = userCurrency.Id,
                Price = product.Price
            };
            _currencyExchangeService.CalculatePricesAfterExchange(cartItem, userCurrency);

            Cart cart;
            if (user != null)
            {
                var loggedUserCartId = (await cartRepository.FindAsync(x => x.UserId == user.Id)).FirstOrDefault()?.Id;
                if (loggedUserCartId != null)
                    cart = await AssignCartItemToCart(cartItem, loggedUserCartId.Value);
                else
                    cart = await CreateCartForCartItem(cartItem, user.Id);
            }
            else if (addProductToCartContract.CartId.HasValue)
            {
                cart = await AssignCartItemToCart(cartItem, addProductToCartContract.CartId.Value);
            }
            else
            {
                cart = await CreateCartForCartItem(cartItem);
            }

            await cartItemRepository.AddAsync(cartItem);

            return cart.Id;
        }

        public async Task RemoveFromCart(Guid cartItemId)
        {
            var cartItemRepository = _baseRepositoryProvider.GetRepository<CartItem>();
            var cartRepository = _baseRepositoryProvider.GetRepository<Cart>();

            var cartItem = await cartItemRepository.GetAsync(cartItemId);
            await cartItemRepository.RemoveAsync(cartItem);
            
            var cart = (await cartRepository.FindAsync(x => x.Id == cartItem.CartId, nameof(Cart.CartItems))).First();
            
            if (cart.CartItems.Any())
            {
                cart.Value = cart.CartItems.Sum(x => x.Price);

                await cartRepository.UpdateAsync(cart);
            }
            else
            {
                await cartRepository.RemoveAsync(cart);
            }
        }

        private async Task<ReadCartDto> ConvertCartEntitiesToTranslatedDtos(Cart cart, Guid userLanguageId)
        {
            var isCartTranslated = _entityRepository.GetByName(nameof(Cart)).IsTranslated;
            var isCartItemTranslated = _entityRepository.GetByName(nameof(CartItem)).IsTranslated;
            var isCurrencyTranslated = _entityRepository.GetByName(nameof(Currency)).IsTranslated;

            var cartDto = isCartTranslated
                ? await _translationService.Translate(cart.Adapt<ReadCartDto>(), userLanguageId)
                : cart.Adapt<ReadCartDto>();

            cartDto.CartItems = cart.CartItems.Select(cartItem =>
            {
                var cartItemDto = isCartItemTranslated
                    ? _translationService.Translate(cartItem.Adapt<ReadCartItemDto>(), userLanguageId).GetAwaiter().GetResult()
                    : cartItem.Adapt<ReadCartItemDto>();

                return cartItemDto;
            });

            cartDto.Currency = isCurrencyTranslated
                ? _translationService.Translate(cartDto.Currency.Adapt<ReadCurrencyDto>(), userLanguageId).GetAwaiter().GetResult()
                : cartDto.Currency.Adapt<ReadCurrencyDto>();

            return cartDto;
        }

        private async Task<Cart> AssignCartItemToCart(CartItem cartItem, Guid cartId)
        {
            var cartRepository = _baseRepositoryProvider.GetRepository<Cart>();

            var cart = (await cartRepository.FindAsync(x => x.Id == cartId, nameof(Cart.Currency))).FirstOrDefault();
            if (cart == null)
            {
                throw new BadRequestException($"Cart with Id: {cartId} doesn't exist");
            }

            if (cart.CurrencyId != cartItem.CurrencyId)
            {
                _currencyExchangeService.CalculatePricesAfterExchange(cartItem, cart.Currency);
                cartItem.Currency = cart.Currency;
            }
            cartItem.CartId = cartId;
            cart.Value += cartItem.Price;

            await cartRepository.UpdateAsync(cart);

            return cart;
        }

        private async Task<Cart> CreateCartForCartItem(CartItem cartItem, Guid? userId = null)
        {
            var cartRepository = _baseRepositoryProvider.GetRepository<Cart>();

            var cart = await cartRepository.AddAsync(new Cart
            {
                Value = cartItem.Price,
                CurrencyId = cartItem.CurrencyId,
                UserId = userId
            });

            cartItem.CartId = cart.Id;

            return cart;
        }
    }
}
using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Application.Common.Interfaces.Orders;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Hubs;
using FooDash.Domain.Entities.Orders;
using Microsoft.AspNetCore.SignalR;
using Mapster;
using FooDash.Application.Orders.Dtos.Contracts;
using FooDash.Domain.Entities.System;
using FooDash.Domain.Services.Prices;
using FooDash.Application.Users.Dtos.Basic;
using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;
        private readonly ITranslationService _translationService;
        private readonly ICurrencyExchangeService _currencyExchangeService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<OrderHub> _orderHub;

        public OrderService(IBaseRepositoryProvider baseRepositoryProvider, ITranslationService translationService, IHubContext<OrderHub> orderHub, ICurrentUserService currentUserService, ICurrencyExchangeService currencyExchangeService)
        {
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider).NotNull().Value;
            _translationService = Guard.Argument(translationService).NotNull().Value;
            _orderHub = Guard.Argument(orderHub).NotNull().Value;
            _currentUserService = Guard.Argument(currentUserService).NotNull().Value;
            _currencyExchangeService = Guard.Argument(currencyExchangeService).NotNull().Value;
        }


        public async Task AssignOrderToCourier(Guid orderId, Guid courierId)
        {
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();
            var userRepository = _baseRepositoryProvider.GetRepository<User>();

            var order = await orderRepository.GetAsync(orderId);
            var courier = await userRepository.GetAsync(courierId);

            if (courier == null)
                throw new BadRequestException($"Courier with Id: {courierId} doesn't exist");

            if (order == null)
                throw new BadRequestException($"Order with Id: {orderId} doesn't exist");

            if (order.Status == OrderStatus.Canceled || order.Status == OrderStatus.Delivered)
                throw new BadRequestException("You can't assign cancelled or delivered order");

            order.CourierId = courierId;
            order.Status = OrderStatus.Pickup;

            await orderRepository.UpdateAsync(order);
            await SendOrderUpdateToClients(order);
        }

        public async Task CancelOrder(Guid orderId)
        {
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();

            var order = await orderRepository.GetAsync(orderId);

            ChangeOrderStatus(order, OrderStatus.Canceled);

            await orderRepository.UpdateAsync(order);
            await SendOrderUpdateToClients(order);
        }

        public async Task ChangeOrdersStatus(Guid orderId, OrderStatus orderStatus)
        {
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();

            var order = await orderRepository.GetAsync(orderId);

            ChangeOrderStatus(order, orderStatus);

            await orderRepository.UpdateAsync(order);
            await SendOrderUpdateToClients(order);
        }

        public async Task<Guid> CreateOrderFromCart(CreateOrderContract createOrderContract)
        {
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();
            var orderItemRepository = _baseRepositoryProvider.GetRepository<OrderItem>();
            var cartRepository = _baseRepositoryProvider.GetRepository<Cart>();

            var cart = (await cartRepository.FindAsync(x => x.Id == createOrderContract.CartId, nameof(Cart.CartItems), nameof(Cart.Currency))).FirstOrDefault();

            if (cart == null)
                throw new BadRequestException($"Cart with Id: {createOrderContract.CartId} doesn't exist.");

            var order = createOrderContract.Adapt<Order>();
            var orderItems = await FillOrderWithCartData(order, cart);

            var orderId = (await orderRepository.AddAsync(order)).Id;
            foreach (var orderItem in orderItems)
                orderItem.OrderId = orderId;

            await orderItemRepository.AddRangeAsync(orderItems);

            return order.Id;
        }

        public async Task<IEnumerable<ReadOrderDto>> GetAllOrders()
        {
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();

            var allOrders = await orderRepository.FindAsync(
                _ => true,
                nameof(Order.OrderingUser),
                nameof(Order.Courier),
                nameof(Order.Currency),
                nameof(Order.OrderItems));

            List<ReadOrderDto> readOrderDtos = new();
            foreach (var order in allOrders)
            {
                var readOrderDto = order.Adapt<ReadOrderDto>();

                readOrderDto.OrderingUser = order.OrderingUser?.Adapt<ReadUserDto>();
                readOrderDto.Courier = order.Courier?.Adapt<ReadUserDto>();
                readOrderDto.Currency = order.Currency.Adapt<ReadCurrencyDto>();
                readOrderDto.OrderItems = order.OrderItems.Select(x => x.Adapt<ReadOrderItemDto>());

                await _translationService.Translate(readOrderDto, _currentUserService.LanguageId);
                if (readOrderDto.OrderingUser != null)
                    await _translationService.Translate(readOrderDto.OrderingUser, _currentUserService.LanguageId);
                if (readOrderDto.Courier != null)
                    await _translationService.Translate(readOrderDto.Courier, _currentUserService.LanguageId);
                await _translationService.Translate(readOrderDto.Currency, _currentUserService.LanguageId);

                foreach (var orderItem in readOrderDto.OrderItems)
                {
                    await _translationService.Translate(orderItem, _currentUserService.LanguageId);
                }

                readOrderDtos.Add(readOrderDto);
            }

            return readOrderDtos;
        }

        public async Task<IEnumerable<ReadOrderDto>> GetCourierOrders(Guid courierId)
        {
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();

            var courierOrders = await orderRepository.FindAsync(
                x => x.CourierId == courierId,
                nameof(Order.OrderingUser),
                nameof(Order.Courier),
                nameof(Order.Currency),
                nameof(Order.OrderItems));

            List<ReadOrderDto> readOrderDtos = new();
            foreach (var order in courierOrders)
            {
                var readOrderDto = order.Adapt<ReadOrderDto>();

                readOrderDto.OrderingUser = order.OrderingUser?.Adapt<ReadUserDto>();
                readOrderDto.Courier = order.Courier?.Adapt<ReadUserDto>();
                readOrderDto.Currency = order.Currency.Adapt<ReadCurrencyDto>();
                readOrderDto.OrderItems = order.OrderItems.Select(x => x.Adapt<ReadOrderItemDto>());

                await _translationService.Translate(readOrderDto, _currentUserService.LanguageId);
                if (readOrderDto.OrderingUser != null)
                    await _translationService.Translate(readOrderDto.OrderingUser, _currentUserService.LanguageId);
                if (readOrderDto.Courier != null)
                    await _translationService.Translate(readOrderDto.Courier, _currentUserService.LanguageId);
                await _translationService.Translate(readOrderDto.Currency, _currentUserService.LanguageId);

                foreach (var orderItem in readOrderDto.OrderItems)
                {
                    await _translationService.Translate(orderItem, _currentUserService.LanguageId);
                }

                readOrderDtos.Add(readOrderDto);
            }


            return readOrderDtos;
        }

        public async Task<ReadOrderDto> GetOrder(Guid orderId)
        {
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();
            var orderRepository = _baseRepositoryProvider.GetRepository<Order>();
            var systemSettings = (await settingsRepository.GetAllAsync()).FirstOrDefault();

            var user = _currentUserService.User;

            var languageId = user != null
                ? user.LanguageId
                : systemSettings.DefaultLanguageId;

            var order = (await orderRepository.FindAsync(
                x => x.Id == orderId,
                nameof(Order.OrderingUser),
                nameof(Order.Courier),
                nameof(Order.Currency),
                nameof(Order.OrderItems))).FirstOrDefault();

            if (order == null)
                throw new BadRequestException($"Order with Id: {orderId} doesn't exist.");

            var readOrderDto = order.Adapt<ReadOrderDto>();

            readOrderDto.OrderingUser = order.OrderingUser?.Adapt<ReadUserDto>();
            readOrderDto.Courier = order.Courier?.Adapt<ReadUserDto>();
            readOrderDto.Currency = order.Currency.Adapt<ReadCurrencyDto>();
            readOrderDto.OrderItems = order.OrderItems.Select(x => x.Adapt<ReadOrderItemDto>());

            await _translationService.Translate(readOrderDto, languageId);
            if (readOrderDto.OrderingUser != null)
                await _translationService.Translate(readOrderDto.OrderingUser, languageId);
            if (readOrderDto.Courier != null)
                await _translationService.Translate(readOrderDto.Courier, languageId);
            await _translationService.Translate(readOrderDto.Currency, languageId);

            foreach (var orderItem in readOrderDto.OrderItems)
            {
                await _translationService.Translate(orderItem, languageId);
            }

            return readOrderDto;
        }

        private async Task<List<OrderItem>> FillOrderWithCartData(Order order, Cart cart)
        {
            var settingsRepository = _baseRepositoryProvider.GetRepository<Settings>();
            var systemSettings = (await settingsRepository.GetAllAsync()).FirstOrDefault();

            order.Status = OrderStatus.New;
            var orderItems = cart.CartItems.Select(x => new OrderItem
            {
                Price = x.Price,
                CurrencyId = x.CurrencyId,
                Name = x.Name
            }).ToList();
            order.CurrencyId = cart.CurrencyId;
            order.Price = orderItems.Sum(x => x.Price);
            order.DeliveryCost = _currencyExchangeService.GetPriceAfterExchange(systemSettings.DeliveryPriceInMainCurrency, cart.Currency);

            return orderItems;
        }

        private async Task SendOrderUpdateToClients(Order order)
        {
            var readOrderDto = order.Adapt<ReadOrderDto>();
            await _orderHub.Clients.Group(order.Id.ToString()).SendAsync("ReceiveOrderUpdate", readOrderDto);
        }

        private void ChangeOrderStatus(Order order, OrderStatus targetStatus)
        {
            var currentStatus = order.Status;
            switch (targetStatus)
            {
                case OrderStatus.New:
                    throw new BadRequestException($"Can't change order status to {OrderStatus.New}");

                case OrderStatus.Paid:
                    if (currentStatus != OrderStatus.New)
                        throw new BadRequestException($"Can't change order status from {currentStatus} to {OrderStatus.Paid}");
                    order.Status = targetStatus;
                    break;

                case OrderStatus.Pickup:
                    if (currentStatus != OrderStatus.Paid || currentStatus != OrderStatus.InProgress)
                        throw new BadRequestException($"Can't change order status from {currentStatus} to {OrderStatus.Pickup}");
                    order.Status = targetStatus;
                    break;

                case OrderStatus.InProgress:
                    if (currentStatus != OrderStatus.Pickup)
                        throw new BadRequestException($"Can't change order status from {currentStatus} to {OrderStatus.InProgress}");
                    order.Status = targetStatus;
                    break;

                case OrderStatus.Delivered:
                    if (currentStatus != OrderStatus.InProgress)
                        throw new BadRequestException($"Can't change order status from {currentStatus} to {OrderStatus.Delivered}");
                    order.Status = targetStatus;
                    break;

                case OrderStatus.Canceled:
                    if (currentStatus == OrderStatus.Delivered)
                        throw new BadRequestException($"Can't change order status from {currentStatus} to {OrderStatus.Canceled}");
                    order.Status = targetStatus;
                    break;
            }
        }
    }
}
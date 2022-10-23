using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Dtos.Contracts;
using FooDash.Domain.Entities.Orders;
using Mapster;

namespace FooDash.Application.MapperConfigurations
{
    public static class OrderMapperConfiguration
    {
        public static TypeAdapterConfig AddOrderConfiguration(this TypeAdapterConfig config)
        {
            config.
                NewConfig<CreateOrderContract, Order>();
            config
                .NewConfig<Order, ReadOrderDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<OrderItem, ReadOrderItemDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<CartDto, Cart>();
            config
                .NewConfig<Cart, ReadCartDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<CartItemDto, CartItem>();
            config
                .NewConfig<CartItem, ReadCartItemDto>()
                .IgnoreNullValues(true);

            return config;
        }
    }
}
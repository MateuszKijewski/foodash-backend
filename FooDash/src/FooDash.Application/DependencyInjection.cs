using FooDash.Application.Auth.Providers;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Security.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using FooDash.Application.Common.Services;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Auth.Services;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Translations.Services;
using FooDash.Application.Metadata.Services;
using FooDash.Application.Common.Interfaces.Metadata;
using FooDash.Application.Common.Interfaces.Converters;
using FooDash.Application.Common.Converters;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Common.Interfaces.Security;
using MapsterMapper;
using Mapster;
using FooDash.Application.MapperConfigurations;
using FooDash.Application.Products.Services;
using FooDash.Application.Common.Interfaces.Products;
using FooDash.Application.Common.Interfaces.Prices;
using FooDash.Application.Prices.Services;
using FooDash.Application.Common.Interfaces.System;
using FooDash.Application.System.Services;
using FooDash.Application.Common.Interfaces.Orders;
using FooDash.Application.Orders.Services;

namespace FooDash.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            #region SignalR

            services.AddSignalR();

            #endregion SignalR

            #region Mapper

            var mapperConfig = new TypeAdapterConfig()
                .AddTranslationsConfiguration()
                .AddIdentityConfiguration()
                .AddProductConfiguration()
                .AddOrderConfiguration()
                .AddPricesConfiguration();

            services.AddSingleton(mapperConfig);
            services.AddSingleton<IMapper, ServiceMapper>();

            #endregion Mapper

            #region Converters

            services.AddTransient<IEntityConverter, EntityConverter>();

            #endregion Converters

            #region Metadata

            services.AddScoped<IEntityService, EntityService>();

            #endregion Metadata

            #region Auth

            services.AddTransient<ITokenProvider, TokenProvider>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();

            #endregion Auth

            #region Security

            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<IHashingService, HashingService>();


            #endregion Security

            #region BasicOperations

            services.AddScoped(typeof(IBasicOperationsService<,,>), typeof(BasicOperationsService<,,>));

            #endregion BasicOperations

            #region Translations

            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<ILabelService, LabelService>();

            #endregion Translations

            #region Products

            services.AddTransient<IMenuService, MenuService>();

            #endregion Products

            #region Prices

            services.AddTransient<ICurrencyService, CurrencyService>();

            #endregion Prices

            #region System

            services.AddTransient<ISystemService, SystemService>();

            #endregion System

            #region Orders

            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, OrderService>();

            #endregion Orders

            return services;
        }
    }
}

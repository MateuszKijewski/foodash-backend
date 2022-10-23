using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Persistence.Common.Providers;
using FooDash.Persistence.Common.Repositories;
using FooDash.Persistence.Repositories.Auth;
using FooDash.Persistence.Repositories.Metadata;
using FooDash.Persistence.Repositories.Prices;
using FooDash.Persistence.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FooDashDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("FooDashDatabase")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IBaseRepositoryProvider, BaseRepositoryProvider>();
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
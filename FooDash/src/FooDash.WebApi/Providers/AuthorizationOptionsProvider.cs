using Dawn;
using FooDash.Application.Common.Exceptions;
using FooDash.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace FooDash.WebApi.Providers
{
    public class AuthorizationOptionsProvider
    {
        private readonly IServiceCollection _services;

        public AuthorizationOptionsProvider(IServiceCollection services)
        {
            _services = Guard.Argument(services, nameof(services)).NotNull().Value;
        }

        public AuthorizationOptions GetAuthorizationOptions(AuthorizationOptions authorizationOptions)
        {
            var dbContext = _services.BuildServiceProvider().GetService<FooDashDbContext>();
            if (dbContext == null)
            {
                throw new NotFoundException("Couldn't retrieve DbContext", nameof(FooDashDbContext));
            }

            var allEntities = dbContext.Entity.ToList();

            foreach (var entity in allEntities)
            {
                authorizationOptions.AddPolicy($"CanCreate{entity.Name}", policy =>
                {
                    policy.RequireClaim("PermissionKey", new string[] { entity.CreatePermissionKey.ToString() });
                });
                authorizationOptions.AddPolicy($"CanRead{entity.Name}", policy =>
                {
                    policy.RequireClaim("PermissionKey", new string[] { entity.ReadPermissionKey.ToString() });
                });
                authorizationOptions.AddPolicy($"CanUpdate{entity.Name}", policy => 
                {
                    policy.RequireClaim("PermissionKey", new string[] { entity.UpdatePermissionKey.ToString() });
                });
                authorizationOptions.AddPolicy($"CanDelete{entity.Name}", policy =>
                {
                    policy.RequireClaim("PermissionKey", new string[] { entity.DeletePermissionKey.ToString() });
                });
            }

            return authorizationOptions;
        }
    }
}

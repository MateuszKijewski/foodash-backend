using Dawn;
using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Metadata;
using FooDash.Domain.Entities.Orders;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Products;
using FooDash.Domain.Entities.System;
using FooDash.Domain.Entities.Translations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace FooDash.Persistence
{
    public class FooDashDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid _defaultAdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public FooDashDbContext(DbContextOptions<FooDashDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = Guard.Argument(httpContextAccessor, nameof(httpContextAccessor)).NotNull().Value;
        }

        #region Metadata

        public DbSet<Entity> Entity { get; set; }

        #endregion Metadata

        #region Identity

        public DbSet<User> User { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion Identity

        #region Translations

        public DbSet<Label> Label { get; set; }
        public DbSet<Language> Language { get; set; }

        #endregion Translations

        #region Products

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<ProductAttributeOptionSetValue> ProductAttributeOptionSetValue { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuCategoryProduct> MenuCategoryProduct { get; set; }

        #endregion Products

        #region Orders

        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }

        #endregion Orders

        #region System

        public DbSet<Currency> Currency { get; set; }
        public DbSet<Settings> Settings { get; set; }

        #endregion System

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FooDashDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var isSeederOrInstaller = Assembly.GetEntryAssembly().GetName().Name != "FooDash.WebApi";

            var now = DateTime.UtcNow;
            var userId = _httpContextAccessor.HttpContext?.User?.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            Guid? inputId = userId == null ? _defaultAdminId : Guid.Parse(userId);

            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            entry.Entity.CreatorId = isSeederOrInstaller ? null : (inputId ?? _defaultAdminId);
                            entry.Entity.CreatedOn = now;
                            entry.Entity.ModifierId = isSeederOrInstaller ? null : (inputId ?? _defaultAdminId);
                            entry.Entity.ModifiedOn = now;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entry.Entity.ModifierId = isSeederOrInstaller ? null : (inputId ?? _defaultAdminId);
                            entry.Entity.ModifiedOn = now;
                            break;
                        }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
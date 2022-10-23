using Dawn;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Products;
using FooDash.Persistence.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FooDash.Persistence.Repositories.Products
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        private readonly FooDashDbContext _dbContext;

        public MenuRepository(FooDashDbContext dbContext) : base(dbContext)
        {
            _dbContext = Guard.Argument(dbContext).NotNull();
        }

        public async Task<Menu?> GetActiveMenuWithCategoriesAndProducts()
        {
            return await _dbContext.Set<Menu>()
                .Include(x => x.MenuCategories)
                .ThenInclude(x => x.MenuCategoryProducts)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.IsActive);
        }
    }
}
using FooDash.Domain.Entities.Products;

namespace FooDash.Application.Common.Interfaces.Repositories
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<Menu?> GetActiveMenuWithCategoriesAndProducts();
    }
}
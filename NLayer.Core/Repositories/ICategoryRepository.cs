using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<Category> GetCategoryByIdWithProductsAsync(int id);
    }
}

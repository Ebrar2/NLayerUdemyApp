using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {


        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {

            //Eager Loading :datayı çekerken kategorilerinde alınması sağlanır
            //Lazer Loading:producta bağlı category i ihtiyaca göre sonra çekme 
            var productsWithCategory = await _context.Products.Include(x => x.Category).ToListAsync();
            return productsWithCategory;

        }
    }
}

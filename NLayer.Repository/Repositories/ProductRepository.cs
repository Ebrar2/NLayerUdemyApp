using Microsoft.EntityFrameworkCore;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

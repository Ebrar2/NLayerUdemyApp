using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetCategoryByIdWithProductsAsync(int id)
        {
            //FirstOrDefault:id den 4 5 tane bulursa ilkini döndürür
            //SingleOrDefault:koşulu sağlayan birden fazla kayıt bulursa hata döner.
            return await _context.Categories.Include(x=>x.Products).Where(x=>x.Id==id).SingleOrDefaultAsync();
               
          }
    }
}

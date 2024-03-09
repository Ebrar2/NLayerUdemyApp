using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product() { Id = 1, Name = "Kitap1", CategoryId = 1, Price = 100, CreatedDate = DateTime.Now },
                new Product() { Id = 2, Name = "Defter1", CategoryId = 2, Price = 50, CreatedDate = DateTime.Now },
                new Product() { Id = 3, Name = "Kalem1", CategoryId = 3, Price = 20, CreatedDate = DateTime.Now },
                new Product() { Id = 4, Name = "Kalem2", CategoryId = 3, Price = 24, CreatedDate = DateTime.Now },
                new Product() { Id = 5, Name = "Kalem3", CategoryId = 3, Price = 22, CreatedDate = DateTime.Now });

        }
    }
}

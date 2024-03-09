using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    internal class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "Kitaplar" },
                new Category { Id = 2, Name = "Defterler" },
               new Category { Id = 3, Name = "Kalemler" });
        }
    }
}

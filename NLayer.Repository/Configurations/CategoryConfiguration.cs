using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Configurations
{
    internal class CategoryConfiguration:IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();  //Birer birer artan 
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50); //name alanı zorunlu ve max 50 karakter


            builder.ToTable("Categories");
                
        }
        
    }
}

using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System.Reflection;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // var v = new Product() {ProductFeature=new ProductFeature() { } };
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFeature> ProductFeautures { get; set; } //İstersek productfeature ı burada kapatıp sadece product üzerinden eklenmesini sağlayabiliriz

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())   //track etmiş entitylerde dönüyoruz .Tracking:Entity framework kullanarak veri tabanından çekilmiş verilerin otomatik olarak izlenmesini sağlayan yapıdır.
            {
                //is:mi?
                if (item.Entity is BaseEntity entityReference)  //eğer item.Entity  BaseEntity ise(Tüm modeller baseEntityden reference alıyor)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                Entry(entityReference).Property(x => x.UpdatedDate).IsModified = false;
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }


                    }
                }
            }
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var item in ChangeTracker.Entries())   //track etmiş entitylerde dönüyoruz .Tracking:Entity framework kullanarak veri tabanından çekilmiş verilerin otomatik olarak izlenmesini sağlayan yapıdır.
            {
                                                               //is:mi?
                if(item.Entity is BaseEntity entityReference)  //eğer item.Entity  BaseEntity ise(Tüm modeller baseEntityden reference alıyor)
                {
                    switch(item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                                

                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  modelBuilder.Entity<Category>().HasKey(X => X.Id); Bu işleme burda da yapabiliriz [Key] olarak Id üstüne yazarak Category classında da fakat bu alanları temiz tutmak için
            //Congigurations dosyası altında her table için ayrı class içinde bu işlemler yapılır.

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  //IEntityTypeConfiguration implemente eden tüm assemblyleri getirir
                                                                                            //  modelBuilder.ApplyConfiguration(new ProductConfiguration());   //tek tek yapmak istesek böyle yaparız

            //best practicies için burda tanımlamamalı
            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature() { Id = 1, Width = 100, Height = 200, Color = "Kırmızı", ProductId = 1 },
                new ProductFeature() { Id = 2, Width = 100, Height = 200, Color = "Kırmızı", ProductId = 2 },
                new ProductFeature() { Id = 3, Width = 200, Height = 300, Color = "Yeşil", ProductId = 3 },
                new ProductFeature() { Id = 4, Width = 300, Height = 300, Color = "Mavi", ProductId = 4 }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}

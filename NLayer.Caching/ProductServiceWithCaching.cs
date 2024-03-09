using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    //    in memeory caching:Dataları nerde host ediliyorsa o hostun memorysini kullanacak.
    //    Clientdan bir istek geldiğinsw önce cashde varmı kontrol edilir
    //.Varsa cahcden çekilir.Yoksa
    //CORE->REPOSITORY->SERVİCE-CACHING(Servis katmanıyla aynı seviyede.Servis özelliklerini kullanabilmek için referans alacak)
    //Çok sık değiştirilmeyen ama çok sık erişilen data cahleme adayıdır.
    //var olan yapıyı bozmamak için IProductService implemente etmek gerekir
    public class ProductServiceWithCaching : IProductService
    {
        //tüm productları tutacağımız cache keyi
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;  //SERVİCE KATMANINDA DB YE YANSITMAK İÇİN


        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;

            if (!_memoryCache.TryGetValue(CacheProductKey, out _))  //belirtilen keye ait cahch varsa true döner.out_ diğerek cachedeki datayı dönmemesini sağlarız
            {
                memoryCache.Set(CacheProductKey, _productRepository.GetProductsWithCategory().Result);  //UYGUALAMADA CACHE BOŞSA DOLDUR  //RESULT ALINARAK ASENKRON METOT SENKRONA DÖNÜŞTÜRÜLÜR
                //CONSTRUCTORDA AWAIT KULLANILAMAZ
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {

            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {

            var any = await _productRepository.AnyAsync(expression);

            return any;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
            if (product == null)
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
            return await Task.FromResult(product);
        }

        public Task<List<ProductsWithCategoryDto>> GetProductsWithCategory()
        {

            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductsWithCategoryDto>>(products);
            return Task.FromResult(productsWithCategoryDto);
        }

        public async Task RemoveAsync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRange(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
            //asqueryable IEnumarable ı IQueryable a dönüştürür
            //IEnumarable:Veriyi belleğe alıp çalıştırır.
            //IQueryable:Veritabanında sorgu işlenir sonra veriler alınır
        }
        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, await _productRepository.GetAll().ToListAsync());
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }
    }

}

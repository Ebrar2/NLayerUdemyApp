using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    //try catch blokları mümkün olduğunca service katmanında tanımlanmalı .Az try catch kullanılmalı
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> genericRepository, IUnitOfWork unitOfWork, IProductRepository repository,IMapper mapper) : base(genericRepository, unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
        }

     
         public async Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategory()
        {
            var productsWithCategory = await _repository.GetProductsWithCategory();
            var productsWithCategoryDtos = _mapper.Map<List<ProductsWithCategoryDto>>(productsWithCategory);


            return CustomResponseDto<List<ProductsWithCategoryDto>>.Success(200,productsWithCategoryDtos);

        }

    }
}

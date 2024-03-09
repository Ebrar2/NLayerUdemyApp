using AutoMapper;
using Microsoft.AspNetCore.Http;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductServiceWithDto : ServiceWithDto<Product, ProductDto>,IProductServiceWithDto
    {
        private readonly IProductRepository _productRepository;
        public ProductServiceWithDto(IGenericRepository<Product> genericRepository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository ) : base(genericRepository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<ProductSaveDto>> AddAsync(ProductSaveDto dto)
        {
            var entity=_mapper.Map<Product>(dto);
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            var product = _mapper.Map<ProductSaveDto>(entity);
            return CustomResponseDto<ProductSaveDto>.Success(StatusCodes.Status200OK, product);
        }

        public async Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategory()
        {
            var productsWithCategory = await _productRepository.GetProductsWithCategory();
            var productsWithCategoryDtos = _mapper.Map<List<ProductsWithCategoryDto>>(productsWithCategory);


            return CustomResponseDto<List<ProductsWithCategoryDto>>.Success(StatusCodes.Status200OK,productsWithCategoryDtos);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }
    }
}

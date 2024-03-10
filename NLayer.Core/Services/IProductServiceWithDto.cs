using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IProductServiceWithDto:IServiceWithDto<Product,ProductDto>
    {
        public Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategory();
        public Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto);
        public Task<CustomResponseDto<ProductSaveDto>> AddAsync(ProductSaveDto dto);
        public Task<CustomResponseDto<List<ProductSaveDto>>> AddRangeAsync(List<ProductSaveDto> dto);
     
    }
}

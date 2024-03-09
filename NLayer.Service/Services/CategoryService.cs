using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(IGenericRepository<Category> genericRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper) : base(genericRepository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CustomResponseDto<CategoryWithProductDto>> GetCategoryByIdWithProductsAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdWithProductsAsync(id);
            var categoryByIdWithProducts = _mapper.Map<CategoryWithProductDto>(category);
            return CustomResponseDto<CategoryWithProductDto>.Success(200, categoryByIdWithProducts);

        }
    }
}

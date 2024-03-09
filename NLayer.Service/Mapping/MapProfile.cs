using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<ProductSaveDto, Product>().ReverseMap();
            CreateMap<ProductAddDto, Product>().ReverseMap();
            CreateMap<ProductsWithCategoryDto, ProductUpdateDto>().ReverseMap();
            CreateMap<ProductsWithCategoryDto, Product>().ReverseMap();
            CreateMap<Category, CategoryWithProductDto>();
        }
    }
}

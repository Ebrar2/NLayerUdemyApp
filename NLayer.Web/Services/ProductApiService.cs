using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductsWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductsWithCategoryDto>>>("products/GetProductsWithCategory");
            return response.Data;
        }

        public async Task<ProductSaveDto> SaveAsync(ProductSaveDto productSaveDto)
        {
            var response = await _httpClient.PostAsJsonAsync("products",productSaveDto); //  slaş koymaya gerek yok çünkü post yapınca tek bir fonksiyon çalışıyor 
            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductSaveDto>>();
            return responseBody.Data;
        }
        public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var response=await _httpClient.PutAsJsonAsync("products", productUpdateDto);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");

            return response.IsSuccessStatusCode;
        }
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");
            return response.Data;

        }
    }
}

﻿using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    public class CategoryApiService
    {

        private readonly HttpClient _httpClient;
        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CategoryDto>> GetAll()
        {
            var response =await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CategoryDto>>>("categories/GetAll");
            return response.Data;
        }
    }
}
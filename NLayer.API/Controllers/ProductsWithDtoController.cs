using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filter;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsWithDtoController : CustomBaseController
    {

        private readonly IProductServiceWithDto _service;


        public ProductsWithDtoController(IProductServiceWithDto service)
        {
            _service = service;
        }
        //birden fazla get isteği olduğunda hangini çağırması gerektiği belirtilmeli
        //[HttpGet("GetProductsWithCategory")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            var products = await _service.GetProductsWithCategory();
            return CreateActionResult(products);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();  //geriye entity(IEnumerable) dönüyor 
            return CreateActionResult(products);
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        // GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await _service.GetByIdAsync(id);  //geriye entity(IEnumerable) dönüyor 
            return CreateActionResult(products);
        }
        //Eğer productDto da validationa takılırsa metotun içine girmez
        [HttpPost]
        public async Task<IActionResult> Save(ProductSaveDto productDto)
        {
            var product = await _service.AddAsync(productDto);
        
            return CreateActionResult(product); //201 created 

        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            var response=await _service.UpdateAsync(productDto);

            return CreateActionResult(response);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response=await _service.RemoveAsync(id);
            return CreateActionResult(response);


        }
        [HttpPost("SaveAll")]
        public async Task<IActionResult> Save(List<ProductSaveDto> productSaveDtos)
        {
            var response = await _service.AddRangeAsync(productSaveDtos);
            return CreateActionResult(response);
        }
        [HttpDelete("RemoveAll")]
        public async Task<IActionResult> RemoveAll(List<int> ids)
        {
            var response = await _service.RemoveRange(ids);
            return CreateActionResult(response);
        }
        [HttpGet("Any{id}")]
        public async Task<IActionResult> Any(int id)
        {

            var response= await _service.AnyAsync(x => x.Id==id);
            return CreateActionResult(response);
        }

    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        private readonly IMapper _mapper;


        public ProductsController(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {

            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoryDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductAddDto productDto)
        {

            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoryDto, "Id", "Name");
            return View();

        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product=await _productService.GetByIdAsync(id);

            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoryDto, "Id", "Name",product.CategoryId);

            var productUpdateDto=_mapper.Map<ProductDto>(product);
            return View(productUpdateDto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if(ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productDto);
                await _productService.UpdateAsync(product);

                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoryDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product=await _productService.GetByIdAsync(id);
            await _productService.RemoveAsync(product);
            return RedirectToAction(nameof(Index));

        }
    }
}

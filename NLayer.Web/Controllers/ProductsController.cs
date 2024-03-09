using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
      
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {

            var categories = await _categoryApiService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductSaveDto productDto)
        {

            if (ModelState.IsValid)
            {
                await _productApiService.SaveAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryApiService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();

        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product=await _productApiService.GetByIdAsync(id);

            var categories = await _categoryApiService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            if(ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDto);

                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryApiService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(productDto);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productApiService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));

        }
    }
}

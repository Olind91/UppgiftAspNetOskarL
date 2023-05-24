using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OskarLAspNet.Helpers.Services;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoriesController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }




        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CategoryRegVM viewModel)
        {
            //Kollar om category redan finns, finns det -> Felmeddelande. Annars Skapas category.
            if (ModelState.IsValid)
            {
                //1:34:00 ish f.10.
                var category = await _productCategoryService.GetCategoryAsync(viewModel.CategoryName);
                if (category != null)
                    //409
                    return Conflict(new { category, error = "This category already exists mylord." });

                category = await _productCategoryService.CreateProductCategoryAsync(viewModel);
                if (category != null)


                TempData["SuccessMessage"] = "Category created successfully.";
                
                return RedirectToAction("Index", "Admin");
            }
            ModelState.Clear();
            return View();
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            if (ModelState.IsValid)
            {
                var categories = await _productCategoryService.GetCategoriesAsync();
                if (categories != null)

                    return Ok(categories);
            }
            return View();
        }

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using OskarLAspNet.Helpers.Repos;
using OskarLAspNet.Models.Dtos;
using OskarLAspNet.Models.Entities;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Helpers.Services
{
    public class ProductCategoryService
    {
        private readonly ProductCategoryRepo _productCategoryRepo;

        public ProductCategoryService(ProductCategoryRepo productCategoryRepo)
        {
            _productCategoryRepo = productCategoryRepo;
        }

        #region Create
        

        public async Task<ProductCategory> CreateProductCategoryAsync(CategoryRegVM viewModel)
        {
            var result = await _productCategoryRepo.AddAsync(viewModel);
            return result;
        }
        #endregion


        #region Get
        public async Task<ProductCategory> GetCategoryAsync(string categoryName)
        {
            var result = await _productCategoryRepo.GetAsync(x => x.CategoryName == categoryName);
            return result;
        }

       
        #endregion


        #region Get All
        public async Task<IEnumerable<ProductCategory>> GetCategoriesAsync()
        {
            var result = await _productCategoryRepo.GetAllAsync();


            var list = new List<ProductCategory>();
            foreach (var category in result)
                list.Add(category);

            return list;
        }
        #endregion



        //TEST

        public async Task<List<SelectListItem>> GetCategorySelectListAsync()
        {
            var categories = await _productCategoryRepo.GetAllAsync();

            return categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();
        }


        




        
    }
}

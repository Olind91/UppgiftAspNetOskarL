﻿using OskarLAspNet.Helpers.Repos;
using OskarLAspNet.Models.Dtos;
using OskarLAspNet.Models.Entities;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Helpers.Services
{
    public class ProductService
    {
        private readonly ProductRepo _productRepo;
        private readonly ProductCategoryService _productCategoryService;
        private readonly TagService _tagService;
        private readonly ProductTagRepo _productTagRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly TagRepo _tagRepo;

        public ProductService(ProductRepo productRepo, ProductCategoryService productCategoryService, TagService tagService, ProductTagRepo productTagRepo, IWebHostEnvironment webHostEnvironment, TagRepo tagRepo)
        {
            _productRepo = productRepo;
            _productCategoryService = productCategoryService;
            _tagService = tagService;
            _productTagRepo = productTagRepo;
            _webHostEnvironment = webHostEnvironment;
            _tagRepo = tagRepo;
        }

        #region Create

        public async Task<Product> CreateAsync(ProductEntity entity)
        {
            var _entity = await _productRepo.GetAsync(x => x.ArticleNumber == entity.ArticleNumber);
            if (_entity == null)
            {
                _entity = await _productRepo.AddAsync(entity);
                if (_entity != null)
                    return entity;
            }
            return null!;
        }

        #endregion



        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _productRepo.GetAllAsync();
        }

                       


        public async Task<bool> UploadImageAsync(Product product, IFormFile image)
        {
            try
            {
                string imagePath = $"{_webHostEnvironment.WebRootPath}/images/products/{product.ImageUrl}";
                await image.CopyToAsync(new FileStream(imagePath, FileMode.Create));
                return true;
            }
            catch { return false; }
        }

        
        public IEnumerable<Product> GetProductsByTagId(int tagId)
        {
            var products = _productRepo.GetProductsByTagId(tagId);
            return products.Select(p => (Product)p).ToList();
        }



        public async Task AddProductTagsAsync(ProductEntity entity, string[] tags)
        {
            foreach (var tag in tags)
            {
                await _productTagRepo.AddAsync(new ProductTagEntity
                {
                    ArticleNumber = entity.ArticleNumber,
                    TagId = int.Parse(tag),



                });
            }
        }
        







        public async Task<Product> GetByArticleNumberAsync(string articleNumber)
        {
            return await _productRepo.GetAsync(x => x.ArticleNumber == articleNumber);
        }



    }
}

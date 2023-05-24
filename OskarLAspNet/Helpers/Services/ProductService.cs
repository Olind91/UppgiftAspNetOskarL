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

        

        //TEST - BEHÖVS INTE ENS!?
        public async Task<Product> CreateProductAsync(ProductRegVM viewModel)
        {
            ProductEntity entity = viewModel;



                if (int.TryParse(viewModel.SelectedCategory, out int parsedCategoryId))
                {
                    await _productCategoryService.AssociateProductWithCategoryAsync(entity, parsedCategoryId);
                }
                else
                {
                    // Handle the case where the categoryId is not a valid integer
                    // You can choose to log an error, skip the category, or throw an exception
                }


            // Create the product
            entity = await _productRepo.AddAsync(entity);

            if (entity != null)
            {
                // Add tags to the product (if applicable)
                foreach (var tagName in viewModel.Tags)
                {
                    var tag = await _tagService.GetTagAsync(tagName);
                    tag ??= await _tagService.CreateTagAsync(tagName);

                    await _productTagRepo.AddAsync(new ProductTagEntity
                    {
                        ArticleNumber = entity.ArticleNumber,
                        TagId = tag.Id,
                    });
                }

                return await GetProductAsync(entity.ArticleNumber);
            }
            return null!;
        }
        #endregion



        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _productRepo.GetAllAsync();
        }


        #region Get
        public async Task<Product> GetProductAsync(string articleNumber)
        {
            var entity = await _productRepo.GetAsync(x => x.ArticleNumber == articleNumber);
            if (entity != null)
            {
                Product product = entity;

                //2:56:00 f.10
                if (entity.ProductTags.Count > 0)
                {
                    var tagList = new List<Tag>();

                    foreach (var productTag in entity.ProductTags)
                        tagList.Add(new Tag { Id = productTag.Tag.Id, TagName = productTag.Tag.TagName });

                    product.Tags = tagList;
                }

                return product;
            }

            return null!;

        }
        #endregion

        public async Task<Product> UpdateProductAsync(Product product)
        {
            //Hämtar tag via ID, om tag id finns, uppdaterar.
            var entity = await _productRepo.GetAsync(x => x.ProductName == product.ProductName);
            if (entity != null)
            {
                product.ProductName = product.ProductName;
                var result = await _productRepo.UpdateAsync(entity);
                return result;
            }

            return null!;
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

        //TEST
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

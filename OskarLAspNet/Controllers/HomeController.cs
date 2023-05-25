using Microsoft.AspNetCore.Mvc;
using OskarLAspNet.Helpers.Services;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;

        public HomeController(ProductService productService)
        {
            _productService = productService;
        }

       


        //TEST
       /* public IActionResult Index()
        {
            //Ändra nummer beroende på Tag. t.ex. 1 = new. 2 = featured. 3 = popular
            var products = _productService.GetProductsByTagId(1);

            return View(products);
        }*/



        //TEST
        public IActionResult Index()
        {
            var model = new ProductsVM();
            // Filter products for the first section (e.g., tag ID = 1)
            model.NewProducts = _productService.GetProductsByTagId(1);

            // Filter products for the second section (e.g., tag ID = 2)
            model.FeaturedProducts = _productService.GetProductsByTagId(2);

            model.PopularProducts = _productService.GetProductsByTagId(3);

            return View(model);
        }
    }
}

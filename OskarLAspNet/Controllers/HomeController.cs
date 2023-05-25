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

       


      


        //SKAPA New / Featured / Popular Tag. Går sedan efter vilken tag produkt har och lägger upp på olika delar av förstasidan
        public IActionResult Index()
        {
            var model = new ProductsVM();
            
            //New
            model.NewProducts = _productService.GetProductsByTagId(1);

            //Featured
            model.FeaturedProducts = _productService.GetProductsByTagId(2);

            //Popular
            model.PopularProducts = _productService.GetProductsByTagId(3);

            return View(model);
        }
    }
}

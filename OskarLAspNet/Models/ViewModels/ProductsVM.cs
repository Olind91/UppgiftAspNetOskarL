using OskarLAspNet.Models.Dtos;

namespace OskarLAspNet.Models.ViewModels
{
    public class ProductsVM
    {
        public string ArticleNumber { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public decimal? Price { get; set; }
        public ProductCategory ProductCategory { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public IEnumerable<Product> NewProducts { get; set; } = new List<Product>();
        public IEnumerable<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IEnumerable<Product> PopularProducts { get; set; } = new List<Product>();

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}

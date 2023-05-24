using OskarLAspNet.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OskarLAspNet.Models.ViewModels
{
    public class CategoryRegVM
    {
        [Required(ErrorMessage = "Please enter a category")]
        [MinLength(2, ErrorMessage = "Your new categoryname should be atleast 2 characters long for good database structure :)")]
        public string CategoryName { get; set; } = null!;


        public static implicit operator ProductCategoryEntity(CategoryRegVM viewModel)
        {
            return new ProductCategoryEntity
            {
                CategoryName = viewModel.CategoryName

            };
        }
    }
}

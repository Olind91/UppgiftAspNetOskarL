using OskarLAspNet.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OskarLAspNet.Models.ViewModels
{
    public class TagRegVM
    {
        [Required(ErrorMessage = "Please enter a tag")]
        [MinLength(2, ErrorMessage = "Your new tagname should be atleast 2 characters long for good database structure :)")]
        public string TagName { get; set; } = null!;




        public static implicit operator TagEntity(TagRegVM viewModel)
        {
            return new TagEntity
            {
                TagName = viewModel.TagName,

            };
        }
    }
}

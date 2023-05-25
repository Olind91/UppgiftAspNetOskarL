using Microsoft.AspNetCore.Mvc.Rendering;
using OskarLAspNet.Helpers.Repos;
using OskarLAspNet.Models.Dtos;
using OskarLAspNet.Models.Entities;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Helpers.Services
{
    //Overloads inte alltid nödvändiga.

    public class TagService
    {
        private readonly TagRepo _tagRepo;

        public TagService(TagRepo tagRepo)
        {
            _tagRepo = tagRepo;
        }


        #region Create        
        
        public async Task<Tag> CreateTagAsync(TagRegVM viewModel)
        {

            var result = await _tagRepo.AddAsync(viewModel);
            return result;
        }
        #endregion

        #region Get
         public async Task<Tag> GetTagAsync(string tagName)
         {
             var result = await _tagRepo.GetAsync(x => x.TagName == tagName);
             return result;



         }
        #endregion


        #region Get All
        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            var result = await _tagRepo.GetAllAsync();

            //Behöver omvandla till lista då det annars hämtas som TagEntity
            var list = new List<Tag>();
            foreach (var tag in result)
                list.Add(tag);

            return list;
        }
        #endregion      
         
        public async Task<List<SelectListItem>> GetTagsAsync()
        {
            var tags = new List<SelectListItem>();



            foreach (var tag in await _tagRepo.GetAllAsync())
            {
                tags.Add(new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.TagName



                });
            }
            return tags;
        }



        public async Task<List<SelectListItem>> GetTagsAsync(string[] selectedTags)
        {
            var tags = new List<SelectListItem>();



            foreach (var tag in await _tagRepo.GetAllAsync())
            {
                tags.Add(new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.TagName,
                    Selected = selectedTags.Contains(tag.Id.ToString())



                });
            }
            return tags;
        }
    }
}
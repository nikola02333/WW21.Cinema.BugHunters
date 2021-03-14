using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class TagService : ITagService
    {
        private readonly ITagsRepository _tagsRepository;

        public TagService(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }
        public async Task<GenericResult<TagDomainModel>> AddTagAsync(TagDomainModel tagToCreate)
        {
            var tagExists = _tagsRepository.GetByIdAsync(tagToCreate.TagName);
            
            if( tagExists != null)
            {
                return new GenericResult<TagDomainModel>
                {
                   IsSuccessful= false,
                   ErrorMessage= "this tag alredy exists"
                };
            }

            var tagToAdd = new Tag
            {
                TagName= tagToCreate.TagName,
                 TagValue= tagToCreate.TagValue ?? ""
            };

           var result= await _tagsRepository.InsertAsync(tagToAdd);
            _tagsRepository.SaveAsync();

            return new GenericResult<TagDomainModel> 
            { 
              IsSuccessful= true,
              Data = new TagDomainModel
              {
                  
                   TagName=result.TagName,
                   TagValue= result.TagValue
              } 
            };
        }

        public Task<GenericResult<TagDomainModel>> DeleteTagAsync(string tagToDelete)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResult<TagDomainModel>> GetTagByName(string tag)
        {
            var tagToFind = await _tagsRepository.GetByIdAsync(tag);
            if(tagToFind == null)
            {
                return new GenericResult<TagDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Error while finding this tag "
                };
            }
            return new GenericResult<TagDomainModel>
            {
                IsSuccessful = true,
                Data= new TagDomainModel
                {
                     TagId = tagToFind.TagId,
                     TagName= tagToFind.TagName,
                     TagValue=tagToFind.TagValue
                }
            };
        }
    }
}

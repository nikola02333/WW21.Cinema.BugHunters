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
    public class TagsMoviesService : ITtagsMoviesService
    {
        private readonly ITagsMoviesRepository _tagsMoviesRepository;

        public TagsMoviesService(ITagsMoviesRepository tagsMoviesRepository)
        {
            _tagsMoviesRepository = tagsMoviesRepository;
        }
        public async Task<GenericResult<TagMovieDomainModel>> AddTagMovieAsync(TagsMovies tagsToadd)
        {
            var tagMovie =  await _tagsMoviesRepository.InsertAsync(tagsToadd);

            return new GenericResult<TagMovieDomainModel>
            {
                IsSuccessful = true,
                Data = new TagMovieDomainModel
                {
                     MovieModel = new MovieDomainModel
                     {
                         Current = tagMovie.Movie.Current,
                         Title= tagMovie.Movie.Title,
                          Genre = tagMovie.Movie.Genre,
                           Id= tagMovie.Movie.Id,
                            Year = tagMovie.Movie.Year,
                             Rating = tagMovie.Movie.Year
                              
                     },
                     TagModel= new TagDomainModel
                     {
                         TagId= tagMovie.Tag.TagId,
                         TagName= tagMovie.Tag.TagName,
                          TagValue= tagMovie.Tag.TagValue
                     }

                }
            };
        }

        public async  Task<GenericResult<TagMovieDomainModel>> GetTagByMovieIDAsync(Guid movieId)
        {
            var tagsMovies = await _tagsMoviesRepository.GetTagByMovieIDAsync(movieId);

            return null;
          
        }

        public Task<GenericResult<TagMovieDomainModel>> SearchTagAsync(TagDomainModel tagDomainModel)
        {
            throw new NotImplementedException();
        }
    }
}

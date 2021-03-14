using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
   public interface ITtagsMoviesService
    {
        Task<GenericResult<TagMovieDomainModel>> AddTagMovie(TagsMovies tagsToAdd);

        Task<GenericResult<TagMovieDomainModel>> SearchTag(TagDomainModel tagDomainModel);

    }
}

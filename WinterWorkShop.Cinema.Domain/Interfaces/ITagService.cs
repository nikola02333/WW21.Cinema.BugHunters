using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
   public interface ITagService
    {
        Task<GenericResult<TagDomainModel>> AddTagAsync(TagDomainModel tagToCreate);

        Task<GenericResult<TagDomainModel>> DeleteTagAsync(string tagToDelete);

        Task<GenericResult<TagDomainModel>> GetTagByName(string tag);
    }
}

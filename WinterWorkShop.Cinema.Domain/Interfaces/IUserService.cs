using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Common;
namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IUserService
    {
       
        Task<GenericResult<UserDomainModel>> GetAllAsync();

      
        Task<GenericResult<UserDomainModel>> GetUserByIdAsync(Guid id);

     
        Task<GenericResult<UserDomainModel>> CreateUserAsync(UserDomainModel userToCreate);


        Task<GenericResult<UserDomainModel>> GetUserByUserNameAsync(string username);

        Task<GenericResult<UserDomainModel>> DeleteUserAsync(Guid userId);

        Task<GenericResult<UserDomainModel>> UpdateUserAsync(Guid userId, UserDomainModel userToUpdate);

        Task<bool> CheckUsername(string usernameToCreate);
    }
}

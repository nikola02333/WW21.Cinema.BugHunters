using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of Users</returns>
        Task<GenericResult<UserDomainModel>> GetAllAsync();

        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        Task<GenericResult<UserDomainModel>> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        Task<GenericResult<UserDomainModel>> CreateUserAsync(UserDomainModel userToCreate);

        /// <summary>
        /// Get a user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>User</returns>
        Task<GenericResult<UserDomainModel>> GetUserByUserNameAsync(string username);

        Task<GenericResult<UserDomainModel>> DeleteUserAsync(Guid userId);

        Task<GenericResult<UserDomainModel>> UpdateUserAsync(Guid userId, UserDomainModel userToUpdate);
    }
}

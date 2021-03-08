using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<GenericResult<UserDomainModel>> CreateUserAsync(UserDomainModel userToCreate)
        {
          if(userToCreate ==null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage= Messages.USER_CREATION_ERROR
                };
            }

            User user = new User()
            {
                FirstName = userToCreate.FirstName,
                 LastName= userToCreate.LastName,
                  UserName= userToCreate.UserName.ToLower(),
                   Role= userToCreate.Role.ToLower()
            };

            var userResult = await _usersRepository.InsertAsync(user);
            _usersRepository.SaveAsync();


            return new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                     FirstName= userResult.FirstName,
                    LastName = userResult.LastName,
                     UserName = userResult.UserName,
                     Role = userResult.Role,
                     Id= userResult.Id
                }
            };

        }

        public async Task<GenericResult<UserDomainModel>> DeleteUserAsync(Guid userId)
        {
            var userToDelete = await _usersRepository.GetByIdAsync(userId);
            
            if(userToDelete == null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage =Messages.USER_NOT_FOUND
                };

            }
            
            _usersRepository.Delete(userToDelete.Id);
            _usersRepository.Save();

            return new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
            };
        }

        public async Task<GenericResult<UserDomainModel>> GetAllAsync()
        {
            var data = await _usersRepository.GetAll();


            GenericResult<UserDomainModel> result = new GenericResult<UserDomainModel>();


            var items = data.Select(item => new UserDomainModel
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.UserName,
                Role = item.Role,
            }).ToList();

           return  new GenericResult<UserDomainModel> 
            { 
            IsSuccessful=true,
            DataList= items
            };
        }

        public async Task<GenericResult<UserDomainModel>> GetUserByIdAsync(Guid id)
        {
            var data = await _usersRepository.GetByIdAsync(id);

            if (data == null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_NOT_FOUND
                };
            }

            GenericResult<UserDomainModel> domainModel = new GenericResult<UserDomainModel>
            { 
                IsSuccessful=true,
                Data = new UserDomainModel
             {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                Role = data.Role,
            }
        };

            return domainModel;
        }

        public async Task<GenericResult<UserDomainModel>> GetUserByUserNameAsync(string username)
        {
            var data = _usersRepository.GetByUserName(username.ToLower());

            if (data == null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_NOT_FOUND
                };
            }

            GenericResult<UserDomainModel> domainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful=true,
                Data =new UserDomainModel {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                Role = data.Role,
            }
            };

            return domainModel;
        }

        public async Task<GenericResult<UserDomainModel>> UpdateUserAsync(Guid userId, UserDomainModel userToUpdate)
        {
            var userForUpdate = await _usersRepository.GetByIdAsync(userId);

            if(userForUpdate ==null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage= Messages.USER_NOT_FOUND
                };
            }

            userForUpdate.FirstName = userToUpdate.FirstName;
            userForUpdate.LastName = userToUpdate.LastName;
            userForUpdate.UserName = userToUpdate.UserName.ToLower();
            userForUpdate.Role = userToUpdate.Role.ToLower();

            _usersRepository.Update(userForUpdate);
            _usersRepository.SaveAsync();

            return new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data =  new UserDomainModel
                {
                    Id = userForUpdate.Id,
                    Role = userForUpdate.Role,
                    FirstName = userForUpdate.FirstName,
                    LastName = userForUpdate.LastName,
                     UserName = userForUpdate.UserName
                } 
            };

        }
    }
}

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

        public async Task<bool> CheckUsername(string usernameToCreate)
        {
           return await _usersRepository.CheckUsername(usernameToCreate);
        }

        public async Task<GenericResult<UserDomainModel>> CreateUserAsync(UserDomainModel userToCreate)
        {
        

            var usernameExists =await CheckUsername(userToCreate.UserName.ToLower());

            if (!usernameExists)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage= Messages.USER_CREATION_ERROR_USERNAME_EXISTS
                };
                   
            }
            User user = new User()
            {
                FirstName = userToCreate.FirstName,
                 LastName= userToCreate.LastName,
                  UserName= userToCreate.UserName.ToLower(),
                   Role= userToCreate.Role.ToLower(),
                   Points= 0
            };

            var userResult = await _usersRepository.InsertAsync(user);

            if(userResult == null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage= Messages.USER_CREATION_ERROR
                };
            }
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
            var allUsers = await _usersRepository.GetAllAsync();


            GenericResult<UserDomainModel> result = new GenericResult<UserDomainModel>();


            var items = allUsers.Select(item => new UserDomainModel
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.UserName,
                Role = item.Role,
                BonusPoints= item.Points.ToString()
            }).ToList();

           return  new GenericResult<UserDomainModel> 
            { 
            IsSuccessful=true,
            DataList= items
            };
        }

        public async Task<GenericResult<UserDomainModel>> GetUserByIdAsync(Guid id)
        {
            var user = await _usersRepository.GetByIdAsync(id);

            if (user == null)
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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Role = user.Role,
            }
        };

            return domainModel;
        }

        public async Task<GenericResult<UserDomainModel>> GetUserByUserNameAsync(string username)
        {
            var user = await _usersRepository.GetByUserNameAsync(username.ToLower());

            if (user == null)
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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Role = user.Role,
                BonusPoints = user.Points.ToString()
                
                }
            };

            return domainModel;
        }

        public async Task<GenericResult<UserDomainModel>> IncrementBonusPointsForUser(Guid userId,int number)
        {
            var userExists = await _usersRepository.GetByIdAsync(userId);
            if(userExists == null)
            {
                return new GenericResult<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_NULL
                };
            }

            userExists.Points+=number;
            _usersRepository.Update(userExists);
            _usersRepository.Save();

            return new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel 
                {
                   UserName= userExists.UserName,
                    BonusPoints= userExists.Points.ToString(),
                     FirstName= userExists.FirstName,
                      LastName= userExists.LastName,
                       Id= userExists.Id
                }
            };
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

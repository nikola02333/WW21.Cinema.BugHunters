using System;
using System.Collections.Generic;
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
                    ErrorMessage= "error message"
                };
            }

            User user = new User()
            {
                FirstName = userToCreate.FirstName,
                 LastName= userToCreate.LastName,
                  UserName= userToCreate.UserName,
                   IsAdmin= userToCreate.IsAdmin
            };
            var userResult = _usersRepository.InsertAsync(user);
            _usersRepository.Save();


            return new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel()
            };

        }

        public async Task<GenericResult<UserDomainModel>> GetAllAsync()
        {
            var data = await _usersRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            GenericResult<UserDomainModel> result = new GenericResult<UserDomainModel>();

            UserDomainModel model;
            foreach (var item in data)
            {
                model = new UserDomainModel
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName,
                    IsAdmin = item.IsAdmin,
                };
                result.DataList.Add(model);
            }

            return result;
        }

        public async Task<GenericResult<UserDomainModel>> GetUserByIdAsync(Guid id)
        {
            var data = await _usersRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

            GenericResult<UserDomainModel> domainModel = new GenericResult<UserDomainModel>
            { 
                IsSuccessful=true,
                Data =
             {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                IsAdmin = data.IsAdmin,
            }
        };

            return domainModel;
        }

        public async Task<GenericResult<UserDomainModel>> GetUserByUserNameAsync(string username)
        {
            var data = _usersRepository.GetByUserName(username);

            if (data == null)
            {
                return null;
            }

            GenericResult<UserDomainModel> domainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful=true,
                Data ={
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                IsAdmin = data.IsAdmin,
            }
            };

            return domainModel;
        }
    }
}

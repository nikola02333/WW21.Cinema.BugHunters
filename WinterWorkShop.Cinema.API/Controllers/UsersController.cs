using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

     
        [HttpGet]
        public async Task<ActionResult<UserDomainModel>> GetAsync()
        {
            GenericResult<UserDomainModel> userDomainModels;

            userDomainModels = await _userService.GetAllAsync();


            return Ok(userDomainModels.DataList);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<UserDomainModel>> GetbyIdAsync(Guid id)
        {
            GenericResult<UserDomainModel> result;

            result = await _userService.GetUserByIdAsync(id);

            if (!result.IsSuccessful)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(result.Data);
        }
       
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<UserDomainModel>> CreateUserAsync(CreateUserModel createUser)
        {

            var userToCreate = new UserDomainModel
            {
                FirstName = createUser.FirstName,
                LastName= createUser.LastName,
                 Role= createUser.Role,
                  UserName= createUser.UserName,
            };
            var user = await _userService.CreateUserAsync(userToCreate);

            if(!user.IsSuccessful)
            {
                return BadRequest(user.ErrorMessage);
            }

            return CreatedAtAction("GetById", new { Id= user.Data.Id }, user.Data);
        }
       
        [HttpGet]
        [Route("Search/{username}")]
        public async Task<ActionResult<UserDomainModel>> GetbyUserNameAsync(string username)
        {
            GenericResult<UserDomainModel> user;

            user = await _userService.GetUserByUserNameAsync(username);

            if (!user.IsSuccessful)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(user.Data);
        }


        [HttpDelete]
        [Route("Delete/{userId}")]
        public async Task<ActionResult<UserDomainModel>> DeleteUser(Guid userId)
        {
            if(userId == null || userId == Guid.Empty)
            {
                return BadRequest();
            }


            var result = await _userService.DeleteUserAsync(userId);

           if(!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Accepted();
        }

        [HttpPut]
        [Route("Update/{userId}")]
        public async Task<ActionResult<UserDomainModel>> UpdateUserAsync(Guid userId, CreateUserModel userToUpdate)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            UserDomainModel userUpdate = new UserDomainModel
            {
                UserName= userToUpdate.UserName,
                 FirstName= userToUpdate.FirstName,
                  LastName = userToUpdate.LastName,
                   Role= userToUpdate.Role
            };
            var result = await _userService.UpdateUserAsync(userId, userUpdate);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Accepted();
        }
    }
}

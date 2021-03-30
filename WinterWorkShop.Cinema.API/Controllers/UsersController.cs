using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

     
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> GetAsync()
        {
            GenericResult<UserDomainModel> userDomainModels;

            userDomainModels = await _userService.GetAllAsync();


            return Ok(userDomainModels.DataList);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> GetbyIdAsync(Guid id)
        {
            GenericResult<UserDomainModel> user;

            user = await _userService.GetUserByIdAsync(id);

            if (!user.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = user.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Ok(user.Data);
        }
       
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> CreateUserAsync(CreateUserModel createUser)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }    
            var userToCreate = new UserDomainModel
            {
                FirstName = createUser.FirstName,
                LastName= createUser.LastName,
                 Role= "user",
                  UserName= createUser.UserName,
            };


            GenericResult<UserDomainModel> user;
            try
            {
              user = await _userService.CreateUserAsync(userToCreate);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!user.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = user.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction("GetById", new { Id= user.Data.Id }, user.Data);
        }
       
        [HttpGet]
        [Route("byusername/{username}")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> GetbyUserNameAsync(string username)
        {
            if(String.IsNullOrEmpty(username))
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_NOT_FOUND,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);

            }

            GenericResult<UserDomainModel> user;

            user = await _userService.GetUserByUserNameAsync(username);

            if (!user.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = user.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(user.Data);
        }


        [HttpDelete]
        [Route("Delete/{userId}")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> DeleteUser(Guid userId)
        {
            if(userId == null || userId == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_ID_NULL,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            var user = await _userService.DeleteUserAsync(userId);

            if (!user.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = user.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Accepted();
        }

        [HttpPut]
        [Route("Update/{userId}")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> UpdateUserAsync(Guid userId, UpdateUserModel userToUpdate)
        {
            if (userId == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_ID_NULL,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            if(!ModelState.IsValid)
            {
               
                return BadRequest(ModelState);
            }

            UserDomainModel userUpdate = new UserDomainModel
            {
                //UserName= userToUpdate.UserName,
                 FirstName= userToUpdate.FirstName,
                  LastName = userToUpdate.LastName,
                   //Role= "user"
            };
            var user = await _userService.UpdateUserAsync(userId, userUpdate);

            if (!user.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = user.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Accepted();
        }
        [HttpPost]
        [Route("IncrementPoints/{userId}")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> IncrementBonusPointsForUser(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_ID_NULL,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

           var userResult= await _userService.IncrementBonusPointsForUser(userId);

            if(!userResult.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_INCREMENT_POINTS_ERROR,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            return Accepted();
        }
    }
}

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

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDomainModel>>> GetAsync()
        {
            GenericResult<UserDomainModel> userDomainModels;

            userDomainModels = await _userService.GetAllAsync();

            if (userDomainModels == null)
            {
                
            }

            return Ok(userDomainModels);
        }

        /// <summary>
        /// Gets User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDomainModel>> GetbyIdAsync(Guid id)
        {
            GenericResult<UserDomainModel> model;

            model = await _userService.GetUserByIdAsync(id);

            if (model == null)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<UserDomainModel>> CreateUser(CreateUserModel createUser)
        {

            //return _userService.CreateUserAsync();
            return null;
        }
        // <summary>
        /// Gets User by UserName
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<UserDomainModel>> GetbyUserNameAsync(string username)
        {
            GenericResult<UserDomainModel> model;

            model = await _userService.GetUserByUserNameAsync(username);

            if (model == null)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(model);
        }
    }
}

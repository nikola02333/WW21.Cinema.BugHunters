using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using System;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.API.TokenServiceExtensions;
using WinterWorkShop.Cinema.Domain.Interfaces;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [OpenApiIgnore]
    public class DemoAuthenticationController : ControllerBase    
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
    
        public DemoAuthenticationController(IConfiguration configuration,IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        // NOT FOR PRODUCTION USE!!!
        // you will need a robust auth implementation for production
        // i.e. try IdentityServer4


     
        [HttpPost]
        [Route("/get-token")]
        public async Task<ActionResult> GenerateToken([FromBody]UserLoginModel userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userService.GetUserByUserNameAsync(userLogin.UserName);

            
            if(userExists.IsSuccessful)
            {


                var jwt = JwtTokenGenerator
                .Generate(userLogin.UserName, userExists.Data.Role,userExists.Data.Id, _configuration["Tokens:Issuer"], _configuration["Tokens:Key"]);

                return Ok(new { token = jwt, firstName= userExists.Data.FirstName });
            }

            return Unauthorized();
            
        }

        [HttpGet]
        [Route("/get-token")]
        public  ActionResult GenerateToken()
        {
           
                var jwt = JwtTokenGenerator
                .Generate("","guest",Guid.NewGuid(), _configuration["Tokens:Issuer"], _configuration["Tokens:Key"]);

                return Ok(new { token = jwt, firstName = "guest"});
           
        }
    }
}

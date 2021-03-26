using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ImdbsController : ControllerBase
    {
        [HttpGet]
        [Route("TegTopTenMovies")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> GetTopTenMovies()
        {

            return null;
        }
    }
}

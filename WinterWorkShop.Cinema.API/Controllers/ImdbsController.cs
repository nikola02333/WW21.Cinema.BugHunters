using IMDbApiLib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
          
            var apiLib = new ApiLib("k_9szm9guo");
            var data = await apiLib.SearchMovieAsync("Inception 2010");

            return null;
        }
    }
}

using IMDbApiLib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
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
        [Route("Search/{searchMovie}")]
        public async Task<ActionResult> GetMovieDetails( string searchMovie)
        {
          
            var apiLib = new ApiLib("k_9szm9guo");
            var data = await apiLib.TitleAsync( searchMovie + "/FullActor,Images,Ratings");

            if(data.ErrorMessage == "")
            {
                return Ok(data);
            }

            ErrorResponseModel errorResponse = new ErrorResponseModel
            {
                ErrorMessage = Messages.IMDB_MOVIE_NOT_FOUND,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            return BadRequest(errorResponse);
        }

        [HttpGet]
        [Route("GetTopTenMovies")]
        public async Task<ActionResult> GetTopTenMovies()
        {

            var apiLib = new ApiLib("k_9szm9guo");
            var data = await apiLib.Top250MoviesAsync();

            if (data.ErrorMessage == "")
            {
                return Ok(data.Items.Take(10));
            }

            ErrorResponseModel errorResponse = new ErrorResponseModel
            {
                ErrorMessage = Messages.IMDB_MOVIE_NOT_FOUND,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            return BadRequest(errorResponse);
        }
    }
}

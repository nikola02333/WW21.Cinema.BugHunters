using IMDbApiLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public ImdbsController(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        [HttpGet]
        [Route("Search/{searchMovie}")]
        public async Task<ActionResult> SearchByMovieId(string searchMovie)
        {


            var apiLib = new ApiLib(_configuration["IMDB_API_KEY:key"]);
            var data = await apiLib.TitleAsync(searchMovie + "/Trailer,Ratings");

            //var rand = new Random();

            if (data.ErrorMessage == "")
            {
                var result = new ImdbSearchModel
                {
                    Title = data.Title,
                    Year = data.Year,

                    ActorList = data.ActorList.Select(actor => new Actor { Name = actor.Name }).Take(3).ToList(),
                    Awards = data.Awards,
                    Genres = data.Genres.Split(",")[0],
                    Image = data.Image,
                    IMDbRating = data.IMDbRating,
                    Plot = data.Plot,
                    RuntimeMins = data.RuntimeMins,
                };

                return Ok(result);
            }

            ErrorResponseModel errorResponse = new ErrorResponseModel
            {
                ErrorMessage = Messages.IMDB_MOVIE_NOT_FOUND,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            return BadRequest(errorResponse);
        }


        [HttpGet]
        [Route("Search/video/{searchMovie}")]
        public async Task<ActionResult> SearchVideoByMovieId( string searchMovie)
        {
            

            var apiLib = new ApiLib(_configuration["IMDB_API_KEY:key"]);
            var data = await apiLib.TitleAsync( searchMovie + "/Trailer,Ratings");
            var youtubeTrailer = await apiLib.YouTubeTrailerAsync(searchMovie);

            var result = new IMDBModel
            {
                Data = data,
                YoutubeData = youtubeTrailer
            };

            if (data.ErrorMessage == "" && youtubeTrailer.ErrorMessage=="")
            {
                return Ok(result);
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

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [HttpGet]
        [Route("GetById/{id:guid}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> GetByIdAsync(Guid id)
        {
           
            GenericResult<MovieDomainModel> movie;

            movie = await _movieService.GetMovieByIdAsync(id);

            if (!movie.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage =movie.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };

                return NotFound(errorResponse);
            }

            return Ok(movie.Data);
        }

        [HttpGet]
        [Route("AllMovies/{isCurrent}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> GetAllAsync(bool? isCurrent)
        {
            
            GenericResult<MovieDomainModel> movieDomainModels;

            movieDomainModels = await _movieService.GetAllMoviesAsync(isCurrent);

           
            return Ok(movieDomainModels.DataList);
        }

        [HttpGet]
        [Route("TopTenMovies")]
        public async Task<ActionResult> GetTopTenMoviesAsync()
        {

            var movies =await _movieService.GetTopTenMoviesAsync();
            return Ok(movies.DataList);
        }

        [HttpGet]
        [Route("GetMoviesSortedByYear")]
        public async Task<ActionResult> GetMoviesSortedByYear()
        {

            var movies = await _movieService.GetAllMoviesBySortedYear();
            return Ok(movies);
        }

        [HttpGet]
        [Route("GetTopTenMoviesByYear/{year}")]
        public async Task<ActionResult> GetTopTenMoviesBySpecificYear(int year)
        {

            var movies = await _movieService.GetTopTenMoviesBySpecificYear(year);
            return Ok(movies.DataList);
        }

        [HttpGet]
        [Route("byauditoriumid/{auditoriumId:int}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> GetByAuditoriumIdAsync(int auditoriumId)
        {
            var movies = await _movieService.GetMoviesByAuditoriumId(auditoriumId);
            if (!movies.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = movies.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };

                return NotFound(errorResponse);
            }
    
            return Ok(movies.DataList);
        }


        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> CreateMovieAsync([FromBody] CreateMovieModel movieModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MovieDomainModel domainModel = new MovieDomainModel
            {
                Current = movieModel.Current,
                Rating = movieModel.Rating,
                Title = movieModel.Title,
                Year = movieModel.Year,
                Genre = movieModel.Genre,
                CoverPicture = movieModel.CoverPicture,
                Tags = movieModel.Tags.Split(","),
                Actors = movieModel.Actors.Split(","),
                //hadKodovano
                HasOscar = false,
                Imdb= movieModel.ImdbId,
                Description= movieModel.Description
            };

            GenericResult<MovieDomainModel> createMovie;

            try
            {
                createMovie = await _movieService.AddMovieAsync(domainModel);
                
              /*  
              if(createMovie.IsSuccessful)
                {
                    // sada ovde ubacujem i actore!!!
                    _movieService.AddTagsForMovie(createMovie.Data);
                }*/
                
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

            if (!createMovie.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createMovie.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return BadRequest(errorResponse);
            }
              return CreatedAtAction("GetById", new { Id= createMovie.Data.Id }, createMovie.Data);
            
        }

        
        //[Authorize(Roles = "admin")]
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> UpdateMovieAsync(Guid id, [FromBody] UpdateMovieModel movieModel)
        {
            if(id == Guid.Empty)
            {
                return BadRequest(Messages.MOVIE_DOES_NOT_EXIST);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResult<MovieDomainModel> movieToUpdate;

            movieToUpdate = await _movieService.GetMovieByIdAsync(id);

            if (!movieToUpdate.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = movieToUpdate.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            movieToUpdate.Data.Title = movieModel.Title;
            movieToUpdate.Data.Current = movieModel.Current;
            movieToUpdate.Data.Year = movieModel.Year;
            movieToUpdate.Data.Rating = movieModel.Rating;
            //movieToUpdate.Data.HasOscar = movieModel.HasOscar;

            GenericResult<MovieDomainModel> movieDomainModel;
            try
            {
                movieDomainModel =  _movieService.UpdateMovie(movieToUpdate.Data);
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

            return Accepted();

        }


        //[Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> Delete(Guid id)
        {

            if (id == Guid.Empty)
            {
                return BadRequest(Messages.MOVIE_DELETE_ERROR);
            }
            GenericResult<MovieDomainModel> deletedMovie;

            try
            {
                deletedMovie = await _movieService.DeleteMovieAsync(id);
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

            if (!deletedMovie.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = deletedMovie.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Accepted();
        }

        [HttpPost]
        [Route("ActivateMovie/{id}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> ActivateMovie(Guid id)
        {
            if (id == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
           
            var movieActivated =await _movieService.ActivateDeactivateMovie(id);

            if(! movieActivated.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = movieActivated.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            return Accepted();
        }

        [HttpGet]
        [Route("SearchMoviesByTag/")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> SearchMoviesByTags( [FromQuery]string query)
        {
            GenericResult<MovieDomainModel> movies;
            try
            {
                movies = await _movieService.SearchMoviesByTag(query);

            }
            catch (Exception e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!movies.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = movies.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
             
         return Ok(movies.DataList);

        }

        [HttpGet]
        [Route("byCineamId/{cineamId:int}")]
        public async Task<ActionResult<GenericResult<MovieDomainModel>>> GetByCinemaIdAsync(int cineamId)
        {
            var movies = await _movieService.GetMoviesByCinemaId(cineamId);
            if (!movies.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = movies.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };

                return NotFound(errorResponse);
            }

            return Ok(movies.DataList);
        }
    }
}

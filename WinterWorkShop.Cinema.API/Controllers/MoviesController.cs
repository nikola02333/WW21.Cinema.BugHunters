using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        [Route("GetById/{id}")]
        public async Task<ActionResult<MovieDomainModel>> GetByIdAsync(Guid id)
        {
            GenericResult<MovieDomainModel> movie;

            movie = await _movieService.GetMovieByIdAsync(id);

            if (!movie.IsSuccessful)
            {
                return NotFound(Messages.MOVIE_DOES_NOT_EXIST);
            }

            return Ok(movie.Data);
        }

        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllAsync()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = await _movieService.GetAllMoviesAsync(true);

           
            return Ok(movieDomainModels);
        }

     
        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CreateMovieAsync([FromBody] CreateMovieModel movieModel)
        {

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Current = movieModel.Current,
                Rating = movieModel.Rating,
                Title = movieModel.Title,
                Year = movieModel.Year,
                Genre= movieModel.Genre
            };

            GenericResult<MovieDomainModel> createMovie;

            try
            {
                createMovie = await _movieService.AddMovieAsync(domainModel);
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

            if (createMovie == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
              return CreatedAtAction("GetById", new { Id= createMovie.Data.Id }, createMovie.Data);
            
        }

        
        //[Authorize(Roles = "admin")]
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] CreateMovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResult<MovieDomainModel> movieToUpdate;

            movieToUpdate = await _movieService.GetMovieByIdAsync(id);

            if (movieToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            movieToUpdate.Data.Title = movieModel.Title;
            movieToUpdate.Data.Current = movieModel.Current;
            movieToUpdate.Data.Year = movieModel.Year;
            movieToUpdate.Data.Rating = movieModel.Rating;
            movieToUpdate.Data.Genre = movieModel.Genre;

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
        public async Task<ActionResult> Delete(Guid id)
        {
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

            if (deletedMovie == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted();
        }
    }
}

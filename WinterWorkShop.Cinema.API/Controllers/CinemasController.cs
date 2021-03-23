using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CinemasController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        private readonly IAuditoriumService _auditoriumService;
        public CinemasController(ICinemaService cinemaService,IAuditoriumService auditoriumService)
        {
            _cinemaService = cinemaService;
            _auditoriumService = auditoriumService;
        }

        /// <summary>
        /// Gets all cinemas
        /// </summary>
        /// <returns>List of cinemas</returns>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<GenericResult<CinemaDomainModel>>> GetAsync()
        {
           GenericResult<CinemaDomainModel> result;

            result = await _cinemaService.GetAllAsync();

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(result.DataList);
        }

        [HttpGet]
        [Route("GetById/{id:int}", Name = nameof(GetCinemaById))]

        public async Task<ActionResult<CinemaDomainModel>> GetCinemaById(int id)
        {
            var response =await _cinemaService.GetCinemaById(id);
            if (!response.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = response.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCinemaAsync([FromBody] CreateCinemaModel cinema)
        {
            CinemaDomainModel cinemaModel = new CinemaDomainModel
            {
                Address = cinema.Address,
                CityName = cinema.CityName,
                Name = cinema.Name
            };
           
            var insertedCinema = await _cinemaService.AddCinemaAsync(cinemaModel);
            if (!insertedCinema.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = insertedCinema.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (cinema.createAuditoriumModel != null)

            {
                foreach (var audit in cinema.createAuditoriumModel)
                {
                    var auditoriumDomainModel = new AuditoriumDomainModel
                    {
                        CinemaId = insertedCinema.Data.Id,
                        Name = audit.auditName

                    };

                    var auditorium = await _auditoriumService.CreateAuditorium(auditoriumDomainModel, audit.seatRows, audit.numberOfSeats);

                    if (!auditorium.IsSuccessful)
                    {
                        ErrorResponseModel errorResponse = new ErrorResponseModel
                        {
                            ErrorMessage = insertedCinema.ErrorMessage,
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        };

                        return BadRequest(errorResponse);
                    }
                }
            }         
            return CreatedAtAction(nameof(GetCinemaById),
                new { Id = insertedCinema.Data.Id },
                insertedCinema.DataList);
        }


        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> DeleteCinema(int id)
        {

            if (id.GetType() !=typeof(int))
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = "Error",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            var result = await _cinemaService.DeleteCinemaAsync(id);

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            return Accepted(result.Data);       
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateCinema(int id, [FromBody] CinemaDomainModel updatedMovie)
        {
            updatedMovie.Id = id;

            var result = await _cinemaService.UpdateCinema(updatedMovie);
            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            return Accepted(result.Data);
        }
    }
}

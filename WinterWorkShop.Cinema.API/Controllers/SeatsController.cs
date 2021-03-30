using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        /// <summary>
        /// Gets all seats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GenericResult<SeatDomainModel>>> GetAllAsync()
        {
            GenericResult<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetAllAsync();

            return Ok(seatDomainModels.DataList);
        }

        [HttpGet]
        [Route("reservedByProjectionId/{id:guid}")]
        public async Task<ActionResult<GenericResult<SeatDomainModel>>> GetReservedSeatsAsync(Guid id)
        {

            if (id == null || id == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.SEAT_ID_NULL,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            GenericResult<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetReservedSeatsAsync(id);

            if (!seatDomainModels.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = seatDomainModels.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(seatDomainModels.DataList);
        }

        [HttpGet]
        [Route("byAuditoriumId/{id:int}")]
        public async Task<ActionResult<GenericResult<SeatDomainModel>>> GetAllByAuditoriumIdAsync(int id)
        {
            GenericResult<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetByAuditoriumIdAsync(id);

            if (!seatDomainModels.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = seatDomainModels.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(seatDomainModels.DataList);
        }

        [HttpGet]
        [Route("maxNumberOfSeatsByAuditoriumId/{id:int}")]
        public async Task<ActionResult<GenericResult<SeatsMaxNumbersDomainModel>>> GetMaxNumbersOfSeatsAsync(int id)
        {
            GenericResult<SeatsMaxNumbersDomainModel> dimensionsOfAuditoriumSeats;

            dimensionsOfAuditoriumSeats = await _seatService.GetMaxNumbersOfSeatsByAuditoriumIdAsync(id);

            if (!dimensionsOfAuditoriumSeats.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = dimensionsOfAuditoriumSeats.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(dimensionsOfAuditoriumSeats.Data);
        }
    }
}

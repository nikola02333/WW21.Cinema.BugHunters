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
    //[Authorize]
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
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAllAsync()
        {
            GenericResult<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetAllAsync();

            return Ok(seatDomainModels.DataList);
        }

        [HttpGet]
        [Route("reservedByProjectionId/{id:guid}")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetReservedSeatsAsync(Guid id)
        {
            GenericResult<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.ReservedSeatsAsync(id);

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
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAllByAuditoriumIdAsync(int id)
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
    }
}

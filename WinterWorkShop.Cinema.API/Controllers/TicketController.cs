using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<TicketDomainModel>>> GetAsync()
        {
            GenericResult<TicketDomainModel> ticketDomainModels;

            ticketDomainModels = await _ticketService.GetAllAsync();

            return Ok(ticketDomainModels.DataList);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<GenericResult<TicketDomainModel>>> GetByIdAsync(Guid id)
        {
            GenericResult<TicketDomainModel> ticketDomainModels;

            ticketDomainModels = await _ticketService.GetTicketByIdAsync(id);

            if (!ticketDomainModels.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = ticketDomainModels.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(ticketDomainModels);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<TicketDomainModel>>> PostAsync([FromBody] Models.CreateTicketModel ticketModel)
        {
            Domain.Models.CreateTicketModel ticket = new Domain.Models.CreateTicketModel
            {
                ProjectionId=ticketModel.ProjectionId,
                SeatId=ticketModel.SeatId,
                UserId=ticketModel.UserId
            };

            GenericResult<TicketDomainModel> createTicket;
            try
            {
               createTicket = await _ticketService.CreateTicketAsync(ticket);
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

            if (!createTicket.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createTicket.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
            return CreatedAtAction("GetById", new { Id = createTicket.Data.Id }, createTicket.Data);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            GenericResult<TicketDomainModel> deletedticket;

            try
            {
                deletedticket = await _ticketService.DeleteTicketAsync(id);
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

            if (!deletedticket.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = deletedticket.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted();
        }
    }
}

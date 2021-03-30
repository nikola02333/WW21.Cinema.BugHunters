using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var ticketDomainModels = await _ticketService.GetAllAsync();

            return Ok(ticketDomainModels.DataList);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<GenericResult<TicketDomainModel>>> GetByIdAsync(Guid id)
        {
            

           var ticketDomainModels = await _ticketService.GetTicketByIdAsync(id);

            if (!ticketDomainModels.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = ticketDomainModels.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(ticketDomainModels.Data);
        }

        [HttpGet]
        [Route("GetByUserId/{id:guid}")]
        public async Task<ActionResult<GenericResult<TicketDomainModel>>> GetByUserIdAsync(Guid id)
        {

            var ticketDomainModels = await _ticketService.GetTicketByUserIdAsync(id);

            if (!ticketDomainModels.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = ticketDomainModels.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(ticketDomainModels.DataList);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<GenericResult<TicketDomainModel>>> CreateTicketAsync([FromBody]CreateTicketModel ticketModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateTicketDomainModel ticket = new CreateTicketDomainModel
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

                return BadRequest(errorResponse);
            }
            return CreatedAtAction("GetById", new { Id = createTicket.DataList[0].Id }, createTicket.DataList);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<TicketDomainModel>> DeleteTicketAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.TICKET_ID_NULL,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

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

                return BadRequest(errorResponse);
            }

            return Accepted();
        }
    }
}

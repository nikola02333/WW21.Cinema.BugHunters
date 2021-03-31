using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriumsController : ControllerBase
    {
        private readonly IAuditoriumService _auditoriumService;

        public AuditoriumsController(IAuditoriumService auditoriumservice)
        {
            _auditoriumService = auditoriumservice;
        }

        /// <summary>
        /// Gets all auditoriums
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<AuditoriumDomainModel>>> GetAllAuditoriumsAsync()
        {
            IEnumerable<AuditoriumDomainModel> auditoriumDomainModels;

            auditoriumDomainModels = await _auditoriumService.GetAllAsync();

            if (auditoriumDomainModels == null)
            {
                auditoriumDomainModels = new List<AuditoriumDomainModel>();
            }

            return Ok(auditoriumDomainModels);
        }

        /// <summary>
        /// Adds a new auditorium
        /// </summary>
        /// <param name="createAuditoriumModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<AuditoriumDomainModel>> CreateAuditoriumAsync(CreateAuditoriumModel createAuditoriumModel) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel
            {
                CinemaId = createAuditoriumModel.cinemaId,
                Name = createAuditoriumModel.auditoriumName
                
            };

             GenericResult<AuditoriumDomainModel> createAuditoriumResultModel;

            try 
            {
                createAuditoriumResultModel = await _auditoriumService.CreateAuditorium(auditoriumDomainModel, createAuditoriumModel.numberOfSeats, createAuditoriumModel.seatRows);
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

            if (!createAuditoriumResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createAuditoriumResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            
            return Created("auditoriums//" + createAuditoriumResultModel.Data.Id, createAuditoriumResultModel.Data);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<GenericResult<AuditoriumDomainModel>>> GetById(int id)
        {
            var response = await _auditoriumService.GetByIdAsync(id);
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


        [HttpGet]
        [Route("ByCinemaId/{id}")]
        public async Task<ActionResult<GenericResult<AuditoriumDomainModel>>> GetAllByCinemaId(int id)
        {
            var response = await _auditoriumService.GetAllAuditoriumByCinemaId(id);

            if (!response.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = response.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(response.DataList);
        }



        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<GenericResult<AuditoriumDomainModel>>> Delete(int id)
        {
            var result= await _auditoriumService.DeleteAsync(id);

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };


                return BadRequest(errorResponse);
            }

            return Ok(result.Data);
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<ActionResult> UpdateAuditorium(int id, [FromBody] AuditoriumToUpdateModel auditorium )
        {
          
            var result = await _auditoriumService.UpdateAuditorium(id, auditorium);
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
﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
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
        [Route("GetAll")]
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
                Name = createAuditoriumModel.auditName
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
            
            return Created("auditoriums//" + createAuditoriumResultModel.Data.Id, createAuditoriumResultModel);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<GenericResult<AuditoriumDomainModel>>> GetById(int id)
        {
            var response = await _auditoriumService.GetByIdAsync(id);
            if (!response.IsSuccessful)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<GenericResult<AuditoriumDomainModel>>> Delete(int id)
        {
            var result= await _auditoriumService.DeleteAsync(id);

            return Ok(result.Data);
        }


       
    }
}
﻿using Microsoft.AspNetCore.Authorization;
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

        public CinemasController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        /// <summary>
        /// Gets all cinemas
        /// </summary>
        /// <returns>List of cinemas</returns>
        [HttpGet]
        public async Task<ActionResult<GenericResult<CinemaDomainModel>>> GetAsync()
        {
           GenericResult<CinemaDomainModel> result;

            result = await _cinemaService.GetAllAsync();

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.DataList);
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetCinemaById))]

        public async Task<ActionResult<CinemaDomainModel>> GetCinemaById(int id)
        {
            var response =await _cinemaService.GetCinemaById(id);
            if (!response.IsSuccessful)
            {
                return NotFound(response.ErrorMessage);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] CinemaDomainModel cinema)
        {

           
            var insertedCinema = await _cinemaService.AddCinemaAsync(cinema);
            if (!insertedCinema.IsSuccessful)
            {
                return BadRequest(insertedCinema.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCinemaById),
                new { Id = insertedCinema.Data.Id },
                insertedCinema.DataList);
        }


        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            
            var result = _cinemaService.DeleteCinema(id);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }


            return Accepted(result.Data);
        
        }

    }
}

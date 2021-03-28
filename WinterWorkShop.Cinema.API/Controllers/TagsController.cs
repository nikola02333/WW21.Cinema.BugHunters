﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITtagsMoviesService _tagsMoviesService;

        public TagsController(ITtagsMoviesService tagService)
        {
            _tagsMoviesService = tagService;
        }

        /*
          [Route("GetById/{id}")]
        public async Task<ActionResult<GenericResult<UserDomainModel>>> GetbyIdAsync(Guid id)
        {
         */
        [HttpGet]
        [Route("GetTagByMovieId/{movieId}")]
        public async Task<ActionResult<GenericResult<TagMovieDomainModel>>> GetTagByMovieId( Guid movieId)
        {
            var tagsByMovieId =await _tagsMoviesService.GetTagByMovieIDAsync(movieId);
            
            if(!tagsByMovieId.IsSuccessful)
            { 
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.GET_TAGS_BY_MOVIEID_NOT_FOUND,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                 
                return BadRequest(errorResponse);
            }

            return Ok(tagsByMovieId.DataList);
        }
    }
}
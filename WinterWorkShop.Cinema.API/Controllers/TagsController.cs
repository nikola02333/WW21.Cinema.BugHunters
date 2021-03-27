using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinterWorkShop.Cinema.Domain.Interfaces;
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
        public async Task<ActionResult> GetTagByMovieId( Guid movieId)
        {
            var tagsByMovieId =await _tagsMoviesService.GetTagByMovieIDAsync(movieId);
            return null;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class TagsMoviesServiceTests
    {

        private Mock<ITagsMoviesRepository> _mockTagsMoviesRepository;
        private TagsMoviesService _tagsMoviesService;
        Movie _movie;
        Tag _tag;
        TagsMovies _tagsMovies;
       [TestInitialize]
        public void TestInit()
        {
            _mockTagsMoviesRepository = new Mock<ITagsMoviesRepository>();
            _tagsMoviesService = new TagsMoviesService(_mockTagsMoviesRepository.Object);

            _movie = new Movie
            {
                Title = "Movie",
                Current = true,
                UserRaitings = 8,
                Year = 2019,
                CoverPicture = "www.google.rs",
                Genre = "comedy",
                HasOscar = true,
                Rating = 9,
                Id = new Guid("0403c450-7426-4755-8c12-1b310c9c5134"),
            };
            _tagsMovies = new TagsMovies
            {
                Movie = _movie,
                MovieId = _movie.Id,
                TagId = _tag.TagId,
                Tag = _tag
            };
            _tag = new Tag
            {
                TagId = 1,
                TagValue = "some value",
                TagName = "tag name",

                TagsMovies = new List<TagsMovies>
                {
                   new TagsMovies
                   {
                     Movie=_movie,
                     MovieId=_movie.Id,
                     TagId=_tag.TagId,
                     Tag=_tag
                   }
                }
            };       
        }


        [TestMethod]
        public async Task AddTagMovie_Return_()
        { 
        
        }





    }
}

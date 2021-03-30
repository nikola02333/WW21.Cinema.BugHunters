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
     
       [TestInitialize]
        public void TestInit()
        {
            _mockTagsMoviesRepository = new Mock<ITagsMoviesRepository>();
            _tagsMoviesService = new TagsMoviesService(_mockTagsMoviesRepository.Object);
                  
        }

        [TestMethod]
        public async Task AddTagMovie_Return_Tags()
        {
            bool isSuccessful = true;

            Movie movie =new Movie
             {
              Current = false,
                Genre = "comedy",
                Id =new Guid("5e9963d0-52e8-4593-9658-df8839712cf7"), 
                Rating = 8, 
                Title = "New_Movie1",
                Year = 1999, 
                HasOscar=true, 
                CoverPicture="coverpicture"

            };
            Tag tag = new Tag
            {
             TagId=1,
             TagName="tagName",
              TagValue="tagValue"
 
            };
            var movieTagsToInsert = new TagsMovies
            {
                Movie = movie,
                MovieId = movie.Id,
                TagId = tag.TagId,
                Tag=tag
                 
            };

            _mockTagsMoviesRepository.Setup(stvc => stvc.InsertAsync(It.IsNotNull<TagsMovies>())).ReturnsAsync(movieTagsToInsert);
            var result = await _tagsMoviesService.AddTagMovieAsync(movieTagsToInsert);

            Assert.IsNotNull(result);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
            Assert.IsInstanceOfType(result,typeof(GenericResult<TagMovieDomainModel>));

        }

        [TestMethod]
        public async Task GetTagByMovieIDAsync_Return_Tags()
        {
            bool isSuccessful = true;

            Movie movie = new Movie
            {
                Current = false,
                Genre = "comedy",
                Id = new Guid("5e9963d0-52e8-4593-9658-df8839712cf7"),
                Rating = 8,
                Title = "New_Movie1",
                Year = 1999,
                HasOscar = true,
                CoverPicture = "coverpicture"

            };
            Tag tag = new Tag
            {
                TagId = 1,
                TagName = "tagName",
                TagValue = "tagValue"

            };
            var movieTagsToInsert = new TagsMovies
            {
                Movie = movie,
                MovieId = movie.Id,
                TagId = tag.TagId,
                Tag = tag

            };

            List<TagsMovies> listOfTags = new List<TagsMovies>();
            listOfTags.Add(movieTagsToInsert);
            _mockTagsMoviesRepository.Setup(stvc => stvc.GetTagByMovieIDAsync(movie.Id)).ReturnsAsync(listOfTags);
            var result = await _tagsMoviesService.GetTagByMovieIDAsync(movie.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagMovieDomainModel>));
        }



    }
}

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
    public class TagsServiceTests
    {
        private Mock<ITagsRepository> _mockTagsRepository;
        private TagService _tagsService;
        Movie _movie;
        Tag _tag;
      [TestInitialize]
        public void TestInit()
        {
            _mockTagsRepository = new Mock<ITagsRepository>();
            _tagsService = new TagService(_mockTagsRepository.Object);
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
            _tag = new Tag
            {
                TagId = 1,
                TagValue = "some value",
                TagName = "tag name",
                 
                TagsMovies =new List<TagsMovies>
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
        public async Task AddTagAsync_When_Tag_Exists_Return_ErrorMessage()
        {
            //Arrange
            bool isSuccessful = false;
            var expectedErrorMessage = "this tag alredy exists";
           
            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.AddTagAsync(It.IsNotNull<TagDomainModel>());

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(expectedErrorMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
        }


        [TestMethod]
        public async Task AddTagAsync_When_Tag_Is_Null_Return_Tags()
        {
            //Arrange
            bool isSuccessful = true;
            _tag = null;         
            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.AddTagAsync(It.IsNotNull<TagDomainModel>());

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
        }

        [TestMethod]
        public async Task DeleteTagAsync_When_Tag_Doesnt_Exist_Return_ErrorMessage()
        {
            //Arrange
            bool isSuccessful = false;
            var expectedErrorMessage = Messages.TAG_DOES_NOT_EXIST;
            _tag = null;

            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.DeleteTagAsync(It.IsNotNull<TagDomainModel>());

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(expectedErrorMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);

        }

        [TestMethod]
        public async Task DeleteTagAsync_Returns_Successful()
        {
            //Arrange
            bool isSuccessful = true;
          
            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(_tag);
            _mockTagsRepository.Setup(repo => repo.Delete(_tag.TagId));

            //Act
            var result = await _tagsService.DeleteTagAsync(It.IsNotNull<TagDomainModel>());

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(isSuccessful, result.IsSuccessful);

        }


        [TestMethod]
        public async Task GetTagByName_If_Tag_Doesnt_Exist_Return_ErrorMassage()
        {
            //Arrange
            bool isSuccessful = false;
            var expectedErrorMessage = Messages.TAG_DOES_NOT_EXIST;
            _tag = null;

            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.GetTagByName(It.IsNotNull<string>());

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(expectedErrorMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);

        }

        [TestMethod]
        public async Task GetTagByName_Return_Tag()
        {
            //Arrange
            bool isSuccessful = true;

            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.GetTagByName(It.IsNotNull<string>());

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(isSuccessful, result.IsSuccessful);

        }

    }
}


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class TagsServiceTests
    {
        private Mock<ITagsRepository> _mockTagsRepository;
        private TagService _tagsService;
    
        [TestInitialize]
        public void TestInit()
        {
            _mockTagsRepository = new Mock<ITagsRepository>();
            _tagsService = new TagService(_mockTagsRepository.Object);
          
        }

        [TestMethod]
        public async Task AddTagAsync_When_Tag_Exists_Return_ErrorMessage()
        {
            //Arrange
            Tag _tag = new Tag
            {
                TagId = 1,
                TagValue = "some value",
                TagName = "tag name",
            };
            TagDomainModel _tagModel = new TagDomainModel
            {
                TagValue = _tag.TagValue,
                TagName = _tag.TagName,
                TagId = _tag.TagId
            };
            bool isSuccessful = false;
            var expectedErrorMessage = "this tag alredy exists";
            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(_tag.TagName)).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.AddTagAsync(_tagModel);

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(expectedErrorMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
        }

        [TestMethod]
        public async Task AddTagAsync_When_Tag_Is_Null_Return_Tag()
        {
            //Arrange
            bool isSuccessful = true;

           
            TagDomainModel _tagModel =new TagDomainModel
            { 
              TagId=2,
              TagName="name",
               TagValue="value"
            };
         
            Tag _tag = new Tag
            {
                TagId = _tagModel.TagId,
                TagName = _tagModel.TagName,
                TagValue = _tagModel.TagValue
            };

        
           
            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(default(Tag));
            _mockTagsRepository.Setup(repo => repo.InsertAsync(It.IsNotNull<Tag>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.AddTagAsync(_tagModel);
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
            Tag _tag = null;        
            
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
            //Arrange
            Tag _tag = new Tag
            {
                TagId = 1,
                TagValue = "some value",
                TagName = "tag name",
            };
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
            Tag _tag = null;    

            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(_tag);

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
            Tag _tag = new Tag
            {
                TagId = 1,
                TagValue = "some value",
                TagName = "tag name",
            };

            _mockTagsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(_tag);

            //Act
            var result = await _tagsService.GetTagByName(It.IsNotNull<string>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<TagDomainModel>));
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
        }
    }
}

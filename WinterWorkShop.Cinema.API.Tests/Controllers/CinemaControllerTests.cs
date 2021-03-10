using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class CinemaControllerTests
    {

        private Mock<ICinemaService> _mockCinemaService;

        private CinemasController _cinemaController;

        private Mock<IAuditoriumService> _mockAuditoriumService;
        [TestInitialize]

        public void TestInit()
        {
            _mockAuditoriumService = new Mock<IAuditoriumService>();

               _mockCinemaService = new Mock<ICinemaService>();
            _cinemaController = new CinemasController(_mockCinemaService.Object, _mockAuditoriumService.Object);


        }


        [TestMethod]

        public async Task GetCinemas_Return_All_Cinemas()
        {
            //Arrange
            int expectedStatusCode = 200;
           

            var expectedCinemas = new GenericResult<CinemaDomainModel>
            {

                IsSuccessful=true,
                DataList = new List<CinemaDomainModel>
                {
                    new CinemaDomainModel() { Id = 1,Address="Perleska 67",CityName="New York",Name="Cinemax"},
                    new CinemaDomainModel() { Id = 2,Address="Perleska 167",CityName="Paris",Name="Cineplex" }
                }
            };


            _mockCinemaService.Setup(srvc => srvc.GetAllAsync())
                .ReturnsAsync(expectedCinemas);

            // Act

            var result = await _cinemaController.GetAsync();


            var cinemaResult = ((OkObjectResult)result.Result).Value;
           


            //Assert
            Assert.IsNotNull(cinemaResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
           Assert.AreSame(expectedCinemas.DataList, cinemaResult);
           Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
           
        }

        [TestMethod]
        public async Task GetCinemas_Return_New_List()
        {
            int expectedStatusCode = 200;

            var expectedCinemas = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful=true,
                DataList = new List<CinemaDomainModel>
                {
                }
            };


            _mockCinemaService.Setup(srvc => srvc.GetAllAsync())
                .ReturnsAsync(expectedCinemas);

            // Act

            var result = await _cinemaController.GetAsync();


            var cinemaResult = ((OkObjectResult)result.Result).Value;


            Assert.IsNotNull(cinemaResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedCinemas.DataList, cinemaResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }


        [TestMethod]
        public async Task GetCinemaById_If_Id_Exists_Returns_User()
        {
            int expectedStatusCode = 200;
            int expectedCinemaId = 1;

            var expectedCinema = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = new CinemaDomainModel
                {
                    Id = expectedCinemaId,
                    Address = "Perleska 167",
                    CityName = "Paris",
                    Name = "Cineplex"
                }
            };


            _mockCinemaService.Setup(srvc => srvc.GetCinemaById(expectedCinemaId))
                .ReturnsAsync(expectedCinema);

            // Act

            var result = await _cinemaController.GetCinemaById(expectedCinemaId);

            //Assert
            var cinemaResult = ((OkObjectResult)result.Result).Value;


            Assert.IsNotNull(cinemaResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedCinema.Data, cinemaResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod]
        public async Task GetCinemaByID_When_Id_Is_Not_Right_Returns_BadRequest()
        {
            int expectedStatusCode = 400;

            int expectedId = 0;

            var expectedErrorMessage = "Cinema doesn't exist";

            var expectedCinema = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = "Cinema doesn't exist"
            };


            _mockCinemaService.Setup(srvc => srvc.GetCinemaById(expectedId))
                .ReturnsAsync(expectedCinema);

            // Act

            var result = await _cinemaController.GetCinemaById(expectedId);

            //Assert
            var cinemaResult = ((BadRequestObjectResult)result.Result).Value;

            Assert.IsNotNull(cinemaResult);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);

            var error = (ErrorResponseModel)cinemaResult;
            Assert.AreEqual(expectedErrorMessage, error.ErrorMessage);
        }

        [TestMethod]
        public async Task AddCinemaAsync_When_IsSuccesfull_Is_False_Returns_BadRrquest()
        {
            string expectedMessage = "Cinema alreday exists";
            int expectedStatusCode = 400;

            GenericResult<CinemaDomainModel> CreateCinemaResponseModel = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = "Cinema alreday exists"
            };

        
            var cinemaToCreate = new CreateCinemaModel
            {
              
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex"
               
            };

            _mockCinemaService.Setup(srvc => srvc.AddCinemaAsync(It.IsAny<CinemaDomainModel>()))
                             .ReturnsAsync(CreateCinemaResponseModel);
            // Act
            var result = await _cinemaController.CreateCinemaAsync(cinemaToCreate);



            var badObjectResult = ((BadRequestObjectResult)result).Value;


            var errorStatusCode = (BadRequestObjectResult)result;

            var errorResult = (ErrorResponseModel)badObjectResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);

            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task CreateCinemaAsync_When_Is_IsSuccessful_True_Returns_User()
        {
            int expectedStatusCode = 201;

           
            var cinemaToCreate = new CreateCinemaModel
            {

                
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex"
            };


            GenericResult<CinemaDomainModel> CreateCinemaResponseModel = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = new CinemaDomainModel
                {
                  
                    Address = cinemaToCreate.Address,
                    CityName = cinemaToCreate.CityName,
                    Name = cinemaToCreate.Name,
                  
                }
            };

            _mockCinemaService.Setup(srvc => srvc.AddCinemaAsync(It.IsAny<CinemaDomainModel>()))
                             .ReturnsAsync(CreateCinemaResponseModel);

            // Act
            var result = await _cinemaController.CreateCinemaAsync(cinemaToCreate);


            var resultResponse = (CreatedAtActionResult)result;
            var cinemaCreated = resultResponse.Value;

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }


      

        [TestMethod]
        public async Task DeleteCinema_When_Called_Returns_Accepted()
        {
            int expectedStatusCode = 202;

            var cinemaId = default(int);

            var DeletedCinema = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = new CinemaDomainModel()
                {
                    Id = cinemaId
                }
            };
           

            _mockCinemaService.Setup(srvc => srvc.DeleteCinema(cinemaId))
                             .Returns(DeletedCinema);

            // Act
            var result = _cinemaController.DeleteCinema(cinemaId);


            var resultResponse = (AcceptedResult)result;

            //Assert
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));


            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
      
        [TestMethod]
        public async Task UpdateCinema_When_IsSuccessful_False_Returns_BadRequest()
        {
            int expectedStatusCode = 400;
            var expectedMessage = "Cinema not found";
            var userId = 1;


            GenericResult<CinemaDomainModel> UpdateCinemaResponseModel = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = "Cinema not found"
            };

            var cinemaToUpdate = new CinemaDomainModel
            {
                Id = userId,
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex"

            };

            // Act
            _mockCinemaService.Setup(srvc => srvc.UpdateCinema(It.IsAny<CinemaDomainModel>()))
                              .ReturnsAsync(UpdateCinemaResponseModel);
            // Act
            var result = await _cinemaController.UpdateCinema(userId,cinemaToUpdate);



            var badObjectResult = ((BadRequestObjectResult)result).Value;


            var errorStatusCode = (BadRequestObjectResult)result;

            var errorResult = (ErrorResponseModel)badObjectResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);

            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task UpdateUser_When_Called_Returns_Accepted()
        {
            int expectedStatusCode = 202;
            var userId = 1;


            var updatedCinemaResult = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
            };

            var cinemaToUpdate = new CinemaDomainModel
            {
                Id = userId,
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex"

            };


            _mockCinemaService.Setup(srvc => srvc.UpdateCinema(It.IsAny<CinemaDomainModel>()))
                            .ReturnsAsync(updatedCinemaResult);

            // Act
            var result = await _cinemaController.UpdateCinema(userId,cinemaToUpdate);

            var usersResult = ((AcceptedResult)result).Value;


            //Assert
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));

        }

    }
}

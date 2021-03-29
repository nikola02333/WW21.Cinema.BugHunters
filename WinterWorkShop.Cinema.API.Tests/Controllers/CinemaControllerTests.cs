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

        public async Task GetAllCinemas_Returns_All_Cinemas()
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
        public async Task GetAllCinemas_Returns_New_List()
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

            //Assert
            Assert.IsNotNull(cinemaResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedCinemas.DataList, cinemaResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }


        [TestMethod]
        public async Task GetCinemaById_If_Id_Exists_Returns_Cinema()
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
            var cinemaResult = ((OkObjectResult)result.Result).Value;

            //Assert
            Assert.IsNotNull(cinemaResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedCinema.Data, cinemaResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod]
        public async Task GetCinemaById_When_Id_Is_Not_Right_Returns_BadRequest()
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
            var cinemaResult = ((BadRequestObjectResult)result.Result).Value;
            var error = (ErrorResponseModel)cinemaResult;

            //Assert
            Assert.IsNotNull(cinemaResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);         
            Assert.AreEqual(expectedErrorMessage, error.ErrorMessage);
        }

        [TestMethod]
        public async Task CreateCinemaAsync_When_IsSuccesfull_Is_False_Returns_Bad_Request()
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
                Name = "Cineplex",
                createAuditoriumModel = new List<CreateAuditoriumModel>
                {
                    new CreateAuditoriumModel{
                    auditoriumName = "New Auditorium",
                    numberOfSeats = 2,
                    seatRows = 2
                    }
                }
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
        public async Task CreateCinemaAsync_When_Is_IsSuccessful_True_Returns_Cinema()
        {
            int expectedStatusCode = 201;

            int cinemaumId = 1;
            var cinemaToCreate = new CreateCinemaModel
            {               
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex",

                createAuditoriumModel = new List<CreateAuditoriumModel>
                {
                    new CreateAuditoriumModel{
                    auditoriumName = "New Auditorium",
                    numberOfSeats = 2,
                    seatRows = 2
                    }
                }
            };
    
            GenericResult<CinemaDomainModel> CreateCinemaResponseModel = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data=new CinemaDomainModel
                {   
                    Id=cinemaumId,
                    Address = "Perleska 167",
                    CityName = "Paris",
                    Name = "Cineplex",
                }
            };

            GenericResult<AuditoriumDomainModel> CreateAuditoriumResponseModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                Data = new AuditoriumDomainModel
                {
                    Id = 1,
                    CinemaId = CreateCinemaResponseModel.Data.Id,
                    Name = cinemaToCreate.createAuditoriumModel[0].auditoriumName,
                   SeatsList = new List<SeatDomainModel>()
                }
            };

            var inserted= _mockCinemaService.Setup(srvc => srvc.AddCinemaAsync(It.IsNotNull<CinemaDomainModel>()))
                           .ReturnsAsync(CreateCinemaResponseModel);        
            _mockAuditoriumService.Setup(srvc => srvc.CreateAuditorium(It.IsNotNull<AuditoriumDomainModel>(), cinemaToCreate.createAuditoriumModel[0].seatRows, cinemaToCreate.createAuditoriumModel[0].numberOfSeats)).ReturnsAsync(CreateAuditoriumResponseModel);
         
            // Act
            var result = await _cinemaController.CreateCinemaAsync(cinemaToCreate);
            var resultResponse = (CreatedAtActionResult)result;
            var cinemaCreated = resultResponse.Value;

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
     
        [TestMethod]
        public async Task CreateCinemaAsync_When_Auditorium_Result_IsSuccessful_False_Returns_Bad_Request()
        {
            int expectedStatusCode = 400;

            int cinemaumId = 1;
            var cinemaToCreate = new CreateCinemaModel
            {
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex",
                createAuditoriumModel = new List<CreateAuditoriumModel>
                {
                    new CreateAuditoriumModel{
                    auditoriumName = "New Auditorium",
                    numberOfSeats = 2,
                    seatRows = 2
                    }
                }
            };

            GenericResult<CinemaDomainModel> CreateCinemaResponseModel = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = new CinemaDomainModel
                {
                    Id = cinemaumId,
                    Address = "Perleska 167",
                    CityName = "Paris",
                    Name = "Cineplex",
                }
            };

            GenericResult<AuditoriumDomainModel> CreateAuditoriumResponseModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false,
                Data = new AuditoriumDomainModel
                {
                    Id = 1,
                    CinemaId = CreateCinemaResponseModel.Data.Id,
                    Name = cinemaToCreate.createAuditoriumModel[0].auditoriumName,
                    SeatsList = new List<SeatDomainModel>()
                }
            };

            var inserted = _mockCinemaService.Setup(srvc => srvc.AddCinemaAsync(It.IsNotNull<CinemaDomainModel>()))
                           .ReturnsAsync(CreateCinemaResponseModel);
            _mockAuditoriumService.Setup(srvc => srvc.CreateAuditorium(It.IsNotNull<AuditoriumDomainModel>(), cinemaToCreate.createAuditoriumModel[0].seatRows, cinemaToCreate.createAuditoriumModel[0].numberOfSeats)).ReturnsAsync(CreateAuditoriumResponseModel);

            // Act
            var result = await _cinemaController.CreateCinemaAsync(cinemaToCreate);
            var resultResponse = (BadRequestObjectResult)result;
            var cinemaCreated = resultResponse;

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
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
                    Id = cinemaId,
                    Address = "Perleska 167",
                    CityName = "Paris",
                    Name = "Cineplex",
                }
            };
           

            _mockCinemaService.Setup(srvc => srvc.DeleteCinemaAsync(cinemaId))
                             .ReturnsAsync(DeletedCinema);

            // Act
            var result =await _cinemaController.DeleteCinema(cinemaId);
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
        public async Task UpdateCinema_When_Called_Returns_Accepted()
        {
            int expectedStatusCode = 202;
            var cinemaId = 1;

            var cinemaToUpdate = new CinemaDomainModel
            {
                Id = cinemaId,
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex"

            };

            var updatedCinemaResult = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = cinemaToUpdate
            };

            _mockCinemaService.Setup(srvc => srvc.UpdateCinema(cinemaToUpdate))
                            .ReturnsAsync(updatedCinemaResult);

            // Act
            var result = await _cinemaController.UpdateCinema(cinemaId,cinemaToUpdate);
            var cinemaResult = ((AcceptedResult)result);

            //Assert
            Assert.IsInstanceOfType(cinemaResult,typeof(AcceptedResult));

        }


        [TestMethod]
        public async Task UpdateCinema_When_Is_Successful_False_Returns_Bad_Request()
        {
            int expectedStatusCode = 400;
            var cinemaId = default(int);
            var expectedErrorMessage = "Cinema not found";
            var cinemaToUpdate = new CinemaDomainModel
            {
                Id = cinemaId,
                Address = "Perleska 167",
                CityName = "Paris",
                Name = "Cineplex"

            };

            var updatedCinemaResult = new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = "Cinema not found"
            };

            _mockCinemaService.Setup(srvc => srvc.UpdateCinema(cinemaToUpdate))
                            .ReturnsAsync(updatedCinemaResult);

            // Act
            var result = await _cinemaController.UpdateCinema(cinemaId, cinemaToUpdate);
            var cinemaResult = ((BadRequestObjectResult)result).Value;        
            var errorStatusCode = (ErrorResponseModel)cinemaResult;

            //Assert
            Assert.IsNotNull(cinemaResult);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(expectedErrorMessage, errorStatusCode.ErrorMessage);
        }

    }
}

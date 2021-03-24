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
    public class AuditoriumControllerTests
    {
        private AuditoriumsController _auditoriumController;

        private Mock<IAuditoriumService> _mockAuditoriumService;


        [TestInitialize]

        public void TestInit()
        {
            _mockAuditoriumService = new Mock<IAuditoriumService>();

            _auditoriumController = new AuditoriumsController(_mockAuditoriumService.Object);

        }


        [TestMethod]

        public async Task GetAllAuditoriums_Returns_All_Auditoriums()
        {
            //Arrange
            int expectedStatusCode = 200;

            var auditoriumsModel = new List<AuditoriumDomainModel>
            {

                    new AuditoriumDomainModel() { Id = 1, CinemaId=1, Name="New Auditorium", SeatsList=new List<SeatDomainModel>{ new SeatDomainModel { AuditoriumId=1, Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"), Number=2, Row=2 } } },
                    new AuditoriumDomainModel() { Id = 2, CinemaId=2, Name="New Auditorium", SeatsList=new List<SeatDomainModel>{ new SeatDomainModel { AuditoriumId=1, Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24457"), Number=2, Row=2 } } }

            };



            IEnumerable<AuditoriumDomainModel> _auditoriumsEnume = auditoriumsModel;

            var expectedAuditoriums = new GenericResult<AuditoriumDomainModel>
            {

                IsSuccessful = true,
                DataList = auditoriumsModel

            };


            _mockAuditoriumService.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(_auditoriumsEnume);

            // Act

            var result = await _auditoriumController.GetAllAuditoriumsAsync();


            var auditoriumResult = ((OkObjectResult)result.Result).Value;



            //Assert
            Assert.IsNotNull(auditoriumResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedAuditoriums.DataList, auditoriumResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }


        [TestMethod]

        public async Task GetAllAuditoriums_Returns__Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;

            var auditoriumsModel = new List<AuditoriumDomainModel>
            {



            };


            IEnumerable<AuditoriumDomainModel> _auditoriumsEnume = auditoriumsModel;

            var expectedAuditoriums = new GenericResult<AuditoriumDomainModel>
            {

                IsSuccessful = true,
                DataList = auditoriumsModel

            };


            _mockAuditoriumService.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(auditoriumsModel);

            // Act
            var result = await _auditoriumController.GetAllAuditoriumsAsync();
            var auditoriumResult = ((OkObjectResult)result.Result).Value;

            //Assert
            Assert.IsNotNull(auditoriumResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedAuditoriums.DataList, auditoriumResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }



        [TestMethod]

        public async Task DeleteAuditorium_Returns__OkObjectResult()
        {

            int expectedStatusCode = 200;

            var auditoriumModel = new AuditoriumDomainModel
            {

                Id = 1,
                CinemaId = 1,
                Name = "New Auditorium",
                SeatsList = new List<SeatDomainModel>
                { new SeatDomainModel
                { AuditoriumId=1,
                  Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                    Number=2, Row=2
                }
                }


            };

            var auditoriumToDelete = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                Data = auditoriumModel

            };

            _mockAuditoriumService.Setup(srvc => srvc.DeleteAsync(auditoriumModel.Id)).ReturnsAsync(auditoriumToDelete);

            var result = await _auditoriumController.Delete(auditoriumModel.Id);


            var auditoriumResult = ((OkObjectResult)result.Result).Value;

            //Assert
            Assert.IsNotNull(auditoriumResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(auditoriumToDelete.Data, auditoriumResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }


        [TestMethod]

        public async Task DeleteAuditorium_Returns__ErrorMessage()
        {

            int expectedStatusCode = 400;

            var auditoriumModel = new AuditoriumDomainModel
            {

                Id = 1,
                CinemaId = 1,
                Name = "New Auditorium",
                SeatsList = new List<SeatDomainModel>
                { new SeatDomainModel
                { AuditoriumId=1,
                  Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                    Number=2, Row=2
                }
                }

            };

            var auditoriumToDelete = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false

            };

            _mockAuditoriumService.Setup(srvc => srvc.DeleteAsync(auditoriumModel.Id)).ReturnsAsync(auditoriumToDelete);

            var result = await _auditoriumController.Delete(auditoriumModel.Id);

            var auditoriumResult = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsNotNull(auditoriumResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);

        }

        [TestMethod]

        public async Task GetById_Returns_Auditorium()
        {
            int expectedStatusCode = 200;

            var auditoriumModel = new AuditoriumDomainModel
            {

                Id = 1,
                CinemaId = 1,
                Name = "New Auditorium",
                SeatsList = new List<SeatDomainModel>
                { new SeatDomainModel
                { AuditoriumId=1,
                  Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                    Number=2, Row=2
                }
                }


            };

            var auditoriumToFind = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                Data = auditoriumModel

            };

            _mockAuditoriumService.Setup(srvc => srvc.GetByIdAsync(auditoriumModel.Id)).ReturnsAsync(auditoriumToFind);

            var result = await _auditoriumController.GetById(auditoriumModel.Id);


            var auditoriumResult = ((OkObjectResult)result.Result);

            //Assert  
            Assert.IsNotNull(auditoriumResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(auditoriumToFind.Data, auditoriumResult.Value);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }

        [TestMethod]

        public async Task GetById_Returns_BadRequest()
        {
            int expectedStatusCode = 400;

            var auditoriumModel = new AuditoriumDomainModel
            {

                Id = 1,
                CinemaId = 1,
                Name = "New Auditorium",
                SeatsList = new List<SeatDomainModel>
                { new SeatDomainModel
                { AuditoriumId=1,
                  Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                    Number=2, Row=2
                }
                }


            };

            var auditoriumToFind = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR

            };

            _mockAuditoriumService.Setup(srvc => srvc.GetByIdAsync(auditoriumModel.Id)).ReturnsAsync(auditoriumToFind);

            var result = await _auditoriumController.GetById(auditoriumModel.Id);


            var auditoriumResult = (BadRequestObjectResult)result.Result;


            //Assert  
            Assert.IsNotNull(auditoriumResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);

        }

        [TestMethod]

        public async Task GetByCinemaId_Returns_Auditoriums()
        {
            int expectedStatusCode = 200;
            int cinemaId = 2;
            var auditoriumsModel = new List<AuditoriumDomainModel>
            {

                    new AuditoriumDomainModel() { Id = 1, CinemaId=2, Name="New Auditorium", SeatsList=new List<SeatDomainModel>{ new SeatDomainModel { AuditoriumId=1, Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"), Number=2, Row=2 } } },
                    new AuditoriumDomainModel() { Id = 2, CinemaId=2, Name="New Auditorium", SeatsList=new List<SeatDomainModel>{ new SeatDomainModel { AuditoriumId=1, Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24457"), Number=2, Row=2 } } }

            };



            IEnumerable<AuditoriumDomainModel> _auditoriumsEnume = auditoriumsModel;


            var auditoriumToFind = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                DataList = auditoriumsModel

            };

            _mockAuditoriumService.Setup(srvc => srvc.GetAllAuditoriumByCinemaId(cinemaId)).ReturnsAsync(auditoriumToFind);

            var result = await _auditoriumController.GetAllByCinemaId(cinemaId);


            var auditoriumResult = ((OkObjectResult)result.Result);

            //Assert  
            Assert.IsNotNull(auditoriumResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }

        [TestMethod]

        public async Task GetByCinemaId_Returns_BadRequest()
        {
            int expectedStatusCode = 400;
            int cinemaId = 2;
            var auditoriumsModel = new List<AuditoriumDomainModel>
            {

                    new AuditoriumDomainModel() { Id = 1, CinemaId=2, Name="New Auditorium", SeatsList=new List<SeatDomainModel>{ new SeatDomainModel { AuditoriumId=1, Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"), Number=2, Row=2 } } },
                    new AuditoriumDomainModel() { Id = 2, CinemaId=2, Name="New Auditorium", SeatsList=new List<SeatDomainModel>{ new SeatDomainModel { AuditoriumId=1, Id= new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24457"), Number=2, Row=2 } } }

            };



            IEnumerable<AuditoriumDomainModel> _auditoriumsEnume = auditoriumsModel;


            var auditoriumToFind = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false
            };

            _mockAuditoriumService.Setup(srvc => srvc.GetAllAuditoriumByCinemaId(cinemaId)).ReturnsAsync(auditoriumToFind);

            var result = await _auditoriumController.GetAllByCinemaId(cinemaId);


            var auditoriumResult = ((BadRequestObjectResult)result.Result);

            //Assert  
            Assert.IsNotNull(auditoriumResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);

        }

        [TestMethod]
        public async Task CreateAuditoriumAsync_When_ModelState_Is_Invalid_Returns_BadRrquest()
        {
            int expectedStatusCode = 400;
            var expectedErrorMessage = "The name field is required.";
            var numOfSeats = 2;
            var numOfRows = 2;
          
            var auditoriumToCreate = new CreateAuditoriumModel
            {

                auditName = "Auditorium two",
                cinemaId = 2,
                numberOfSeats = numOfSeats,
                seatRows = numOfRows

            };

            // Act
            _auditoriumController.ModelState.AddModelError("Name", "The name field is required.");

            var result = await _auditoriumController.CreateAuditoriumAsync(auditoriumToCreate);
            var usersResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResponse = ((SerializableError)usersResult).GetValueOrDefault("Name");
            var message = (string[])errorResponse;
            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(message[0], expectedErrorMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task CreateAuditoriumAsync_When_IsSuccesfull_False_Returns_BadRequest()
        {
            string expectedMessage = "Error occured while creating new auditorium, please try again.";
            int expectedStatusCode = 400;
            var numOfSeats = 2;
            var numOfRows = 2;

            GenericResult<AuditoriumDomainModel> CreateUserResponseModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
            };

            var auditoriumToCreate = new CreateAuditoriumModel
            {
                auditName = "Auditorium two",
                cinemaId = 2,
                numberOfSeats = numOfSeats,
                seatRows = numOfRows
            };

            _mockAuditoriumService.Setup(srvc => srvc.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), auditoriumToCreate.numberOfSeats, auditoriumToCreate.seatRows))
                             .ReturnsAsync(CreateUserResponseModel);

            // Act
            var result = await _auditoriumController.CreateAuditoriumAsync(auditoriumToCreate);
            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;
            var errorStatusCode = (BadRequestObjectResult)result.Result;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task CreateAuditoriumAsync_When_Called_Handles_Exception_Returns_BadRequest()
        {
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            var numOfSeats = 2;
            var numOfRows = 2;
            int auditoriumId = 1;

            GenericResult<AuditoriumDomainModel> CreateUserResponseModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                Data = new AuditoriumDomainModel
                {
                    CinemaId = 2,
                    Id = auditoriumId,
                    Name = "Auditorium two",
                    SeatsList = new List<SeatDomainModel>
                {
                  new SeatDomainModel
                  {
                   AuditoriumId= auditoriumId,
                    Id=new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     Number=numOfSeats,
                      Row=numOfRows

                  }
                }

                }
            };

            var auditoriumToCreate = new CreateAuditoriumModel
            {
                auditName = "Auditorium two",
                cinemaId = 2,
                numberOfSeats = numOfSeats,
                seatRows = numOfRows
            };

            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);





            _mockAuditoriumService.Setup(srvc => srvc.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), auditoriumToCreate.numberOfSeats, auditoriumToCreate.seatRows))
                             .Throws(dbUpdateException);

            // Act
            var result = await _auditoriumController.CreateAuditoriumAsync(auditoriumToCreate);
            var resultResponse = (BadRequestObjectResult)result.Result;
            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public async Task CreateAuditoriumAsync_When_called_IsSuccessful_True_returns_AUditorium()
        {

            int expectedStatusCode = 201;
            var numOfSeats = 2;
            var numOfRows = 2;
            int auditoriumId = 1;

            GenericResult<AuditoriumDomainModel> CreateAuditoriumResponseModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                Data = new AuditoriumDomainModel
                {

                    CinemaId = 2,
                    Id = auditoriumId,
                    Name = "Auditorium two",
                    SeatsList = new List<SeatDomainModel>
                {
                  new SeatDomainModel
                  {
                   AuditoriumId= auditoriumId,
                    Id=new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     Number=numOfSeats,
                      Row=numOfRows

                  }
                }

                }
            };

            var auditoriumToCreate = new CreateAuditoriumModel
            {
                auditName = "Auditorium two",
                cinemaId = 2,
                numberOfSeats = numOfSeats,
                seatRows = numOfRows
            };

            _mockAuditoriumService.Setup(srvc => srvc.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), auditoriumToCreate.numberOfSeats, auditoriumToCreate.seatRows))
                             .ReturnsAsync(CreateAuditoriumResponseModel);

            // Act
            var result = await _auditoriumController.CreateAuditoriumAsync(auditoriumToCreate);
            var resultResponse = (CreatedResult)result.Result;
            var userCreated = resultResponse.Value;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedResult));
            Assert.AreEqual(userCreated, CreateAuditoriumResponseModel.Data);
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode); ;



        }
    }
}

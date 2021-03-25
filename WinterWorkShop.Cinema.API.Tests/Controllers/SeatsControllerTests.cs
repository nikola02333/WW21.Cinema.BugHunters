using Microsoft.AspNetCore.Mvc;
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
    public class SeatsControllerTests
    {
        private Mock<ISeatService> _mockSeatService;
        private SeatsController _seatsController;

        [TestInitialize]
        public void TestInit()
        {
            _mockSeatService = new Mock<ISeatService>();
            _seatsController = new SeatsController(_mockSeatService.Object);
        }

        [TestMethod]
        public async Task GetAllAsync_Return_All_Seats()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 2;
            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel>
                {
                    new SeatDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = 1,
                        Number = 1,
                        Row = 1
                    },
                    new SeatDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = 1,
                        Number = 2,
                        Row = 1
                    }
                }
            };

            _mockSeatService.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetAllAsync();
            var seatsResult = ((OkObjectResult)result.Result).Value;
            var resultRespoce = (OkObjectResult)result.Result;
            var seatsList = (List<SeatDomainModel>)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(resultRespoce.StatusCode, expectedStatusCode);
            Assert.AreEqual(seatsList.Count,expectedResultCount);
            Assert.AreEqual(expectedSeats.DataList, seatsResult);

        }

        [TestMethod]
        public async Task GetAllAsync_Return_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 0;

            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel> { }
            };

            _mockSeatService.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetAllAsync();
            var seatsResult = ((OkObjectResult)result.Result).Value;
            var resultRespoce = (OkObjectResult)result.Result;
            var seatsList = (List<SeatDomainModel>)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(resultRespoce.StatusCode, expectedStatusCode);
            Assert.AreEqual(seatsList.Count, expectedResultCount);
            Assert.AreEqual(expectedSeats.DataList, seatsResult);
        }

        [TestMethod]
        public async Task GetReservedSeatsAsync_When_Id_Exists_IsSuccessfull_True_Return_Reserved_Seats()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 2;
            var projectionId = Guid.NewGuid();
            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel>
                {
                    new SeatDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = 1,
                        Number = 1,
                        Row = 1
                    },
                    new SeatDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = 1,
                        Number = 2,
                        Row = 1
                    }
                }
            };

            _mockSeatService.Setup(srvc => srvc.GetReservedSeatsAsync(It.IsAny<Guid>())).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetReservedSeatsAsync(projectionId);
            var seatsResult = ((OkObjectResult)result.Result).Value;
            var resultResponse = (OkObjectResult)result.Result;
            var seatsList = (List<SeatDomainModel>)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(resultResponse.StatusCode, expectedStatusCode);
            Assert.AreEqual(seatsList.Count, expectedResultCount);
            Assert.AreEqual(expectedSeats.DataList, seatsResult);
        }

        [TestMethod]
        public async Task GetReservedSeatsAsync_When_Id_Exists_IsSuccessfull_True_Return_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 0;
            var projectionId = Guid.NewGuid();
            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel> { }
            };

            _mockSeatService.Setup(srvc => srvc.GetReservedSeatsAsync(It.IsAny<Guid>())).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetReservedSeatsAsync(projectionId);
            var seatsResult = ((OkObjectResult)result.Result).Value;
            var resultResponse = (OkObjectResult)result.Result;
            var seatsList = (List<SeatDomainModel>)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(resultResponse.StatusCode, expectedStatusCode);
            Assert.AreEqual(seatsList.Count, expectedResultCount);
            Assert.AreEqual(expectedSeats.DataList, seatsResult);
        }

        [TestMethod]
        public async Task GetReservedSeatsAsync_When_Id_Is_NullOrEmpty_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var projectionId = Guid.Empty;
            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_ID_NULL
            };

            //Act
            var result = await _seatsController.GetReservedSeatsAsync(projectionId);
            var seatsResult = ((BadRequestObjectResult)result.Result).Value;
            var resultResponse = (BadRequestObjectResult)result.Result;
            var seatsList = (ErrorResponseModel)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(resultResponse.StatusCode, expectedStatusCode);
            Assert.AreEqual(expectedSeats.ErrorMessage, seatsList.ErrorMessage);
        }

        [TestMethod]
        public async Task GetReservedSeatsAsync_When_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var projectionId = Guid.NewGuid();
            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PROJECTION_GET_BY_ID
            };
            _mockSeatService.Setup(srvc => srvc.GetReservedSeatsAsync(It.IsAny<Guid>())).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetReservedSeatsAsync(projectionId);
            var seatsResult = ((BadRequestObjectResult)result.Result).Value;
            var resultResponse = (BadRequestObjectResult)result.Result;
            var seatsList = (ErrorResponseModel)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(resultResponse.StatusCode, expectedStatusCode);
            Assert.AreEqual(expectedSeats.ErrorMessage, seatsList.ErrorMessage);
        }

        [TestMethod]
        public async Task GetAllByAuditoriumIdAsync_When_IsSuccessful_True_Return_SeatsList()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 2;
            var auditoriumId = 1;
            var expectedSeats = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel>
                {
                    new SeatDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = auditoriumId,
                        Number = 1,
                        Row = 1
                    },
                    new SeatDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = auditoriumId,
                        Number = 2,
                        Row = 1
                    }
                }
            };
            _mockSeatService.Setup(srvc => srvc.GetByAuditoriumIdAsync(auditoriumId)).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetAllByAuditoriumIdAsync(auditoriumId);
            var seatsResult = ((OkObjectResult)result.Result).Value;
            var resultResponse = (OkObjectResult)result.Result;
            var seatsList = (List<SeatDomainModel>)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(resultResponse.StatusCode, expectedStatusCode);
            Assert.AreEqual(expectedResultCount, seatsList.Count);
        }

        [TestMethod]
        public async Task GetAllByAuditoriumIdAsync_When_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var auditoriumId = 1;
            var expectedResult = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR
            };
            _mockSeatService.Setup(srvc => srvc.GetByAuditoriumIdAsync(auditoriumId)).ReturnsAsync(expectedResult);

            //Act
            var result = await _seatsController.GetAllByAuditoriumIdAsync(auditoriumId);
            var seatsResult = ((BadRequestObjectResult)result.Result).Value;
            var resultResponse = (BadRequestObjectResult)result.Result;
            var errorResponse = (ErrorResponseModel)seatsResult;

            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(resultResponse.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public async Task GetMaxNumbersOfSeatsAsync_If_Is_Successsful__False_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
       
            var expectedSeats = new GenericResult<SeatsMaxNumbersDomainModel>
            {
                IsSuccessful = false,
            };

            _mockSeatService.Setup(srvc => srvc.GetMaxNumbersOfSeatsByAuditoriumIdAsync(It.IsNotNull<int>())).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetMaxNumbersOfSeatsAsync(It.IsNotNull<int>());
            var seatsResult = (BadRequestObjectResult)result.Result;
      
            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual( expectedStatusCode, seatsResult.StatusCode);
           
        }


        [TestMethod]
        public async Task GetMaxNumbersOfSeatsAsync_If_Is_Successsful__True_Return_OkObjectRequest()
        {
            //Arrange
            int expectedStatusCode = 200;
            bool isSuccessful = true;
            var expectedSeats = new GenericResult<SeatsMaxNumbersDomainModel>
            {
                IsSuccessful = true,
                Data= new SeatsMaxNumbersDomainModel { }
            };

            _mockSeatService.Setup(srvc => srvc.GetMaxNumbersOfSeatsByAuditoriumIdAsync(It.IsNotNull<int>())).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatsController.GetMaxNumbersOfSeatsAsync(It.IsNotNull<int>());
            var seatsResult = (OkObjectResult)result.Result;
        
            //Assert
            Assert.IsNotNull(seatsResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, seatsResult.StatusCode);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TicketControllerTests
    {
        private Mock<ITicketService> _mockTicketService;
        private TicketController _ticketController;

        [TestInitialize]
        public void TestInit()
        {
            _mockTicketService = new Mock<ITicketService>();
            _ticketController = new TicketController(_mockTicketService.Object);
        }

        [TestMethod]
        public async Task GetAsync_Return_All_Tickets()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 2;

            var expectedTickets = new GenericResult<TicketDomainModel>
            {
                DataList = new List<TicketDomainModel>
                {
                    new TicketDomainModel
                    {
                        Created = DateTime.Now.AddDays(1),
                        Id = Guid.NewGuid(),
                        Projection = new ProjectionDomainModel
                        {
                            Id = Guid.NewGuid(),
                            AditoriumName = "Auditorium Name",
                            AuditoriumId = 1,
                            MovieId = Guid.NewGuid(),
                            MovieTitle = "NazivFilma",
                            ProjectionTime = DateTime.Now.AddDays(1),
                            Duration = 90
                        },
                        Seat = new SeatDomainModel
                        {
                            AuditoriumId = 1,
                            Id = Guid.NewGuid(),
                            Number = 1,
                            Row = 1
                        },
                        User = new UserDomainModel
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Sasa",
                            LastName = "Gataric",
                            UserName = "sasag",
                            Role = "admin"
                        }
                    },
                    new TicketDomainModel
                    {
                        Created = DateTime.Now.AddDays(1),
                        Id = Guid.NewGuid(),
                        Projection = new ProjectionDomainModel
                        {
                            Id = Guid.NewGuid(),
                            AditoriumName = "Auditorium Name",
                            AuditoriumId = 1,
                            MovieId = Guid.NewGuid(),
                            MovieTitle = "NazivFilma",
                            ProjectionTime = DateTime.Now.AddDays(1),
                            Duration = 90
                        },
                        Seat = new SeatDomainModel
                        {
                            AuditoriumId = 1,
                            Id = Guid.NewGuid(),
                            Number = 2,
                            Row = 1
                        },
                        User = new UserDomainModel
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Sasa 2",
                            LastName = "Gataric 2",
                            UserName = "sasag 2",
                            Role = "admin"
                        }
                    }
                }
            };

            _mockTicketService.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedTickets);

            //Act
            var result = await _ticketController.GetAsync();
            var ticketsResult = ((OkObjectResult)result.Result).Value;
            var ticketDomainModelResultList = (List<TicketDomainModel>)ticketsResult;
            

            //Assert

            Assert.IsNotNull(ticketsResult);
            Assert.AreEqual(expectedResultCount, ticketDomainModelResultList.Count);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedTickets.DataList, ticketsResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }

        [TestMethod]

        public async Task GetAsync_Return_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 0;

            var expectedTickets = new GenericResult<TicketDomainModel>
            {
                DataList = new List<TicketDomainModel> { }
            };

            _mockTicketService.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedTickets);

            //Act
            var result = await _ticketController.GetAsync();
            var ticketsResult = ((OkObjectResult)result.Result).Value;
            var ticketDomainModelResultList = (List<TicketDomainModel>)ticketsResult;
            //Assert
            Assert.IsNotNull(ticketsResult);
            Assert.AreEqual(expectedResultCount, ticketDomainModelResultList.Count);
            Assert.AreSame(expectedTickets.DataList, ticketsResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod]
        public async Task GetTicketByID_When_Id_Exists_Returns_User()
        {
            //Arrange
            int expectedStatusCode = 200;
            var expectedId = Guid.NewGuid();

            var expectedTicket = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                Data = new TicketDomainModel
                {
                    Created = DateTime.Now.AddDays(1),
                    Id = expectedId,
                    Projection = new ProjectionDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AditoriumName = "Auditorium Name",
                        AuditoriumId = 1,
                        MovieId = Guid.NewGuid(),
                        MovieTitle = "NazivFilma",
                        ProjectionTime = DateTime.Now.AddDays(1),
                        Duration = 90
                    },
                    Seat = new SeatDomainModel
                    {
                        AuditoriumId = 1,
                        Id = Guid.NewGuid(),
                        Number = 2,
                        Row = 1
                    },
                    User = new UserDomainModel
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Sasa 2",
                        LastName = "Gataric 2",
                        UserName = "sasag 2",
                        Role = "admin"
                    }
                }
            };

            _mockTicketService.Setup(srvc => srvc.GetTicketByIdAsync(expectedId))
                .ReturnsAsync(expectedTicket);

            // Act
            var result = await _ticketController.GetByIdAsync(expectedId);
            var ticketResult = ((OkObjectResult)result.Result).Value;

            //Assert
            Assert.IsNotNull(ticketResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedTicket.Data, ticketResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod]
        public async Task GetTicketByID_When_Id_Is_Empty_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var expectedId = Guid.Empty;
            var expectedErrorMessage = Messages.TICKET_GET_BY_ID;

            var expectedTicket = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.TICKET_GET_BY_ID
            };

            _mockTicketService.Setup(srvc => srvc.GetTicketByIdAsync(expectedId))
                .ReturnsAsync(expectedTicket);

            // Act
            var result = await _ticketController.GetByIdAsync(expectedId);
            var ticketResult = ((BadRequestObjectResult)result.Result).Value;
            var error = (ErrorResponseModel)ticketResult;

            //Assert
            Assert.IsNotNull(ticketResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);
            Assert.AreEqual(expectedErrorMessage, error.ErrorMessage);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_CreateModelState_IsValide_Return_Ticket()
        {
            //Arrange
            int expectedStatusCode = 201;
            var createTicketModel = new CreateTicketModel
            {
                ProjectionId = Guid.NewGuid(),
                SeatId= Guid.NewGuid(),
                UserId= Guid.NewGuid()
            };

            var ticketReposnseModel = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                Data = new TicketDomainModel
                {
                    Created = DateTime.Now.AddDays(1),
                    Id = Guid.NewGuid(),
                    Projection = new ProjectionDomainModel
                    {
                        Id = createTicketModel.ProjectionId,
                        AditoriumName = "Auditorium Name",
                        AuditoriumId = 1,
                        MovieId = Guid.NewGuid(),
                        MovieTitle = "NazivFilma",
                        ProjectionTime = DateTime.Now.AddDays(1),
                        Duration = 90
                    },
                    Seat = new SeatDomainModel
                    {
                        AuditoriumId = 1,
                        Id = createTicketModel.SeatId,
                        Number = 2,
                        Row = 1
                    },
                    User = new UserDomainModel
                    {
                        Id = createTicketModel.UserId,
                        FirstName = "Sasa 2",
                        LastName = "Gataric 2",
                        UserName = "sasag 2",
                        Role = "admin"
                    }
                }
            };
            _mockTicketService.Setup(srvc => srvc.CreateTicketAsync(It.IsAny<CreateTicketDomainModel>())).ReturnsAsync(ticketReposnseModel);

            //Act
            var result =await _ticketController.CreateTicketAsync(createTicketModel);
            var resultResponse = (CreatedAtActionResult)result.Result;
            var ticketCreated = resultResponse.Value;

            //Assert
            Assert.IsNotNull(ticketCreated);
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            Assert.AreEqual(ticketCreated, ticketReposnseModel.Data);
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);

        }

        [TestMethod]
        public async Task CreateTicketAsync_When_CreateModelState_Is_Invalide_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var expectedErrorMessage = "The ProjectionId field is required.";
            var createTicketModel = new CreateTicketModel
            {
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _ticketController.ModelState.AddModelError("ProjectionId", expectedErrorMessage);

            //Act
            var result = await _ticketController.CreateTicketAsync(createTicketModel);
            var ticketResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResponse = ((SerializableError)ticketResult).GetValueOrDefault("ProjectionId");
            var message = (string[])errorResponse;
            var statisCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(message[0], expectedErrorMessage);
            Assert.AreEqual(expectedStatusCode, statisCode.StatusCode);

        }

        [TestMethod]
        public async Task CreateTicketAsync_When_IsSuccesfull_False_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var expectedErrorMessage = "Error occured while getting projection by Id, please try again.";
            GenericResult<TicketDomainModel> CreateTicketResponseModel = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PROJECTION_GET_BY_ID
            };

            var createTicketModel = new CreateTicketModel
            {
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _mockTicketService.Setup(srvc => srvc.CreateTicketAsync(It.IsAny<CreateTicketDomainModel>())).ReturnsAsync(CreateTicketResponseModel);

            //Act
            var result = await _ticketController.CreateTicketAsync(createTicketModel);

            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorResult.ErrorMessage, expectedErrorMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);

        }

        [TestMethod]
        public async Task CreateTicketAsync_When_Called_Handles_Exception_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            var createTicketModel = new CreateTicketModel
            {
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _mockTicketService.Setup(srvc => srvc.CreateTicketAsync(It.IsAny<CreateTicketDomainModel>())).Throws(dbUpdateException);

            //Act
            var result = await _ticketController.CreateTicketAsync(createTicketModel);

            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);

        }

        [TestMethod]
        public async Task DeleteTicket_When_Id_Is_Empty_Or_Null_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Error occured becouse Id field is empty or null.";
            int expectedStatusCode = 400;
            var Id = Guid.Empty;

            // Act
            var result = await _ticketController.DeleteTicketAsync(Id);
            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;
            var resultResponse = (BadRequestObjectResult)result.Result;
            var errorResponse = (ErrorResponseModel)badObjectResult;


            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
            Assert.AreEqual(errorResponse.ErrorMessage, expectedMessage);
        }

        [TestMethod]
        public async Task DeleteTicket_When_Called_Returns_Accepted()
        {
            //Arrange
            int expectedStatusCode = 202;
            var Id = Guid.NewGuid();

            var deletedTicket = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true
            };

            _mockTicketService.Setup(srvc => srvc.DeleteTicketAsync(Id)).ReturnsAsync(deletedTicket);

            // Act
            var result = await _ticketController.DeleteTicketAsync(Id);
            _mockTicketService.Verify(srvc => srvc.DeleteTicketAsync(Id), Times.Once);
            var resultResponse = (AcceptedResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
           
        }

        [TestMethod]
        public async Task DeleteTicket_When_IsSuccessful_False_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var Id = Guid.NewGuid();

            var deletedTicket = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false
            };

            _mockTicketService.Setup(srvc => srvc.DeleteTicketAsync(Id)).ReturnsAsync(deletedTicket);

            // Act
            var result = await _ticketController.DeleteTicketAsync(Id);
            _mockTicketService.Verify(srvc => srvc.DeleteTicketAsync(Id),Times.Once);
            var resultResponse = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);

        }

        [TestMethod]
        public async Task DeleteTicket_When_Called_Handles_Exception_Return_BadRequest()
        {
            //Arrange
            var Id = Guid.NewGuid();
            int expectedStatusCode = 400;
            string expectedMessage = "Inner exception error message.";
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _mockTicketService.Setup(srvc => srvc.DeleteTicketAsync(It.IsAny<Guid>())).Throws(dbUpdateException);

            //Act
            var result = await _ticketController.DeleteTicketAsync(Id);
            _mockTicketService.Verify(srvc => srvc.DeleteTicketAsync(Id), Times.Once);
            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;
            var errorStatusCode = (BadRequestObjectResult)result.Result;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);

        }
    }
}

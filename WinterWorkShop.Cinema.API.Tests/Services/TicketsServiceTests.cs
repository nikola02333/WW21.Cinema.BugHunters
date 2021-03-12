using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class TicketsServiceTests
    {
        private Mock<ITicketsRepository> _mockTicketRepository;
        private Mock<ISeatsRepository> _mockSeatsRepository;
        private Mock<IUsersRepository> _mockUsersRepository;
        private Mock<IProjectionsRepository> _mockProjectionsRepository;
        private TicketService _ticketService;

        private Ticket _ticket;
        private List<Ticket> _ticketList;
        private List<Seat> _reservedSeatsEmpty;
        private IEnumerable<Ticket> _expectedTickets;
        private IEnumerable<Seat> _reservedSeats;
        private Guid _movieId;
        private DateTime _date;
        private Guid _ticketId;
        private DateTime _ticketCreated;
        private CreateTicketDomainModel _createTicketDomainModel;
        private Seat _seatById;
        private Projection _projectionById;
        private User _userById;

        [TestInitialize]
        public void TestInit()
        {
            _mockTicketRepository = new Mock<ITicketsRepository>();
            _mockSeatsRepository = new Mock<ISeatsRepository>();
            _mockUsersRepository = new Mock<IUsersRepository>();
            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _ticketService = new TicketService(_mockTicketRepository.Object,_mockSeatsRepository.Object,_mockUsersRepository.Object,_mockProjectionsRepository.Object);

            _ticket = new Ticket
        {
                Created = DateTime.Now,
                Id = Guid.NewGuid(),
                Price = 300,
                    Projection = new Projection 
                    {
                        Id = Guid.NewGuid(),
                    Auditorium = new Auditorium { AuditoriumName = "Auditorium", Id = 1 },
                    Duration = 100,
                    Movie = new Movie { Id = Guid.NewGuid(), Title = "Film" },
                    ShowingDate = DateTime.Now,
                    },
                    Seat = new Seat
                    {
                    Id = Guid.NewGuid(),
                    AuditoriumId = 1,
                    Number = 2,
                    Row = 2
                    },
                    User = new User
                    {
                    Id = Guid.NewGuid(),
                    FirstName = "sasa",
                    LastName = "gataric",
                        Role = "admin",
                    UserName = "sasag"
                    }
            };
           

            _ticketList = new List<Ticket>();
            _ticketList.Add(_ticket);

            _expectedTickets = _ticketList;

            
            _reservedSeatsEmpty = new List<Seat>();
            _movieId = Guid.NewGuid();
            _date = DateTime.Now;
            _ticketId = Guid.NewGuid();
            _ticketCreated = DateTime.Now;
            _createTicketDomainModel = new CreateTicketDomainModel
            {
                ProjectionId = Guid.NewGuid(),
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };
            _reservedSeats = new List<Seat> { new Seat { Id = _createTicketDomainModel.SeatId, AuditoriumId = 1, Number = 2, Row = 2 } };
            _seatById = new Seat
            {
                Id = _createTicketDomainModel.SeatId,
                AuditoriumId = 1,
                Number = 2,
                Row = 2
            };
            _projectionById = new Projection
            {
                Id = _createTicketDomainModel.ProjectionId,
                Auditorium = new Auditorium
                {
                    AuditoriumName = "Auditorium",
                    Id = 1
                },
                Duration = 100,
                Movie = new Movie
                {
                    Title = "Film",
                    Id = _movieId
                },
                ShowingDate = _date,
                AuditoriumId = 1,
                MovieId = _movieId
            };
            _userById = new User
            {
                Id = _createTicketDomainModel.UserId,
                FirstName = "sasa",
                LastName = "gataric",
                Role = "admin",
                UserName = "sasag"
            };

        }

        [TestMethod]
        public async Task GetAllAsync_Return_GenericResult_With_TicketList()
        {
            //Arrange
            var expectedResultCount = 1;
            
            var expectedTicketsList = (List<Ticket>)_expectedTickets;

            _mockTicketRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_expectedTickets);

            //Act
            var resultAction = await _ticketService.GetAllAsync();
            var result = resultAction.DataList;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result[0], typeof(TicketDomainModel));
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.AreEqual(expectedTicketsList[0].Id, result[0].Id);

        }

        [TestMethod]
        public async Task GetAllAsync_Return_Null()
        {
            //Arrange
            IEnumerable<Ticket> expectedTickets = null;
            

            _mockTicketRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedTickets);

            //Act
            var resultAction = await _ticketService.GetAllAsync();
            

            //Assert
            Assert.IsNull(resultAction);
            
        }

        [TestMethod]
        public async Task GetTicketByIdAsyn_When_Not_Null_Return_GenericResult_With_Ticket()
        {
            //Arrange
            var id = Guid.NewGuid();
            
            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(_ticket);

            //Act
            var resultAction = await _ticketService.GetTicketByIdAsync(id);
            var result = resultAction.Data;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TicketDomainModel));
          

        }

        [TestMethod]
        public async Task GetTicketByIdAsyn_When_Is_Null_Return_GenericResult_ErrorMessge()
            {
            //Arrange
            var id = Guid.NewGuid();
            Ticket expectedTicket = null;
            var genericResult = new GenericResult<TicketDomainModel>
                {
                IsSuccessful = false,
                ErrorMessage = Messages.TICKET_GET_BY_ID
            };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedTicket);

            //Act
            var resultAction = await _ticketService.GetTicketByIdAsync(id);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_Return_GenericResult_IsSuccessful_True()
        {
            //Arrange
            var id = Guid.NewGuid();
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true
            };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(_ticket);
            _mockTicketRepository.Setup(repo => repo.Delete(id)).Returns(_ticket);

            
           
            //Act
            var resultAction = await _ticketService.DeleteTicketAsync(id);
            _mockTicketRepository.Verify(repo => repo.Delete(id), Times.Once);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.IsSuccessful, resultAction.IsSuccessful);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_When_GetByIdAsync_Is_Null_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var id = Guid.NewGuid();
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.TICKET_GET_BY_ID
            };
            Ticket resurnedTicketById = null;

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(resurnedTicketById);

            //Act
            var resultAction = await _ticketService.DeleteTicketAsync(id);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_When_Delete_Is_Null_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var id = Guid.NewGuid();
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.TICKET_DELTE_ERROR
            };
            Ticket deletedTicketById = null;

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(_ticket);
            _mockTicketRepository.Setup(repo => repo.Delete(id)).Returns(deletedTicketById);

            //Act
            var resultAction = await _ticketService.DeleteTicketAsync(id);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_Seat_IsNot_Reserved_Return_GenericResolt_With_Created_Ticket()
        {
            //Arrange
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                Data = new TicketDomainModel
                {
                    Created = _ticketCreated,
                    Id = _ticketId,
                    Price = 300,
                    Projection = new ProjectionDomainModel
                    {
                        ProjectionTime = _date,
                        MovieTitle = "Film",
                        MovieId = _movieId,
                        Id = _createTicketDomainModel.ProjectionId,
                        AuditoriumId = 1,
                    Duration = 100,
                        AditoriumName = "Auditorium"
                },
                    Seat = new SeatDomainModel
                {
                        Id = _createTicketDomainModel.SeatId,
                    AuditoriumId = 1,
                    Number = 2,
                    Row = 2
                },
                    User = new UserDomainModel
                {
                        Id = _createTicketDomainModel.UserId,
                    FirstName = "sasa",
                    LastName = "gataric",
                        UserName = "sasag",
                        Role = "admin"
                        
                    }
                }
            };
            Ticket ticketForInsert = new Ticket
            {
                Created = _ticketCreated,
                Id = _ticketId,
                ProjectionId = _projectionById.Id,
                Projection = new Projection
                {
                    Id = _createTicketDomainModel.ProjectionId,
                    Duration = 100,
                    ShowingDate = _date,
                    AuditoriumId = 1,
                    MovieId = _movieId
                },
                SeatId = _seatById.Id,
                Seat = _seatById,
                UserId = _userById.Id,
                User = _userById,
                Price = 300
            };
            Ticket insertedTicketById = new Ticket
            {
                Created = _ticketCreated,
                Id = _ticketId,
                ProjectionId = _projectionById.Id,
                Projection = _projectionById,
                SeatId = _seatById.Id,
                Seat = _seatById,
                UserId = _userById.Id,
                User = _userById,
                Price = 300
            };


            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(_seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(_userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_projectionById);
            _mockSeatsRepository.Setup(repo => repo.GetSeatInProjectionAuditoriumAsync(_createTicketDomainModel.SeatId, _createTicketDomainModel.ProjectionId)).ReturnsAsync(_seatById);
            _mockTicketRepository.Setup(repo => repo.InsertAsync(It.IsAny<Ticket>())).ReturnsAsync(ticketForInsert);
            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(_ticketId)).ReturnsAsync(insertedTicketById);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);
            _mockTicketRepository.Verify(repo => repo.InsertAsync(It.IsAny<Ticket>()),Times.Once);

            //Assert
            Assert.IsNotNull(resultAction.Data);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.Data.Id, resultAction.Data.Id);
            Assert.IsTrue(genericResult.IsSuccessful);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_Sear_Is_Reserved_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_RESERVED
            };
            
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeats);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_GetSeatById_Is_Null_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            Seat seatById = null;

            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_GET_BY_ID
            };
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(_userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_projectionById);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_GetUserById_And_GetUserById_Is_Null_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            Seat seatById = null;
            User userById = null;

            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_GET_BY_ID + Messages.USER_ID_NULL
            };
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_projectionById);

          
            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_GetUserById_And_GetUserById_And_GetProjectionById_Is_Null_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            Seat seatById = null;
            User userById = null;
            Projection projectionById = null;

            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_GET_BY_ID + Messages.USER_ID_NULL + Messages.PROJECTION_GET_BY_ID
            };
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(projectionById);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }
        [TestMethod]
        public async Task CreateTicketAsync_When_Seat_is_Not_In_Projection_Is_Null_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            Seat seatByIdNotInProjection = null;
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_NOT_IN_AUDITORIUM_OF_PROJECTION
            };
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(_seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(_userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_projectionById);
            _mockSeatsRepository.Setup(repo => repo.GetSeatInProjectionAuditoriumAsync(_createTicketDomainModel.SeatId, _createTicketDomainModel.ProjectionId)).ReturnsAsync(seatByIdNotInProjection);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }

        [TestMethod]
        public async Task CreateTicketAsync_When_InsertAsync_Is_Null_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            Ticket returnInserted = null;
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.TICKET_CREATION_ERROR
            };
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(_seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(_userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_projectionById);
            _mockSeatsRepository.Setup(repo => repo.GetSeatInProjectionAuditoriumAsync(_createTicketDomainModel.SeatId, _createTicketDomainModel.ProjectionId)).ReturnsAsync(_seatById);
            _mockTicketRepository.Setup(repo => repo.InsertAsync(It.IsAny<Ticket>())).ReturnsAsync(returnInserted);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);
            _mockTicketRepository.Verify(repo => repo.InsertAsync(It.IsAny<Ticket>()),Times.Once);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }
        [TestMethod]
        public async Task CreateTicketAsync_When_GetByIdAsync_Inserted_Is_Null_Return_GenericResolt_ErrorMessage()
        {
            //Arrange
            Ticket insertedTicketById = null;
            var genericResult = new GenericResult<TicketDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.TICKET_GET_BY_ID
            };
            Ticket ticketForInsert = new Ticket
            {
                Created = _ticketCreated,
                Id = _ticketId,
                ProjectionId = _projectionById.Id,
                Projection = new Projection
                {
                    Id = _createTicketDomainModel.ProjectionId,
                    Duration = 100,
                    ShowingDate = _date,
                    AuditoriumId = 1,
                    MovieId = _movieId
                },
                SeatId = _seatById.Id,
                Seat = _seatById,
                UserId = _userById.Id,
                User = _userById,
                Price = 300
            };
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_reservedSeatsEmpty);
            _mockSeatsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.SeatId)).ReturnsAsync(_seatById);
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.UserId)).ReturnsAsync(_userById);
            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(_createTicketDomainModel.ProjectionId)).ReturnsAsync(_projectionById);
            _mockSeatsRepository.Setup(repo => repo.GetSeatInProjectionAuditoriumAsync(_createTicketDomainModel.SeatId, _createTicketDomainModel.ProjectionId)).ReturnsAsync(_seatById);
            _mockTicketRepository.Setup(repo => repo.InsertAsync(It.IsAny<Ticket>())).ReturnsAsync(ticketForInsert);
            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(_ticketId)).ReturnsAsync(insertedTicketById);

            //Act
            var resultAction = await _ticketService.CreateTicketAsync(_createTicketDomainModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(GenericResult<TicketDomainModel>));
            Assert.AreEqual(genericResult.ErrorMessage, resultAction.ErrorMessage);
            Assert.IsFalse(genericResult.IsSuccessful);
        }


    }
}

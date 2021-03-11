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
        private Mock<IProjectionsRepository> _mockRrojectionsRepository;
        private Mock<ITicketServiceFunction> _mockTicketServiceFunction;

        private TicketService _ticketService;

        [TestInitialize]
        public void TestInit()
        {
            _mockTicketRepository = new Mock<ITicketsRepository>();
            _mockSeatsRepository = new Mock<ISeatsRepository>();
            _mockUsersRepository = new Mock<IUsersRepository>();
            _mockRrojectionsRepository = new Mock<IProjectionsRepository>();
            _mockTicketServiceFunction = new Mock<ITicketServiceFunction>();
            _ticketService = new TicketService(_mockTicketRepository.Object,_mockSeatsRepository.Object,_mockUsersRepository.Object,_mockRrojectionsRepository.Object, _mockTicketServiceFunction.Object);
        }

        [TestMethod]
        public async Task GetAllAsync_Return_GenericResult_With_TicketList()
        {
            //Arrange
            var expectedResultCount = 1;
            IEnumerable<Ticket> expectedTickets = new List<Ticket>
            {
               new Ticket
                {
                    Created= DateTime.Now,
                    Id=Guid.NewGuid(),
                    price=300,
                    Projection = new Projection 
                    {
                        Id = Guid.NewGuid(),
                        Auditorium = new Auditorium{AuditoriumName="Auditorium", Id=1},
                        Duration=100,
                        Movie = new Movie{Id=Guid.NewGuid(), Title = "Film"},
                        ShowingDate=DateTime.Now,  
                    },
                    Seat = new Seat
                    {
                        Id=Guid.NewGuid(),
                        AuditoriumId =1,
                        Number=2,
                        Row=2
                    },
                    User = new User
                    {
                        Id=Guid.NewGuid(),
                        FirstName="sasa",
                        LastName="gataric",
                        Role = "admin",
                        UserName="sasag"
                    }
                }
            };
            var expectedTicketsList = (List<Ticket>)expectedTickets;

            _mockTicketRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedTickets);

            //_mockTicketServiceFunction.Verify(repo => repo.createTicketDomainModel(expectedTicketsList[0]), Times.Exactly(expectedTicketsList.Count));
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
            IEnumerable<Ticket> expectedTickets = new List<Ticket>();
            var expectedTicketsList = (List<Ticket>)expectedTickets;

            _mockTicketRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedTickets);

            //_mockTicketServiceFunction.Verify(repo => repo.createTicketDomainModel(expectedTicketsList[0]), Times.Exactly(expectedTicketsList.Count));
            //Act
            var resultAction = await _ticketService.GetAllAsync();
            var result = resultAction.DataList;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TicketDomainModel>));
        }

        [TestMethod]
        public async Task GetTicketByIdAsyn_When_Not_Null_Return_GenericResult_With_Ticket()
        {
            //Arrange
            var id = Guid.NewGuid();
            var expectedTicket = new Ticket
            {
                Created = DateTime.Now,
                Id = id,
                price = 300,
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
            
            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedTicket);

            //Act
            var resultAction = await _ticketService.GetTicketByIdAsync(id);
            var result = resultAction.Data;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TicketDomainModel));
          

        }

        [TestMethod]
        public async Task GetTicketByIdAsyn_When_Is_Null_Return_GenericResult_With_ErrorMessge()
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
    }
}

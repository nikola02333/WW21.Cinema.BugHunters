using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class SeatsServiceTests
    {
        private Mock<ISeatsRepository> _mockSeatsRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumsRepository;
        private Mock<IProjectionsRepository> _mockProjectionsRepository;
        private SeatService _seatService;

        private Seat _seat;
        private List<Seat> _seatList;
        private IEnumerable<Seat> _expectedSeats;

        [TestInitialize]
        public void TestInit()
        {
            _mockSeatsRepository = new Mock<ISeatsRepository>();
            _mockAuditoriumsRepository = new Mock<IAuditoriumsRepository>();
            _mockProjectionsRepository = new Mock<IProjectionsRepository>();

            _seatService = new SeatService(_mockSeatsRepository.Object, _mockAuditoriumsRepository.Object, _mockProjectionsRepository.Object);

            _seat = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 1,
                Row = 2,
                AuditoriumId = 1
            };

            _seatList = new List<Seat>();
            _seatList.Add(_seat);

            _expectedSeats = _seatList;
        }

        [TestMethod]
        public async Task GetAllAsync_When_GetAllFromRepository_Is_NotNull_Return_SeatsList_In_GenericResult()
        {
            //Arrange
            var expectedCount = 1;
            var expectedResponce = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel>
                {
                    new SeatDomainModel
                    {
                        AuditoriumId=_seat.AuditoriumId,
                        Id=_seat.Id,
                        Number= _seat.Number,
                        Row = _seat.Row
                    }
                }
            };

            _mockSeatsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_expectedSeats);

            //Act
            var result =await _seatService.GetAllAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatDomainModel>));
            Assert.AreEqual(expectedResponce.DataList[0].Id, result.DataList[0].Id);
            Assert.IsTrue(expectedResponce.IsSuccessful);
            Assert.AreEqual(expectedCount, result.DataList.Count);
        }
        [TestMethod]
        public async Task GetAllAsync_When_GetAllFromRepository_Is_Null_Return_Null_GenericResult()
        {
            //Arrange
            IEnumerable<Seat> expectedSeats = null;

            _mockSeatsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatService.GetAllAsync();

            //Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public async Task GetByAuditoriumIdAsync_When_AuditoriumId_Is_Valid_Return_GenericResult_With_SeatsDomainModel()
        {
            //Arrange
            var auditoriumId = 1;
            var auditorium = new Auditorium
            {
                AuditoriumName = "Auditorium",
                CinemaId = 1,
                Id = auditoriumId,
            };
            var expectedResponce = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel>
                {
                    new SeatDomainModel
                    {
                        AuditoriumId=_seat.AuditoriumId,
                        Id=_seat.Id,
                        Number= _seat.Number,
                        Row = _seat.Row
                    }
                }
            };

            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(auditoriumId)).ReturnsAsync(auditorium);
            _mockSeatsRepository.Setup(repo => repo.GetSeatsByAuditoriumIdAsync(auditoriumId)).ReturnsAsync(_expectedSeats);

            //Act
            var result = await _seatService.GetByAuditoriumIdAsync(auditoriumId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatDomainModel>));
            Assert.AreEqual(expectedResponce.DataList[0].Id, result.DataList[0].Id);
            Assert.IsTrue(expectedResponce.IsSuccessful);
        }

        [TestMethod]
        public async Task GetByAuditoriumIdAsync_When_AuditoriumId_DoseNot_Exist_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var auditoriumId = 1;

            Auditorium auditorium = null;
            var expectedResponce = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR
            };

            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(auditoriumId)).ReturnsAsync(auditorium);
            

            //Act
            var result = await _seatService.GetByAuditoriumIdAsync(auditoriumId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatDomainModel>));
            Assert.AreEqual(expectedResponce.ErrorMessage,result.ErrorMessage);
            Assert.IsFalse(expectedResponce.IsSuccessful);
        }

        [TestMethod]
        public async Task GetByAuditoriumIdAsync_When_GetSeatsByAuditoriumIdAsync_IsNull_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var auditoriumId = 1;
            IEnumerable<Seat> expectedSeats = null;
            var auditorium = new Auditorium
            {
                AuditoriumName = "Auditorium",
                CinemaId = 1,
                Id = auditoriumId,
            };
            var expectedResponce = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEATS_IN_AUDITORIUM
            };

            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(auditoriumId)).ReturnsAsync(auditorium);
            _mockSeatsRepository.Setup(repo => repo.GetSeatsByAuditoriumIdAsync(auditoriumId)).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatService.GetByAuditoriumIdAsync(auditoriumId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatDomainModel>));
            Assert.AreEqual(expectedResponce.ErrorMessage, result.ErrorMessage);
            Assert.IsFalse(expectedResponce.IsSuccessful);
        }

        [TestMethod]
        public async Task GetReservedSeatsAsync_When_Projection_Have_Reserved_Seats_Return_GenericResult_With_RezervedSeats()
        {
            //Arrange
            var projectionId = Guid.NewGuid();
            var showingDate = DateTime.Now;
            var projection = new Projection
            {
                Id = projectionId,
                Duration = 100,
                Price = 300,
                ShowingDate = showingDate
            };
            var expectedResponce = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<SeatDomainModel>
                {
                    new SeatDomainModel
                    {
                        AuditoriumId=_seat.AuditoriumId,
                        Id=_seat.Id,
                        Number= _seat.Number,
                        Row = _seat.Row
                    }
                }
            };

            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(projectionId)).ReturnsAsync(projection);
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(projectionId)).ReturnsAsync(_expectedSeats);

            //Act
            var result = await _seatService.GetReservedSeatsAsync(projectionId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatDomainModel>));
            Assert.AreEqual(expectedResponce.DataList[0].Id, result.DataList[0].Id);
            Assert.IsTrue(expectedResponce.IsSuccessful);
        }

        [TestMethod]
        public async Task GetReservedSeatsAsync_When_Projection_DontHave_Reserved_Seats_Return_NULL()
        {
            //Arrange
            var projectionId = Guid.NewGuid();
            var showingDate = DateTime.Now;
            IEnumerable<Seat> expectedSeats = null;
            var projection = new Projection
            {
                Id = projectionId,
                Duration = 100,
                Price = 300,
                ShowingDate = showingDate
            };
            

            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(projectionId)).ReturnsAsync(projection);
            _mockSeatsRepository.Setup(repo => repo.GetReservedSeatsForProjectionAsync(projectionId)).ReturnsAsync(expectedSeats);

            //Act
            var result = await _seatService.GetReservedSeatsAsync(projectionId);

            //Assert
            Assert.IsNull(result);
           
        }
        [TestMethod]

        public async Task GetReservedSeatsAsync_When_Projection_DoesNot_Exist_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var projectionId = Guid.NewGuid();
            Projection projection = null;
            var expectedResponce = new GenericResult<SeatDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PROJECTION_GET_BY_ID
            };


            _mockProjectionsRepository.Setup(repo => repo.GetByIdAsync(projectionId)).ReturnsAsync(projection);

            //Act
            var result = await _seatService.GetReservedSeatsAsync(projectionId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatDomainModel>));
            Assert.AreEqual(expectedResponce.ErrorMessage, result.ErrorMessage);
            Assert.IsFalse(expectedResponce.IsSuccessful);
        }

        [TestMethod]

        public async Task GetMaxNumbersOfSeatsByAuditoriumIdAsync_If_Auditorium_Null_Returns_ErrorMessage()
        {
            //Arrange
            Auditorium auditorium = null;
            bool isSuccessful = false;
            var expectedErrorMassage = Messages.AUDITORIUM_GET_BY_ID_ERROR;

            _mockAuditoriumsRepository.Setup(srvc => srvc.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(auditorium);

            //Act
            var result = await _seatService.GetMaxNumbersOfSeatsByAuditoriumIdAsync(It.IsNotNull<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatsMaxNumbersDomainModel>));
            Assert.AreEqual(expectedErrorMassage, result.ErrorMessage);
            Assert.AreEqual(isSuccessful,result.IsSuccessful);
        }

        [TestMethod]

        public async Task GetMaxNumbersOfSeatsByAuditoriumIdAsync_If_Seats_Null_Returns_ErrorMessage()
        {
            //Arrange
            Auditorium auditorium = new Auditorium { };
            bool isSuccessful = false;
            var expectedErrorMassage = Messages.SEATS_IN_AUDITORIUM;
            List<Seat> seats = null;

            _mockAuditoriumsRepository.Setup(srvc => srvc.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(auditorium);

            _mockSeatsRepository.Setup(srvc => srvc.GetSeatsByAuditoriumIdAsync(It.IsAny<int>())).ReturnsAsync(seats);

            //Act
            var result = await _seatService.GetMaxNumbersOfSeatsByAuditoriumIdAsync(It.IsNotNull<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatsMaxNumbersDomainModel>));
            Assert.AreEqual(expectedErrorMassage, result.ErrorMessage);
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
        }

        [TestMethod]

        public async Task GetMaxNumbersOfSeatsByAuditoriumIdAsync_Returns_Seats()
        {
            //Arrange
            Auditorium auditorium = new Auditorium { };
            bool isSuccessful = true;
        
            _mockAuditoriumsRepository.Setup(srvc => srvc.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(auditorium);
            _mockSeatsRepository.Setup(srvc => srvc.GetSeatsByAuditoriumIdAsync(It.IsAny<int>())).ReturnsAsync(_seatList);

            //Act
            var result = await _seatService.GetMaxNumbersOfSeatsByAuditoriumIdAsync(It.IsNotNull<int>());
          
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<SeatsMaxNumbersDomainModel>));
            Assert.AreEqual(isSuccessful, result.IsSuccessful);
        }

    }
}

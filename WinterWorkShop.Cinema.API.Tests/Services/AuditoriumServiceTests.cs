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
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class AuditoriumServiceTests
    {

        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<IProjectionsRepository> _mockProjectionRepository;
        private Mock<ISeatsRepository> _mockSeatRepository;
        private Mock<ICinemasRepository> _mockCinemaRepository;
        private AuditoriumService _auditoriumService;

        [TestInitialize]
        public void TestInit()
        {
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _auditoriumService = new AuditoriumService(_mockAuditoriumRepository.Object, _mockCinemaRepository.Object, _mockSeatRepository.Object, _mockProjectionRepository.Object);
        }



        [TestMethod]
        public async Task GetAllAuditoriums_Returns_LisatOfAuditoriums()
        {

            var expectedAuditoriums = new List<Auditorium>
            {
                new Auditorium
                {
                    AuditoriumName = "First Auditorium",
                    Id = 1,
                    CinemaId = 1,
                    Seats = new List<Seat>
                    {
                        new Seat
                        {
                            Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                            AuditoriumId = 1,
                            Number = 3,
                            Row = 1
                        },
                        new Seat
                        {
                            Id = new Guid("206c5c79-32d5-456a-97ec-3ee26bdd742b"),
                            AuditoriumId = 1,
                            Number = 2,
                            Row = 2
                        }
                    }


                },

                new Auditorium
                {
                    AuditoriumName = "First Auditorium",
                    Id = 2,
                    CinemaId = 1,
                    Seats = new List<Seat>
                    {
                        new Seat
                        {
                            Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                            AuditoriumId = 2,
                            Number = 3,
                            Row = 3
                        },
                        new Seat
                        {
                            Id = new Guid("206c5c79-32d5-456a-97ec-3ee26bdd742b"),
                            AuditoriumId = 2,
                            Number = 2,
                            Row = 2
                        }
                    }


                }
            };

            var expectedAuditoriumResult = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<AuditoriumDomainModel>
                {

                }
            };

            foreach (var item in expectedAuditoriums)
            {
                var auditoriumDomainModel = new AuditoriumDomainModel
                {
                    CinemaId = item.CinemaId,
                    Id = item.Id,
                    Name = item.AuditoriumName,
                    SeatsList = item.Seats.Select(s => new SeatDomainModel
                    {
                        Id = s.Id,
                        AuditoriumId = s.AuditoriumId,
                        Number = s.Number,
                        Row = s.Row
                    }).ToList()
                };
                expectedAuditoriumResult.DataList.Add(auditoriumDomainModel);
            }

            _mockAuditoriumRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedAuditoriums);

            //Act
            var result = await _auditoriumService.GetAllAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList()[0].Name, expectedAuditoriums[0].AuditoriumName);
            Assert.AreEqual(result.ToList().Count, expectedAuditoriums.Count);
        }

        [TestMethod]
        public async Task GetAllAuditoriums_Returns_EmptyList()
        {

            var expectedAuditoriums = new List<Auditorium>
            {

            };

            var expectedAuditoriumResult = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<AuditoriumDomainModel>
                {

                }
            };


            _mockAuditoriumRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedAuditoriums);

            //Act
            var result = await _auditoriumService.GetAllAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList().Count, expectedAuditoriums.Count);
        }

        [TestMethod]
        public async Task CreateAuditoriums_Returns_Created_Auditorium()
        {
            int numberOfRows = 2;

            int numberOfSeats = 2;

            var cinema = new Data.Cinema

            {
                Id = 1,
                Address = "Perleska2",
                CityName = "New York",
                Name = "Cinestar"
            };

            string auditName = "First Auditorium";
            int id = 2;
            var auditoriumToCreate = new Auditorium
            {
                AuditoriumName = "First Auditorium",
                Id = 2,
                CinemaId = 1,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                     Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     AuditoriumId = 2,
                     Number =  numberOfSeats,
                     Row = numberOfRows
                    }
                }
            };





            IEnumerable<Auditorium> _expectedAuditoriums = new List<Auditorium>
            {



            };

            var auditoriumModel = new AuditoriumDomainModel
            {
                CinemaId = auditoriumToCreate.CinemaId,
                Id = auditoriumToCreate.Id,
                Name = auditoriumToCreate.AuditoriumName,
                SeatsList = auditoriumToCreate.Seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                }).ToList()
            };




            GenericResult<AuditoriumDomainModel> resultModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Data = auditoriumModel
            };

            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumToCreate.CinemaId)).ReturnsAsync(cinema);

            _mockAuditoriumRepository.Setup(srvc => srvc.GetByAuditoriumNameAsync(auditoriumToCreate.AuditoriumName, auditoriumToCreate.CinemaId)).ReturnsAsync(_expectedAuditoriums);

            _mockAuditoriumRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<Auditorium>())).ReturnsAsync(auditoriumToCreate);

            var result = _auditoriumService.CreateAuditorium(auditoriumModel, numberOfRows, numberOfSeats);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));
            Assert.AreEqual(resultModel.Data.Name, result.Result.Data.Name);
        }



        [TestMethod]
        public async Task CreateAuditoriums_If_Cinema_Id_Is_Wrong_Returns_Error_Massage()
        {


            int cinemaId = 1;

            int numberOfRows = 2;

            int numberOfSeats = 2;

            var expectedErrorMessage = Messages.AUDITORIUM_UNVALID_CINEMAID;


            int id = 2;

            var cinema = new Data.Cinema
            {


            };



            var auditoriumToCreate = new Auditorium
            {
                AuditoriumName = "First Auditorium",
                Id = 2,
                CinemaId = 1,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                     Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     AuditoriumId = 2,
                     Number =  numberOfSeats,
                     Row = numberOfRows
                    }
                }
            };

            var auditoriumModel = new AuditoriumDomainModel
            {
                CinemaId = auditoriumToCreate.CinemaId,
                Id = auditoriumToCreate.Id,
                Name = auditoriumToCreate.AuditoriumName,
                SeatsList = auditoriumToCreate.Seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                }).ToList()
            };


            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(It.IsAny<Data.Cinema>());

            var result = _auditoriumService.CreateAuditorium(auditoriumModel, numberOfRows, numberOfSeats);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));
            Assert.AreEqual(expectedErrorMessage, result.Result.ErrorMessage);
            Assert.AreEqual(result.Result.IsSuccessful, false);
        }

        [TestMethod]
        public async Task CreateAuditoriums_If_Auditorium_Already_Exists_Returns_Error_Massage()
        {


            int cinemaId = 1;

            int numberOfRows = 2;

            int numberOfSeats = 2;

            var expectedErrorMessage = Messages.AUDITORIUM_SAME_NAME;

            string auditName = "First Auditorium";
            int id = 2;

            var cinema = new Data.Cinema

            {
                Id = 1,
                Address = "Perleska2",
                CityName = "New York",
                Name = "Cinestar"
            };

            var auditoriumToCreate = new Auditorium
            {


                AuditoriumName = "First Auditorium",
                Id = 2,
                CinemaId = 1,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                     Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     AuditoriumId = 2,
                     Number =  numberOfSeats,
                     Row = numberOfRows
                    }
                }



            };

            var _auditoriumList = new List<Auditorium>();

            _auditoriumList.Add(auditoriumToCreate);

            IEnumerable<Auditorium> _expectedAuditoriums = _auditoriumList;

            var auditoriumModel = new AuditoriumDomainModel
            {
                CinemaId = auditoriumToCreate.CinemaId,
                Id = auditoriumToCreate.Id,
                Name = auditoriumToCreate.AuditoriumName,
                SeatsList = auditoriumToCreate.Seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                }).ToList()
            };



            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumToCreate.CinemaId)).ReturnsAsync(cinema);

            _mockAuditoriumRepository.Setup(srvc => srvc.GetByAuditoriumNameAsync(auditoriumToCreate.AuditoriumName, auditoriumToCreate.CinemaId)).ReturnsAsync(_auditoriumList);

            var result = _auditoriumService.CreateAuditorium(auditoriumModel, numberOfRows, numberOfSeats);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));

            Assert.AreEqual(result.Result.ErrorMessage, expectedErrorMessage);
        }

        [TestMethod]
        public async Task CreateAuditoriums_Returns_Created_Error()
        {
            int numberOfRows = 2;

            int numberOfSeats = 2;




            var cinema = new Data.Cinema

            {
                Id = 1,
                Address = "Perleska2",
                CityName = "New York",
                Name = "Cinestar"
            };

            var expectedErrorMessage = Messages.AUDITORIUM_CREATION_ERROR;

            string auditName = "First Auditorium";
            int id = 2;

            var auditorium = new Auditorium
            {

            };

            var auditoriumToCreate = new Auditorium
            {
                AuditoriumName = "First Auditorium",
                Id = 2,
                CinemaId = 1,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                     Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     AuditoriumId = 2,
                     Number =  numberOfSeats,
                     Row = numberOfRows
                    }
                }
            };


            var auditoriumModel = new AuditoriumDomainModel
            {
                CinemaId = auditoriumToCreate.CinemaId,
                Id = auditoriumToCreate.Id,
                Name = auditoriumToCreate.AuditoriumName,
                SeatsList = auditoriumToCreate.Seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                }).ToList()
            };

            IEnumerable<Auditorium> _expectedAuditoriums = new List<Auditorium>
            {



            };

            GenericResult<AuditoriumDomainModel> resultModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Data = auditoriumModel
            };

            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumToCreate.CinemaId)).ReturnsAsync(cinema);

            _mockAuditoriumRepository.Setup(srvc => srvc.GetByAuditoriumNameAsync(auditoriumToCreate.AuditoriumName, auditoriumToCreate.CinemaId)).ReturnsAsync(_expectedAuditoriums);

            _mockAuditoriumRepository.Setup(srvc => srvc.InsertAsync(auditoriumToCreate)).ReturnsAsync(auditorium);

            var result = _auditoriumService.CreateAuditorium(auditoriumModel, numberOfRows, numberOfSeats);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));

            Assert.AreEqual(result.Result.ErrorMessage, expectedErrorMessage);
        }

        [TestMethod]
        public async Task GetByAuditorium_Returns_Auditorium()
        {
            int numberOfRows = 2;

            int numberOfSeats = 2;
            int auditoriumId = 1;

            var auditoriumToGet = new Auditorium
            {
                AuditoriumName = "First Auditorium",
                Id = auditoriumId,
                CinemaId = 1,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                     Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     AuditoriumId = 2,
                     Number =  numberOfSeats,
                     Row = numberOfRows
                    }
                }
            };

            var auditoriumModel = new AuditoriumDomainModel
            {
                CinemaId = auditoriumToGet.CinemaId,
                Id = auditoriumToGet.Id,
                Name = auditoriumToGet.AuditoriumName,
                SeatsList = auditoriumToGet.Seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                }).ToList()
            };
            _mockAuditoriumRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumId)).ReturnsAsync(auditoriumToGet);

            var result = _auditoriumService.GetByIdAsync(auditoriumModel.Id);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));

            Assert.AreEqual(result.Result.IsSuccessful, true);



        }


        [TestMethod]
        public async Task GetByAuditorium_Returns_ErrorMessage()
        {
            int numberOfRows = 2;

            int numberOfSeats = 2;
            int auditoriumId = 1;

            var expectedErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR;

            var auditoriumToGet = new Auditorium
            {

            };

            var auditoriumModel = new AuditoriumDomainModel
            {

                Id = auditoriumToGet.Id

            };

            _mockAuditoriumRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumId)).ReturnsAsync(auditoriumToGet);

            var result = _auditoriumService.GetByIdAsync(auditoriumModel.Id);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));

            Assert.AreEqual(result.Result.IsSuccessful, false);

            Assert.AreEqual(expectedErrorMessage, result.Result.ErrorMessage);

        }

        [TestMethod]
        public async Task DeleteAuditorium_Returns_Deleted_Auditorium()
        {
            int numberOfRows = 2;

            int numberOfSeats = 2;
            int auditoriumId = 1;

            var auditoriumToDelete = new Auditorium
            {
                AuditoriumName = "First Auditorium",
                Id = auditoriumId,
                CinemaId = 1,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                     Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                     AuditoriumId = 2,
                     Number =  numberOfSeats,
                     Row = numberOfRows
                    }
                }
            };

            var auditoriumModel = new AuditoriumDomainModel
            {
                CinemaId = auditoriumToDelete.CinemaId,
                Id = auditoriumToDelete.Id,
                Name = auditoriumToDelete.AuditoriumName,
                SeatsList = auditoriumToDelete.Seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                }).ToList()
            };
            _mockAuditoriumRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumToDelete.Id)).ReturnsAsync(auditoriumToDelete);
            _mockAuditoriumRepository.Setup(srvc => srvc.Delete(auditoriumToDelete.Id)).Returns(auditoriumToDelete);

            var result = _auditoriumService.DeleteAsync(auditoriumModel.Id);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result.Result, typeof(GenericResult<AuditoriumDomainModel>));

            Assert.AreEqual(result.Result.IsSuccessful, true);



        }


        [TestMethod]

        public async Task GetAllAuditoriumByCinemaId_Returns_Auditoriums()
        {

            var cinemaId = 1;
            var expectedAuditoriums = new List<Auditorium>
            {
                new Auditorium
                {
                    AuditoriumName = "First Auditorium",
                    Id = 1,
                    CinemaId = cinemaId,
                    Seats = new List<Seat>
                    {
                        new Seat
                        {
                            Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                            AuditoriumId = 1,
                            Number = 3,
                            Row = 1
                        },
                        new Seat
                        {
                            Id = new Guid("206c5c79-32d5-456a-97ec-3ee26bdd742b"),
                            AuditoriumId = 1,
                            Number = 2,
                            Row = 2
                        }
                    }


                },

                new Auditorium
                {
                    AuditoriumName = "First Auditorium",
                    Id = 2,
                    CinemaId = cinemaId,
                    Seats = new List<Seat>
                    {
                        new Seat
                        {
                            Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                            AuditoriumId = 2,
                            Number = 3,
                            Row = 3
                        },
                        new Seat
                        {
                            Id = new Guid("206c5c79-32d5-456a-97ec-3ee26bdd742b"),
                            AuditoriumId = 2,
                            Number = 2,
                            Row = 2
                        }
                    }


                }
            };

            var expectedAuditoriumResult = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<AuditoriumDomainModel>
                {

                }
            };

            foreach (var item in expectedAuditoriums)
            {
                var auditoriumDomainModel = new AuditoriumDomainModel
                {
                    CinemaId = item.CinemaId,
                    Id = item.Id,
                    Name = item.AuditoriumName,
                    SeatsList = item.Seats.Select(s => new SeatDomainModel
                    {
                        Id = s.Id,
                        AuditoriumId = s.AuditoriumId,
                        Number = s.Number,
                        Row = s.Row
                    }).ToList()
                };
                expectedAuditoriumResult.DataList.Add(auditoriumDomainModel);
            }

            var cinema = new Data.Cinema
            {
                Id = 1,
                Address = "Perleska2",
                CityName = "New York",
                Name = "Cinestar"

            };

            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(cinema);
            _mockAuditoriumRepository.Setup(srvc => srvc.GetAllByCinemaIdAsync(cinemaId)).ReturnsAsync(expectedAuditoriums);

            //Act
            var result = await _auditoriumService.GetAllAuditoriumByCinemaId(cinemaId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<AuditoriumDomainModel>));
            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(expectedAuditoriumResult.DataList[0].Name, result.DataList[0].Name);
        }



        [TestMethod]

        public async Task GetAllAuditoriumByCinemaId_Returns_ErrorMassage()
        {

            var cinemaId = 1;

            var expectedAuditoriumResult = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_UNVALID_CINEMAID


            };

            var cinema = new Data.Cinema
            {
                Id = 1,
                Address = "Perleska2",
                CityName = "New York",
                Name = "Cinestar"

            };
            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(cinema);
            _mockAuditoriumRepository.Setup(srvc => srvc.GetAllByCinemaIdAsync(cinemaId)).ReturnsAsync(It.IsAny<List<Auditorium>>());

            //Act
            var result = await _auditoriumService.GetAllAuditoriumByCinemaId(cinemaId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<AuditoriumDomainModel>));
            Assert.AreEqual(expectedAuditoriumResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedAuditoriumResult.ErrorMessage, result.ErrorMessage);
        }



        [TestMethod]

        public async Task GetAllAuditoriumByCinemaId_Cinema_Id_Not_Found_Returns_ErrorMassage()
        {

            var cinemaId = 1;

            var expectedAuditoriumResult = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.CINEMA_ID_NOT_FOUND

            };

            var cinema = new Data.Cinema
            {

            };

            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(It.IsAny<Data.Cinema>);

            //Act
            var result = await _auditoriumService.GetAllAuditoriumByCinemaId(cinemaId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GenericResult<AuditoriumDomainModel>));
            Assert.AreEqual(expectedAuditoriumResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedAuditoriumResult.ErrorMessage, result.ErrorMessage);
        }



    }
}

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
    public class CinemaServiceTests
    {
        [TestClass]
        public class CinemasServiceTests
        {
            private Mock<ICinemasRepository> _mockCinemaRepository;
            private CinemaService _cinemaService;
            private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;

            [TestInitialize]
            public void TestInit()
            {
                _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
                _mockCinemaRepository = new Mock<ICinemasRepository>();
                _cinemaService = new CinemaService(_mockCinemaRepository.Object);
            }

            [TestMethod]
            public async Task GetAllCinemas_Returns_ListOfCinemas()
            {
                var expectedCinemas = new List<Data.Cinema>
            {
                new Data.Cinema
                {

                Id=1,
                Address="Perleska2",
                CityName="New York",
                Name="Cinestar"
                },
                new Data.Cinema
                {
                Id=2,
                Address="Perleska7",
                CityName="New York",
                Name="Cinestar"
                }
            };

                var expectedCinemaResult = new GenericResult<CinemaDomainModel>
                {
                    IsSuccessful = true,
                    DataList = new List<CinemaDomainModel>
                    {

                    }
                };

                foreach (var item in expectedCinemas)
                {
                    var cinemaDomainModel = new CinemaDomainModel
                    {
                        Address = item.Address,
                        CityName = item.CityName,
                        Id = item.Id,
                        Name = item.Name

                    };
                    expectedCinemaResult.DataList.Add(cinemaDomainModel);
                }

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedCinemas);
                var result = await _cinemaService.GetAllAsync();

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedCinemas[0].Id, result.DataList[0].Id);
                Assert.IsInstanceOfType(result.DataList[0], typeof(CinemaDomainModel));
            }

            [TestMethod]
            public async Task GetAllCinemas_Returns_EmptyList()
            {
                var expectedCinemas = new List<Data.Cinema>
                {

                };

                var expectedCinemaResult = new GenericResult<CinemaDomainModel>
                {
                    IsSuccessful = true,
                    DataList = new List<CinemaDomainModel>
                    {

                    }
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedCinemas);
                var result = await _cinemaService.GetAllAsync();

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedCinemas.Count, result.DataList.Count);
                Assert.IsInstanceOfType(result.DataList, typeof(List<CinemaDomainModel>));
                Assert.AreEqual(result.IsSuccessful, true);
            }

            [TestMethod]
            public async Task GetCinemaById_If_Id_Exists_Returns_User()
            {
                int cinemaId = 1;

                var expectedCinemas = new Data.Cinema
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                var expectedCinemaResult = new GenericResult<CinemaDomainModel>
                {
                    IsSuccessful = true,
                    Data = new CinemaDomainModel
                    {
                        Id = cinemaId,
                        Address = "Perleska2",
                        CityName = "New York",
                        Name = "Cinestar"
                    }
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(expectedCinemas);
                var result = await _cinemaService.GetCinemaById(cinemaId);

                //Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(GenericResult<CinemaDomainModel>));
                Assert.AreEqual(result.IsSuccessful, true);
                Assert.IsInstanceOfType(result.Data, typeof(CinemaDomainModel));
            }

            [TestMethod]
            public async Task GetCinemaById_If_Id_Is_Null_Returns_NotFound()
            {
                int userId = 0;
                var expectedErrorMessage = Messages.CINEMA_ID_NOT_FOUND;

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(userId)).ReturnsAsync(It.IsAny<Data.Cinema>);
                var result = await _cinemaService.GetCinemaById(userId);

                //Assert
                Assert.IsInstanceOfType(result, typeof(GenericResult<CinemaDomainModel>));
                Assert.AreEqual(result.ErrorMessage, expectedErrorMessage);
            }

            [TestMethod]
            public async Task AddCinema_Returns_Cinema()
            {
                var cinemaId = default(int);
                
                var cinemaToInsert = new Data.Cinema
               
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                var cinemaToCreate = new CinemaDomainModel
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                var expectedCinemaDomainModel = new GenericResult<CinemaDomainModel>
                {
                    IsSuccessful = true,
                    Data = new CinemaDomainModel
                    {
                        Id = cinemaId,
                        Address = "Perleska2",
                        CityName = "New York",
                        Name = "Cinestar"
                    }
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<Data.Cinema>())).ReturnsAsync(cinemaToInsert);
                var result = await _cinemaService.AddCinemaAsync(cinemaToCreate);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(result.Data.Id, expectedCinemaDomainModel.Data.Id);
                Assert.AreEqual(result.Data.Name, expectedCinemaDomainModel.Data.Name);
                Assert.IsInstanceOfType(result, typeof(GenericResult<CinemaDomainModel>));
            }

            [TestMethod]
            public async Task AddCinema_If_Id_Exists_Returns_Cinema_Already_Exists()
            {
                var cinemaId = 1;
               
                var errorMessage = "Cinema already exists";

                var cinema = new Data.Cinema
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                var cinemaToInsert = new CinemaDomainModel
                {
                    Id = 1,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<Data.Cinema>())).ReturnsAsync(It.IsNotNull<Data.Cinema>());
                _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(cinema);
                var result = await _cinemaService.AddCinemaAsync(cinemaToInsert);

                //Assert
                Assert.AreEqual(result.ErrorMessage, errorMessage);
                Assert.AreEqual(result.IsSuccessful, false);
            }

            [TestMethod]
            public async Task UpdateCinema_Returns_Updated_Cinema()
            {
                var cinemaId = 1;
            
                var cinemaToUpdate = new Data.Cinema
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                var cinemaToUpdateModel = new CinemaDomainModel
                {
                    Id = 1,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.Update(It.IsNotNull<Data.Cinema>())).Returns(cinemaToUpdate);
                _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaId)).ReturnsAsync(cinemaToUpdate);
                var result = await _cinemaService.UpdateCinema(cinemaToUpdateModel);

                //Assert
                Assert.IsInstanceOfType(result.Data, typeof(CinemaDomainModel));
                Assert.AreEqual(result.IsSuccessful, true);
            }

            [TestMethod]
            public async Task UpdateCinema_If_Cinema_Not_Found_Return_Error_Message()
            {
                var expectedErrorMessage = "Cinema not found";

                var cinemaToUpdateModel = new CinemaDomainModel
                {
                    Id = 0,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(cinemaToUpdateModel.Id)).ReturnsAsync(It.IsAny<Data.Cinema>);
                var result = await _cinemaService.UpdateCinema(cinemaToUpdateModel);

                //Assert
                Assert.IsInstanceOfType(result, typeof(GenericResult<CinemaDomainModel>));
                Assert.AreEqual(result.ErrorMessage, expectedErrorMessage);
            }

            [TestMethod]
            public async Task DeleteCinema_Returns__Cinema()
            {
                var cinemaId = 1;

                var cinemaToUpdate = new Data.Cinema
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                var cinemaToUpdateModel = new CinemaDomainModel
                {
                    Id = cinemaId,
                    Address = "Perleska2",
                    CityName = "New York",
                    Name = "Cinestar"
                };

                //Act
                _mockCinemaRepository.Setup(srvc => srvc.Delete(cinemaId)).Returns(cinemaToUpdate);
                var result =_cinemaService.DeleteCinema(cinemaToUpdateModel.Id);

                //Assert
                Assert.IsInstanceOfType(result.Data, typeof(CinemaDomainModel));
                Assert.AreEqual(result.IsSuccessful, true);
                Assert.AreEqual(result.Data.Id, cinemaId);
            }
        }
    }
}
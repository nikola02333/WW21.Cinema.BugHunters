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
        private AuditoriumService  _auditoriumService;

        [TestInitialize]
        public void TestInit()
        {
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _auditoriumService = new AuditoriumService(_mockAuditoriumRepository.Object,_mockCinemaRepository.Object,_mockSeatRepository.Object,_mockProjectionRepository.Object);
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
                  CinemaId=item.CinemaId,
                  Id=item.Id,
                   Name=item.AuditoriumName,
                   SeatsList=item.Seats.Select(s=>new SeatDomainModel
                   { 
                    Id=s.Id,
                    AuditoriumId=s.AuditoriumId,
                    Number=s.Number,
                    Row=s.Row                 
                   }).ToList()
                };
                expectedAuditoriumResult.DataList.Add(auditoriumDomainModel);
            }

            _mockAuditoriumRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedAuditoriums);
           
            //Act
            var result =await _auditoriumService.GetAllAsync();
          
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
                     Number = 3,
                     Row = 3
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

            GenericResult<AuditoriumDomainModel> resultModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Data = auditoriumModel
            };

            _mockCinemaRepository.Setup(srvc => srvc.GetByIdAsync(auditoriumToCreate.CinemaId)).ReturnsAsync(It.IsNotNull<Data.Cinema>());

            _mockAuditoriumRepository.Setup(srvc => srvc.GetByAuditoriumNameAsync(auditoriumToCreate.AuditoriumName, auditoriumToCreate.CinemaId)).ReturnsAsync(It.IsAny<IEnumerable<Auditorium>>);

            _mockAuditoriumRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<Auditorium>())).ReturnsAsync(auditoriumToCreate);

            //var result = _auditoriumService.CreateAuditorium(auditoriumModel, auditoriumModel.SeatsList);

        }

     


    }
}

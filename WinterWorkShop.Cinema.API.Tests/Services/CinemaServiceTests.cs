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
           


            [TestInitialize]
            public void TestInit()
            {
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
                    new CinemaDomainModel
                    {

                        Id = 1,
                        Address = "Perleska2",
                        CityName = "New York",
                        Name = "Cinestar"

                    },

                    new CinemaDomainModel
                    {

                        Id = 2,
                        Address = "Perleska67",
                        CityName = "New York",
                        Name = "Cinestar"
                        
                    }
                }
                };


               _mockCinemaRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedCinemas);

                var result = await _cinemaService.GetAllAsync();

                var cinemaResult = result;


                Assert.IsNotNull(cinemaResult);
                Assert.IsInstanceOfType(result, typeof(GenericResult<CinemaDomainModel>));
                Assert.AreEqual(expectedCinemaResult.IsSuccessful, cinemaResult.IsSuccessful);
              
             

            }


        }
        }
}

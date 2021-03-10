using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
   public class UsersServiceTests
    {
        private Mock<IUsersRepository> _mockUserRepository;
        private Mock<IUserService> _mockUserService;
        //private Mock<ILogger<MoviesController>> _mockLogger;


        [TestInitialize]
        public void TestInit()
        {
            _mockUserRepository = new Mock<IUsersRepository>();
            _mockUserService = new Mock<IUserService>(_mockUserRepository.Object);
        }


        [TestMethod]
        public async Task GetByUserNameAsync_Returns_User()
        {
            var userNameToFind = "nikola023";

            var expectedUser = new User
            {
                UserName="nikola023",
                 FirstName="Nikola",
                 Role="admin",
                  LastName="Velickovic",
                  Id=Guid.NewGuid(),
            };

            _mockUserRepository.Setup(srvc => srvc.GetByUserNameAsync(userNameToFind)).ReturnsAsync(expectedUser);

            //_mockUserService
            // Assert

           // Assert.IsInstanceOfType(result, typeof(User));
        }

    }
}

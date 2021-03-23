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
   public class UsersServiceTests
    {
        private Mock<IUsersRepository> _mockUserRepository;
        private UserService _userService;
 

        [TestInitialize]
        public void TestInit()
        {
            _mockUserRepository = new Mock<IUsersRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }


        [TestMethod]
        public async Task GetUserByUserNameAsync_Returns_User()
        {
            var userNameToFind = "nikola023";

            var expectedUser = new User
            {
                UserName = "nikola023",
                FirstName = "Nikola",
                Role = "admin",
                LastName = "Velickovic",
                Id = Guid.NewGuid(),
            };

            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
               IsSuccessful= true,
                Data= new UserDomainModel
                {
                    UserName = expectedUser.UserName,
                    FirstName = expectedUser.FirstName,
                    Role = expectedUser.Role,
                    LastName = expectedUser.LastName,
                    Id = expectedUser.Id,
                }
            };

            _mockUserRepository.Setup(srvc => srvc.GetByUserNameAsync(userNameToFind)).ReturnsAsync(expectedUser);
           
            //Act
            var result =await _userService.GetUserByUserNameAsync(userNameToFind);


            // Assert

            Assert.AreEqual(result.Data.Id, expectedUserDomainModel.Data.Id);
            Assert.IsInstanceOfType(result, typeof(GenericResult<UserDomainModel>));

        }
        [TestMethod]
        public async Task GetUserByUserNameAsync_Returns_User_Not_Found()
        {
            var userNameToFind = "nikola023";

            var expectedUser = new User
            {
                UserName = "nikola023",
                FirstName = "Nikola",
                Role = "admin",
                LastName = "Velickovic",
                Id = Guid.NewGuid(),
            };

            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    UserName = expectedUser.UserName,
                    FirstName = expectedUser.FirstName,
                    Role = expectedUser.Role,
                    LastName = expectedUser.LastName,
                    Id = expectedUser.Id,
                }
            };

            _mockUserRepository.Setup(srvc => srvc.GetByUserNameAsync(userNameToFind)).ReturnsAsync(expectedUser);

           
            //Act
            var result = await _userService.GetUserByUserNameAsync(userNameToFind);


            // Assert

            Assert.AreEqual(result.Data.Id, expectedUserDomainModel.Data.Id);
            Assert.IsInstanceOfType(result, typeof(GenericResult<UserDomainModel>));

        }

        [TestMethod]
        public async Task GetAllAsync_Returns_ListUsers()
        {

            var expectedUserList = new List<User>
            {
                new User{ 
                UserName = "nikola023",
                FirstName = "Nikola",
                Role = "admin",
                LastName = "Velickovic",
                Id = Guid.NewGuid(), 
                },
                new User{
                UserName = "nikola024",
                FirstName = "Nikola2",
                Role = "admin",
                LastName = "Velickovic2",
                Id = Guid.NewGuid(),
                }
            };


            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<UserDomainModel>
                {
                }
            };
            foreach (var item in expectedUserList)
            {
                var userDomaimModel = new UserDomainModel
                {
                     FirstName= item.FirstName,
                     UserName= item.UserName,
                      Id= item.Id,
                       LastName= item.LastName,
                        Role= item.Role
                };
                expectedUserDomainModel.DataList.Add(userDomaimModel);
            }

            _mockUserRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedUserList);

            //Act
            var resultList = await _userService.GetAllAsync();


            // Assert

            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedUserList.Count, resultList.DataList.Count);
            Assert.AreEqual(expectedUserList[0].Id, resultList.DataList[0].Id);
            Assert.IsInstanceOfType(resultList.DataList[0], typeof(UserDomainModel));

        }

        [TestMethod]
        public async Task GetAllAsync_Returns_Empty_List()
        {

            var expectedUserList = new List<User>
            {
               
            };


            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
               IsSuccessful=true,
                DataList = new List<UserDomainModel>
                {
                }
            };
           

            _mockUserRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedUserList);

            //Act
            var resultList = await _userService.GetAllAsync();


            // Assert

            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedUserList.Count, resultList.DataList.Count);
            Assert.AreEqual(resultList.IsSuccessful, true);
            Assert.IsInstanceOfType(resultList.DataList, typeof(List<UserDomainModel>));

        }
        [TestMethod]
        public async Task GetUserByIdAsync_Returns_User()
        {
            var userId = Guid.NewGuid();

            var expectedUser = new User
            {
                UserName = "nikola023",
                FirstName = "Nikola",
                Role = "admin",
                LastName = "Velickovic",
                Id = userId
            };

            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    UserName = expectedUser.UserName,
                    FirstName = expectedUser.FirstName,
                    Role = expectedUser.Role,
                    LastName = expectedUser.LastName,
                    Id = expectedUser.Id,
                }
            };

            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync(userId)).ReturnsAsync(expectedUser);

            //Act
            var result = await _userService.GetUserByIdAsync(userId);


            // Assert

            Assert.AreEqual(result.Data.Id, expectedUserDomainModel.Data.Id);
            Assert.IsInstanceOfType(result.Data, typeof(UserDomainModel));

        }

        [TestMethod]
        public async Task GetByIdAsync_When_Id_Null_Returns_NotFound()
        {
            var userId = Guid.NewGuid();

            var expectedMessage = Messages.USER_NOT_FOUND;
           

           

            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync(userId)).ReturnsAsync(It.IsAny<User>());

            //Act
            var result = await _userService.GetUserByIdAsync(userId);


            // Assert

            Assert.AreEqual(result.ErrorMessage, expectedMessage);
            Assert.AreEqual(result.IsSuccessful, false);


        }



        [TestMethod]
        public async Task CreateUserAsync_Returns_User()
        {
            var userId = Guid.NewGuid();
            var userToInsert = new User
            {
                UserName = "nikola023",
                FirstName = "Nikola",
                Role = "admin",
                LastName = "Velickovic",
                Id = userId
            };

            var userToCreate = new UserDomainModel
            {
                UserName = "nikola023",
                FirstName = "Nikola",
                Role = "admin",
                LastName = "Velickovic",
                Id = userId
            };
            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    UserName = userToInsert.UserName,
                    FirstName = userToInsert.FirstName,
                    Role = userToInsert.Role,
                    LastName = userToInsert.LastName,
                    Id = userToInsert.Id,
                }
            };

            _mockUserRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<User>())).ReturnsAsync(userToInsert);

            _mockUserRepository.Setup(srvc => srvc.CheckUsername(userToInsert.UserName)).ReturnsAsync(true);
            
            //Act

            var result = await _userService.CreateUserAsync(userToCreate);


            // Assert

            Assert.AreEqual(result.Data.Id, expectedUserDomainModel.Data.Id);
            Assert.IsInstanceOfType(result.Data, typeof(UserDomainModel));

        }

        [TestMethod]
        public async Task CreateUserAsync_When_UsernameExists_Returns_User_Creation_Error()
        {
            var userId = Guid.NewGuid();
            var expectedMessage = Messages.USER_CREATION_ERROR_USERNAME_EXISTS;

            var userToCreate = new UserDomainModel
            {
                UserName="nikola023"
            };

           
            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.USER_CREATION_ERROR_USERNAME_EXISTS
            };

            _mockUserRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<User>())).ReturnsAsync(It.IsNotNull<User>());

            _mockUserRepository.Setup(srvc => srvc.CheckUsername(It.IsNotNull<string>())).ReturnsAsync(false);
           
            //Act


            var result = await _userService.CreateUserAsync(userToCreate);


            // Assert

            Assert.AreEqual(result.ErrorMessage, expectedMessage);
            Assert.AreEqual(result.IsSuccessful, false);

        }

        [TestMethod]
        public async Task CheckUsername_When_Username_Exists_Return_False()
        {
            var exptectedResult = false;
            var username = "Nikola";
            _mockUserRepository.Setup(srvc => srvc.CheckUsername(username)).ReturnsAsync(false);

            //Act
            var result =await _userService.CheckUsername(username);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exptectedResult, result);
        }

        [TestMethod]
        public async Task CheckUsername_When_Username_Exists_Return_True()
        {
            var exptectedResult = true;
            var username = "Nikola";
            _mockUserRepository.Setup(srvc => srvc.CheckUsername(username)).ReturnsAsync(true);

            //Act
            var result =await _userService.CheckUsername(username);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exptectedResult, result);
        }

        [TestMethod]
        public async Task CreateUserAsync_When_Called_Retunr_User_Creation_Error()
        {
            var userId = Guid.NewGuid();
            var expectedMessage = Messages.USER_CREATION_ERROR;

            var userToCreate = new UserDomainModel
            {
                UserName = "nikola023",
                Role="amin"
                
            };


            var expectedUserDomainModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.USER_CREATION_ERROR
        };

            _mockUserRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<User>())).ReturnsAsync(default(User));

            _mockUserRepository.Setup(srvc => srvc.CheckUsername(It.IsNotNull<string>())).ReturnsAsync(true);
            
            //Act


            var result = await _userService.CreateUserAsync(userToCreate);


            // Assert

            Assert.AreEqual(result.ErrorMessage, expectedMessage);
            Assert.AreEqual(result.IsSuccessful, false);

        }

        [TestMethod]
        public async Task UpdateUserAsync_Returns_UpdatedUser()
        {
            var userId = Guid.NewGuid();


            var userToUpdate = new User
            {
                UserName="Nikola",
                Role= "admin"
            };
            var userToUpdateModel = new UserDomainModel
            {

                    UserName = userToUpdate.UserName,
                    Role="admin",
                    Id = userId,
            };

            _mockUserRepository.Setup(srvc => srvc.Update(It.IsNotNull<User>())).Returns(userToUpdate);

            

            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(userToUpdate);

            
            //Act
            var result = await _userService.UpdateUserAsync(userId, userToUpdateModel);

            _mockUserRepository.Verify(srvc => srvc.GetByIdAsync(It.IsNotNull<Guid>()),Times.Once);

            // Assert
            Assert.IsInstanceOfType(result.Data, typeof(UserDomainModel));
           

        }

        [TestMethod]
        public async Task UpdateUserAsync_When_User_Id_Not_Found_Returns_IsSuccesful_False()
        {
            var userId = Guid.NewGuid();

            var expectedMessage = Messages.USER_NOT_FOUND;
            var isSuccesful = false;

            var userToUpdate = new User
            {
                UserName = "Nikola",
                Role = "admin"
            };
            var userToUpdateModel = new UserDomainModel
            {

                UserName = userToUpdate.UserName,
                Role = "admin",
                Id = userId,
            };

            _mockUserRepository.Setup(srvc => srvc.Update(It.IsNotNull<User>())).Returns(userToUpdate);



            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync( default(User));


            //Act
            var result = await _userService.UpdateUserAsync(userId, userToUpdateModel);

            _mockUserRepository.Verify(srvc => srvc.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);

            // Assert
            Assert.AreEqual(expectedMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);


        }

        [TestMethod]
        public async Task DeleteUserAsync_When_Id_Not_Exists_Retuns_ErrorMessage()
        {
            var expectedMessage = Messages.USER_NOT_FOUND;
            var isSuccesful = false;
            var userIdToDelete = Guid.NewGuid();

            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync((Guid)userIdToDelete)).ReturnsAsync(default(User));

            var result =await _userService.DeleteUserAsync(userIdToDelete);

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ErrorMessage, expectedMessage);
            Assert.AreEqual(result.IsSuccessful, isSuccesful);

        }


        [TestMethod]
        public async Task DeleteUserAsync_When_Id_Exists_Returns_IsSuccesful_True()
        {
            var isSuccesful = true;
            var userIdToDelete = Guid.NewGuid();
            var userToReturn = new User();

            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync((Guid)userIdToDelete)).ReturnsAsync(userToReturn);

            //Act
            var result = await _userService.DeleteUserAsync(userIdToDelete);

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.IsSuccessful, isSuccesful);

        }

        [TestMethod]
        public async Task IncrementBonusPointsForUser_When_UserId_Not_Found_Returns_IsSuccesful_False()
        {
            var isSuccesful = false;
            var expectedMessage = Messages.USER_ID_NULL;

            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync(userId)).ReturnsAsync(default(User));

            

            var result = await _userService.IncrementBonusPointsForUser(userId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(GenericResult<UserDomainModel>));
            Assert.AreEqual(expectedMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);



        }

        [TestMethod]
        public async Task IncrementBonusPointsForUser_When_Called_Returns_IsSuccesful_True()
        {
            var isSuccesful = true;

            var userId = Guid.NewGuid();

            var userResult = new GenericResult<UserDomainModel>
            {
                IsSuccessful= true,

            };
            var userToReturn = new User
            {

            };
            _mockUserRepository.Setup(srvc => srvc.GetByIdAsync(userId)).ReturnsAsync(userToReturn);



            var result = await _userService.IncrementBonusPointsForUser(userId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(GenericResult<UserDomainModel>));
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }
    }
}

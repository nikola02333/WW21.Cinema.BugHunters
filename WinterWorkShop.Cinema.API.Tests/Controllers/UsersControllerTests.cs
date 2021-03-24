using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class UsersControllerTests
    {
        private Mock<IUserService> _mockUserService;

        //private Mock<ILogger<MoviesController>> _mockLogger;

        private UsersController _userController;

        [TestInitialize]
        public void TestInit()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UsersController(_mockUserService.Object);
        }

        [TestMethod]
        public async Task GetUsers_Return_All_Users()
        {
            //Arrange
            int expectedStatusCode = 200;

            var expectedUsers = new GenericResult<UserDomainModel>
            {
                DataList = new List<UserDomainModel>
                {
                    new UserDomainModel() { Id = Guid.NewGuid(),  FirstName = "Nikola",  LastName ="velickovic", Role= "admin",  UserName="nikolaNiki"},
                    new UserDomainModel() { Id = Guid.NewGuid(),  FirstName = "Marko",  LastName ="Markovic", Role= "user",  UserName="marko023"}
                }
            };


            _mockUserService.Setup(srvc => srvc.GetAllAsync())
                .ReturnsAsync(expectedUsers);

            // Act

            var result = await _userController.GetAsync();


            var usersResult = ((OkObjectResult)result.Result).Value;


            Assert.IsNotNull(usersResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedUsers.DataList, usersResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod]
        public async Task GetUsers_Return_New_List()
        {
            int expectedStatusCode = 200;

            var expectedUsers = new GenericResult<UserDomainModel>
            {
                DataList = new List<UserDomainModel>
                {
                }
            };


            _mockUserService.Setup(srvc => srvc.GetAllAsync())
                .ReturnsAsync(expectedUsers);

            // Act

            var result = await _userController.GetAsync();


            var usersResult = ((OkObjectResult)result.Result).Value;


            Assert.IsNotNull(usersResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedUsers.DataList, usersResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }
        [TestMethod]
        public async Task GetUserByID_When_Id_Exists_Returns_User()
        {
            int expectedStatusCode = 200;
            var expectedId = Guid.NewGuid();

            var expectedUser = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    Id = expectedId,
                    FirstName = "Nikola",
                    LastName = "Velickovic",
                    Role = "admin",
                    UserName = "nikola023"
                }
            };


            _mockUserService.Setup(srvc => srvc.GetUserByIdAsync(expectedId))
                .ReturnsAsync(expectedUser);

            // Act

            var result = await _userController.GetbyIdAsync(expectedId);

            //Assert
            var usersResult = ((OkObjectResult)result.Result).Value;


            Assert.IsNotNull(usersResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(expectedUser.Data, usersResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
        }



        [TestMethod]
        public async Task GetUserByID_When_Id_Is_Empty_Returns_BadRequest()
        {
            int expectedStatusCode = 400;
            var expectedId = Guid.Empty;
            var expectedErrorMessage = Messages.USER_NOT_FOUND;

            var expectedUser = new GenericResult<UserDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.USER_NOT_FOUND
            };


            _mockUserService.Setup(srvc => srvc.GetUserByIdAsync(expectedId))
                .ReturnsAsync(expectedUser);

            // Act

            var result = await _userController.GetbyIdAsync(expectedId);

            //Assert
            var usersResult = ((BadRequestObjectResult)result.Result).Value;

            Assert.IsNotNull(usersResult);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);

            var error = (ErrorResponseModel)usersResult;
            Assert.AreEqual(expectedErrorMessage, error.ErrorMessage);
        }

        [TestMethod]
        public async Task CreateUserAsync_When_ModelState_Is_Invalid_Returns_BadRrquest()
        {
            int expectedStatusCode = 400;
            var expectedErrorMessage = "The UserName field is required.";
            // Username 
            var userToCreate = new CreateUserModel
            {
                FirstName = "Nikola",
                LastName = "Velickovic",
                Role = "admin"
            };

            // Act
            _userController.ModelState.AddModelError("Username", "The UserName field is required.");
            var result = await _userController.CreateUserAsync(userToCreate);

      
            var usersResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResponse = ((SerializableError)usersResult).GetValueOrDefault("Username");
            var message = (string[])errorResponse;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(message[0], expectedErrorMessage);
            

            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }
        [TestMethod]
         public async Task CreateUserAsync_When_IsSuccesfull_False_Returns_BadRrquest()
        {
            string expectedMessage = "Error occured while creating new user, please try again.";
            int expectedStatusCode = 400;

            GenericResult<UserDomainModel> CreateUserResponseModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.USER_CREATION_ERROR 
            };

            var userToCreate = new CreateUserModel
            {
                FirstName = "Nikola",
                LastName = "Velickovic",
                Role = "admin"
            };

            _mockUserService.Setup(srvc => srvc.CreateUserAsync(It.IsAny<UserDomainModel>()))
                             .ReturnsAsync(CreateUserResponseModel);
            // Act
            var result = await _userController.CreateUserAsync(userToCreate);

            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;
            

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            var errorResult = (ErrorResponseModel)badObjectResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task CreateUserAsync_When_Called_Handles_Exception_Returns_BadRrquest()
        {
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            var userToCreate = new CreateUserModel
            {
                FirstName = "Nikola",
                LastName = "Velickovic",
                Role = "admin"
            };

            GenericResult<UserDomainModel> CreateUserResponseModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    FirstName= userToCreate.FirstName,
                    LastName= userToCreate.LastName,
                    Role = userToCreate.Role
                }
            };

            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);


            _mockUserService.Setup(srvc => srvc.CreateUserAsync(It.IsAny<UserDomainModel>()))
                             .Throws(dbUpdateException);

            // Act
            var result = await _userController.CreateUserAsync(userToCreate);


            var resultResponse = (BadRequestObjectResult)result.Result;
            var badObjectResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            Assert.AreEqual(errorResult.ErrorMessage, expectedMessage);

            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }


        [TestMethod]
        public async Task CreateUserAsync_When_Called_IsSuccessful_True_Returns_User()
        {
            int expectedStatusCode = 201;
            
            // Username 
            var userToCreate = new CreateUserModel
            {
                FirstName = "Nikola",
                LastName = "Velickovic",
                Role = "admin",
                UserName="nikola023"
            };

            GenericResult<UserDomainModel> CreateUserResponseModel = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    Id= Guid.NewGuid(),
                    FirstName = userToCreate.FirstName,
                    LastName = userToCreate.LastName,
                    Role = userToCreate.Role,
                    UserName= userToCreate.UserName
                }
            };

            _mockUserService.Setup(srvc => srvc.CreateUserAsync(It.IsAny<UserDomainModel>()))
                             .ReturnsAsync(CreateUserResponseModel);

            // Act
            var result = await _userController.CreateUserAsync(userToCreate);


            var resultResponse = (CreatedAtActionResult)result.Result;
            var userCreated = resultResponse.Value;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            Assert.AreEqual(userCreated, CreateUserResponseModel.Data);

            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public async Task GetByUserNameAsync_When_Username_IsEmty_Returns_BadRequest()
        {
            int expectedStatusCode = 400;
            var expectedMessage = Messages.USER_NOT_FOUND;
           
            
            _mockUserService.Setup(srvc => srvc.GetUserByUserNameAsync(It.IsAny<string>()))
                             .ReturnsAsync(It.IsAny<GenericResult<UserDomainModel>>());

            // Act
            var result = await _userController.GetbyUserNameAsync(null);


            var resultObjectResponse = (BadRequestObjectResult)result.Result;
            var responseMessage = resultObjectResponse.Value;

            var errorMessage = (ErrorResponseModel)responseMessage;
            var errorStatusCode = (BadRequestObjectResult)result.Result;

           
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(errorMessage.ErrorMessage, expectedMessage);

            Assert.AreEqual(expectedStatusCode, resultObjectResponse.StatusCode);
            
        }
        [TestMethod]
        public async Task GetByUserNameAsync_When_Called_Returns_User()
        {
            int expectedStatusCode = 200;

            GenericResult<UserDomainModel> userExpected = new GenericResult<UserDomainModel>
            {
                IsSuccessful=true,
                Data= new UserDomainModel
                {
                    UserName="nikola",
                     FirstName ="Nikola",
                      Id= Guid.NewGuid(),
                       LastName="Velickovic",
                        Role= "admin"
                }
            };



            _mockUserService.Setup(srvc => srvc.GetUserByUserNameAsync(It.IsAny<string>()))
                             .ReturnsAsync(userExpected);

            // Act
            var result = await _userController.GetbyUserNameAsync(userExpected.Data.UserName);


            var resultResponse = (OkObjectResult)result.Result;
            var userResponse = resultResponse.Value;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(userResponse, userExpected.Data);

            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);

        }
        [TestMethod]
        public async Task DeleteUser_When_UserId_Is_Empty_Return_BadRequest()
        {
            int expectedStatusCode = 400;

            var userId = Guid.Empty;

            // Act
            var result = await _userController.DeleteUser(userId);


            var resultResponse = (BadRequestObjectResult)result.Result;
            var userResponse = resultResponse.Value;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
           

            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }


        [TestMethod]
        public async Task DeleteUser_When_Called_Returns_Accepted()
        {
            int expectedStatusCode = 202;

            var userId = Guid.NewGuid();

            var DeletedUser = new GenericResult<UserDomainModel>
            {
                IsSuccessful= true,
                Data= new UserDomainModel()
                {
                    Id= userId
                }
            };


             _mockUserService.Setup(srvc => srvc.DeleteUserAsync(userId))
                              .ReturnsAsync(DeletedUser);
            
            // Act
            var result = await _userController.DeleteUser(userId);


            var resultResponse = (AcceptedResult)result.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));

            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public async Task UpdateUser_When_Id_Is_Empty_Returns_BadRequest()
        {
            int expectedStatusCode = 400;
            var expectedMessage = Messages.USER_ID_NULL;
            var userId = Guid.Empty;

            // Act
            var result = await _userController.DeleteUser(userId);

            var resultObjectResponse = (BadRequestObjectResult)result.Result;

            var errorMessage = (ErrorResponseModel)resultObjectResponse.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            Assert.AreEqual(expectedStatusCode, resultObjectResponse.StatusCode);
            Assert.AreEqual(expectedMessage, errorMessage.ErrorMessage);
        }

        [TestMethod]
        public async Task UpdateUser_When_ModelState_IsInvalid_Returns_BadRequest()
        {
            int expectedStatusCode = 400;
            var expectedMessage = Messages.USER_CREATION_ERROR;
            var userId = Guid.NewGuid();


            _userController.ModelState.AddModelError("Username", Messages.USER_CREATION_ERROR);

            var invalidUserUpdate = new CreateUserModel
            {
                FirstName="firstName",
                LastName="Lastname",
                 Role="user"
            };

            // Act
            var result = await _userController.UpdateUserAsync(userId,invalidUserUpdate);

            var usersResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResponse = ((SerializableError)usersResult).GetValueOrDefault("Username");
            var message = (string[])errorResponse;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(message[0], expectedMessage);


            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task UpdateUser_When_Called_Returns_Accepted()
        {
            int expectedStatusCode = 202;
            var userId = Guid.NewGuid();


            var updatedUserResult = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,
            };

            var UserUpdate = new CreateUserModel
            {
                FirstName = "firstName",
                LastName = "Lastname",
                Role = "user",
                UserName= "username"
            };

            _mockUserService.Setup(srvc => srvc.UpdateUserAsync(userId, It.IsAny<UserDomainModel>()))
                            .ReturnsAsync(updatedUserResult);

            // Act
            var result = await _userController.UpdateUserAsync(userId, UserUpdate);

            var usersResult = ((AcceptedResult)result.Result).Value;

            var StatusCode = (AcceptedResult)result.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, StatusCode.StatusCode);
                
        }

        [TestMethod]
        public async Task IncrementBonusPointsForUser_When_UserId_Not_Guid_Returns_BadRequest()
        {

            var expectedStatusCode = 400;
            var expectedMessage = Messages.USER_ID_NULL;

            // ACT
            var result = await _userController.IncrementBonusPointsForUser(Guid.Empty);


            var usersResult = ((BadRequestObjectResult)result.Result).Value;

            var message = (ErrorResponseModel)usersResult;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            Assert.AreEqual(expectedMessage, message.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }

        [TestMethod]
        public async Task IncrementBonusPointsForUser_When_Called_Retun_Acepted()
        {

            var expectedStatusCode = 202;
            var userId = Guid.NewGuid();
            var incrementResult = new GenericResult<UserDomainModel>
            {
                IsSuccessful = true,

            };
            _mockUserService.Setup(srvc => srvc.IncrementBonusPointsForUser(userId)).ReturnsAsync(incrementResult);
            // ACT
            var result = await _userController.IncrementBonusPointsForUser(userId);


            var usersResult = ((AcceptedResult)result.Result);


            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));

            Assert.AreEqual(expectedStatusCode, usersResult.StatusCode);
        }

        [TestMethod]
        public async Task IncrementBonusPointsForUser_When_IsSuccessful_False_Retunrs_BadRerqest()
        {

            var expectedStatusCode = 400;
            var expectedMessage = Messages.USER_INCREMENT_POINTS_ERROR;
            var userId = Guid.NewGuid();
            var incrementResult = new GenericResult<UserDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage= Messages.USER_INCREMENT_POINTS_ERROR

            };
            _mockUserService.Setup(srvc => srvc.IncrementBonusPointsForUser(userId)).ReturnsAsync(incrementResult);
            // ACT
            var result = await _userController.IncrementBonusPointsForUser(userId);


            var usersResult = ((BadRequestObjectResult)result.Result).Value;

            var message = (ErrorResponseModel)usersResult;
            var errorStatusCode = (BadRequestObjectResult)result.Result;
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedMessage, message.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
        }
    }
}
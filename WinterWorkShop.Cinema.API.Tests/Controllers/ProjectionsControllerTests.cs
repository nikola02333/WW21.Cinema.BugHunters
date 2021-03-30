using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ProjectionsControllerTests
    {
        private Mock<IProjectionService> _mockProjectionService;
        ProjectionsController _projectionsController;

        [TestInitialize]
        public void TestInit()
        {
            _mockProjectionService = new Mock<IProjectionService>();
            _projectionsController = new ProjectionsController(_mockProjectionService.Object);
        }

        [TestMethod]
        public void GetAsync_Return_All_Projections()
        {
            //Arrange
            List<ProjectionDomainModel> projectionsDomainModelsList = new List<ProjectionDomainModel>();
            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };
            projectionsDomainModelsList.Add(projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projectionDomainModels = projectionsDomainModelsList;
            Task<IEnumerable<ProjectionDomainModel>> responseTask = Task.FromResult(projectionDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

<<<<<<< HEAD
            _mockProjectionService.Setup(x => x.GetAllAsync(true)).Returns(responseTask);
=======
            _mockProjectionService.Setup(x => x.GetAllAsync(false)).Returns(responseTask);
>>>>>>> origin/development
           
            //Act
            var result = _projectionsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
         
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_NewList()
        {
            //Arrange
            IEnumerable<ProjectionDomainModel> projectionDomainModels = null;
            Task<IEnumerable<ProjectionDomainModel>> responseTask = Task.FromResult(projectionDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

<<<<<<< HEAD
            _mockProjectionService.Setup(x => x.GetAllAsync(true)).Returns(responseTask);
=======
            _mockProjectionService.Setup(x => x.GetAllAsync(false)).Returns(responseTask);
>>>>>>> origin/development

            //Act
            var result = _projectionsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - false
        // try  await _projectionService.CreateProjection(domainModel) - return valid mock
        // if (!createProjectionResultModel.IsSuccessful) - false
        // return Created
        [TestMethod]
        public void PostAsync_Create_createProjectionResultModel_IsSuccessful_True_Projection() 
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };
            GenericResult<ProjectionDomainModel> createProjectionResultModel = new GenericResult<ProjectionDomainModel>
            {
                Data = new ProjectionDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumName = "ImeSale",
                    AuditoriumId = createProjectionModel.AuditoriumId,
                    MovieId = createProjectionModel.MovieId,
                    MovieTitle = "ImeFilma",
                    ProjectionTime = createProjectionModel.ProjectionTime
                },
                IsSuccessful = true
            };
            Task<GenericResult<ProjectionDomainModel>> responseTask = Task.FromResult(createProjectionResultModel);


            
            _mockProjectionService.Setup(x => x.CreateProjection(It.IsAny<ProjectionDomainModel>())).Returns(responseTask);
            

            //Act
            var result = _projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var projectionDomainModel = (ProjectionDomainModel)createdResult;

            //Assert
            Assert.IsNotNull(projectionDomainModel);
            Assert.AreEqual(createProjectionModel.MovieId, projectionDomainModel.MovieId);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }

        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - false
        // try  await _projectionService.CreateProjection(domainModel) - throw DbUpdateException
        // return BadRequest
        [TestMethod]
        public void PostAsync_Create_Throw_DbException_Projection()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };
            CreateProjectionResultModel createProjectionResultModel = new CreateProjectionResultModel
            {
                Projection = new ProjectionDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumName = "ImeSale",
                    AuditoriumId = createProjectionModel.AuditoriumId,
                    MovieId = createProjectionModel.MovieId,
                    MovieTitle = "ImeFilma",
                    ProjectionTime = createProjectionModel.ProjectionTime
                },
                IsSuccessful = true
            };
            Task<CreateProjectionResultModel> responseTask = Task.FromResult(createProjectionResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

          
            _mockProjectionService.Setup(x => x.CreateProjection(It.IsAny<ProjectionDomainModel>())).Throws(dbUpdateException);
           

            //Act
            var result = _projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }


        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - false
        // try  await _projectionService.CreateProjection(domainModel) - return valid mock
        // if (!createProjectionResultModel.IsSuccessful) - true
        // return BadRequest
        [TestMethod]
        public void PostAsync_Create_createProjectionResultModel_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Error occured while creating new projection, please try again.";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };
            GenericResult<ProjectionDomainModel> createProjectionResultModel = new GenericResult<ProjectionDomainModel>
            {
                Data = new ProjectionDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumName = "ImeSale",
                    AuditoriumId = createProjectionModel.AuditoriumId,
                    MovieId = createProjectionModel.MovieId,
                    MovieTitle = "ImeFilma",
                    ProjectionTime = createProjectionModel.ProjectionTime
                },
                IsSuccessful = false,
                ErrorMessage = Messages.PROJECTION_CREATION_ERROR,
            };
            Task<GenericResult<ProjectionDomainModel>> responseTask = Task.FromResult(createProjectionResultModel);


            
            _mockProjectionService.Setup(x => x.CreateProjection(It.IsAny<ProjectionDomainModel>())).Returns(responseTask);
           

            //Act
            var result = _projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        // if (!ModelState.IsValid) - true
        // return BadRequest
        [TestMethod]
        public void PostAsync_With_UnValid_ModelState_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 0
            };

            
            _projectionsController.ModelState.AddModelError("key","Invalid Model State");

            //Act
            var result = _projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createdResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = ((SerializableError)createdResult).GetValueOrDefault("key");
            var message = (string[])errorResponse;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - true
        // return BadRequest
        [TestMethod]
        public void PostAsync_With_UnValid_ProjectionDate_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Projection time cannot be in past.";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(-1),
                AuditoriumId = 0
            };

            //Act
            var result = _projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createdResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = ((SerializableError)createdResult).GetValueOrDefault(nameof(createProjectionModel.ProjectionTime));
            var message = (string[])errorResponse;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public async Task GetFilteredProjection_Return_Filtered_Projections()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 1;
            var date = DateTime.Now;
            var movieId = Guid.NewGuid();
            var filterModel = new FilterProjectionModel
            {
                AuditoriumId = 1,
                CinemaId = 1,
                DateTime = date,
                MovieId = movieId
            };
            

            var responce = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                DataList =new List<ProjectionDomainModel>{
                    new ProjectionDomainModel
                    {
                        Id=Guid.NewGuid(),
                        AuditoriumName="Naziv",
                        AuditoriumId=(int)filterModel.AuditoriumId,
                        Duration=100,
                        MovieId=movieId,
                        MovieTitle="nazivfilma",
                        Price=300,
                        ProjectionTime=date
                    }
                }
            };

            _mockProjectionService.Setup(srvc => srvc.FilterProjectionAsync(It.IsAny<FilterProjectionDomainModel>())).ReturnsAsync(responce);

            //Act
            var result =await _projectionsController.GetFilteredProjection(filterModel);
            var projectionResult = ((OkObjectResult)result.Result).Value;
            var projectionDomainModelResultList = (List<ProjectionDomainModel>)projectionResult;

            //Assert

            Assert.IsNotNull(projectionResult);
            Assert.AreEqual(expectedResultCount, projectionDomainModelResultList.Count);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreSame(responce.DataList, projectionResult);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }

        [TestMethod]
        public async Task GetFilteredProjection_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
           
            var filterModel = new FilterProjectionModel
            {
                AuditoriumId = 1,
                CinemaId = 1,
            };

            var responce = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.CINEMA_ID_NOT_FOUND
            };

            _mockProjectionService.Setup(srvc => srvc.FilterProjectionAsync(It.IsAny<FilterProjectionDomainModel>())).ReturnsAsync(responce);

            //Act
            var result =await _projectionsController.GetFilteredProjection(filterModel);
            var projectionResult = ((BadRequestObjectResult)result.Result).Value;
            var errorResponce = (ErrorResponseModel)projectionResult;

            //Assert

            Assert.IsNotNull(projectionResult);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result.Result).StatusCode);
            Assert.AreEqual(errorResponce.ErrorMessage, responce.ErrorMessage);

        }

        [TestMethod]
        public async Task GetByIdAsync_If_IsSuccessful_False_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var expectedErrorMassage = Messages.PROJECTION_GET_BY_ID;
            GenericResult<ProjectionDomainModel> projectionDomainModel = new GenericResult<ProjectionDomainModel>
           {
              IsSuccessful=false,
              ErrorMessage= Messages.PROJECTION_GET_BY_ID
           };

            _mockProjectionService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(projectionDomainModel);

            //Act
            var result =await _projectionsController.GetByIdAsync(It.IsAny<Guid>());

            var resultStatusCode = (BadRequestObjectResult)result.Result;
            var resultErrorMassage = (ErrorResponseModel)resultStatusCode.Value;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStatusCode, resultStatusCode.StatusCode);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedErrorMassage, resultErrorMassage.ErrorMessage);
        }


        [TestMethod]
        public async Task GetByIdAsync_If_IsSuccessful_True_Returns_OkObjectResult()
        {
            //Arrange
            int expectedStatusCode = 200;
 
            GenericResult<ProjectionDomainModel> projectionDomainModel = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
               Data=new ProjectionDomainModel { }
            };

            _mockProjectionService.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(projectionDomainModel);

            //Act
            var result = await _projectionsController.GetByIdAsync(It.IsNotNull<Guid>());

            var resultStatusCode = (OkObjectResult)result.Result;
           
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStatusCode, resultStatusCode.StatusCode);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
          
        }

    }
}

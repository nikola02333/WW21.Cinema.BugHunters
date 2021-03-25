using Microsoft.AspNetCore.Mvc;
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
   public class MoviesControllerTests
    {
        private Mock<IMovieService> _mockMovieService;
        private Mock<ILogger<MoviesController>> _mockLogger;
        private MoviesController _moviesController;

        [TestInitialize]
        public void TestInit()
        {
            _mockMovieService = new Mock<IMovieService>();
            _mockLogger = new Mock<ILogger<MoviesController>>();
            _moviesController = new MoviesController(_mockLogger.Object, _mockMovieService.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_When_ID_Null_Retuns_NotFound()
        {
            //Arrange
            int expectedStatusCode = 404;
            var expectedErrorMessage = Messages.MOVIE_GET_BY_ID;
            var movieId = Guid.Empty;

            var expectedMovieResult = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage= Messages.MOVIE_GET_BY_ID
            };

            _mockMovieService.Setup(src => src.GetMovieByIdAsync(movieId)).ReturnsAsync(expectedMovieResult);


            //Act

            var result = await _moviesController.GetByIdAsync(movieId);

            //Assert
            var movieResult = ((NotFoundObjectResult)result.Result).Value;
            var error = (ErrorResponseModel)movieResult;

            Assert.IsNotNull(movieResult);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));

            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result.Result).StatusCode);

          
            Assert.AreEqual(expectedErrorMessage, error.ErrorMessage);

        }

        [TestMethod]
        public async Task GetByIdAsync_When_IsSuccessful_False_Retuns_NotFound()
        {
            //Arrange
            int expectedStatusCode = 404;
            var expectedErrorMessage = Messages.MOVIE_DOES_NOT_EXIST;
            var movieId = Guid.NewGuid();

            var expectedMovie = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
            };


            _mockMovieService.Setup(src => src.GetMovieByIdAsync(movieId)).ReturnsAsync(expectedMovie);


            //Act

            var result = await _moviesController.GetByIdAsync(movieId);

            //Assert
            var movieResult = ((NotFoundObjectResult)result.Result).Value;
            var error = (ErrorResponseModel)movieResult;

            Assert.IsNotNull(movieResult);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));

            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result.Result).StatusCode);


            Assert.AreEqual(expectedErrorMessage, error.ErrorMessage);

        }

        [TestMethod]
        public async Task GetByIdAsync_When_IsSuccessful_True_Retuns_Movie()
        {
            //Arrange
            int expectedStatusCode = 200;
            var movieId = Guid.NewGuid();

            var Movie = new MovieDomainModel
            {
                Id = movieId,
                 Current= true,
                  Genre= "comedy",
                   Rating= 10,
                    Title="New Movie",
                     Year= 1999
            };

            var expectedMovie = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data = Movie
            };


            _mockMovieService.Setup(src => src.GetMovieByIdAsync(movieId)).ReturnsAsync(expectedMovie);


            //Act

            var result = await _moviesController.GetByIdAsync(movieId);

            //Assert
            var movieResult = ((OkObjectResult)result.Result).Value;

            Assert.IsNotNull(movieResult);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            var movie = (MovieDomainModel)movieResult;
            Assert.AreEqual(movie.Id, movieId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);

        }

        [TestMethod]
        public async Task GetAllAsync_Returns_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;

        

            var expectedMovies = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<MovieDomainModel>()
            };


            _mockMovieService.Setup(src => src.GetAllMoviesAsync(false)).ReturnsAsync(expectedMovies);


            //Act

            var result = await _moviesController.GetAllAsync(false);

            var movieResult = ((OkObjectResult)result.Result).Value;

            //Assert

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
            Assert.AreSame(expectedMovies.DataList, movieResult);

        }

        [TestMethod]
        public async Task GetAllAsync_Returns_Movies()
        {
            //Arrange
            int expectedStatusCode = 200;



            var expectedMovies = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<MovieDomainModel>
                {
                    new MovieDomainModel{ Current= false, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie1", Year=1999},
                     new MovieDomainModel{ Current= false, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie2", Year=1999},
                }
            };


            _mockMovieService.Setup(src => src.GetAllMoviesAsync(false)).ReturnsAsync(expectedMovies);


            //Act

            var result = await _moviesController.GetAllAsync(false);

            var movieResult = ((OkObjectResult)result.Result).Value;

            //Assert

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
            Assert.AreSame(expectedMovies.DataList, movieResult);

        }

        [TestMethod]
        public async Task GetAllCurrentMoviesAsync_Returns_Movies()
        {
            //Arrange
            int expectedStatusCode = 200;



            var expectedMovies = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<MovieDomainModel>
                {
                    new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie1", Year=1999},
                     new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie2", Year=1999},
                }
            };


            _mockMovieService.Setup(src => src.GetAllMoviesAsync(true)).ReturnsAsync(expectedMovies);


            //Act

            var result = await _moviesController.GetAllAsync(true);

            var movieResult = ((OkObjectResult)result.Result).Value;

            //Assert

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result.Result).StatusCode);
            Assert.AreSame(expectedMovies.DataList, movieResult);

        }

        [TestMethod]
        public async Task GetTopTenMoviesAsync_Returns_EmptyList()
        {
            //Arrange
            int expectedStatusCode = 200;



            var expectedMovies = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<MovieDomainModel>()
               
            };


            _mockMovieService.Setup(src => src.GetTopTenMoviesAsync()).ReturnsAsync(expectedMovies);


            //Act

            var result = await _moviesController.GetTopTenMoviesAsync();

            var movieResult = ((OkObjectResult)result).Value;

            //Assert

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
            Assert.AreSame(expectedMovies.DataList, movieResult);

        }


        [TestMethod]
        public async Task GetTopTenMoviesAsync_Returns_TopTenMovies()
        {
            //Arrange
            int expectedStatusCode = 200;



            var expectedMovies = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<MovieDomainModel>
                {
                       new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie1", Year=1999},
                     new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 7, Title="New_Movie2", Year=1999},
                        new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 6, Title="New_Movie3", Year=1999},
                     new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 5, Title="New_Movie4", Year=1999},
                        new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 4, Title="New_Movie5", Year=1999},
                     new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 3, Title="New_Movie6", Year=1999},
                        new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 2, Title="New_Movie7", Year=1999},
                     new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie8", Year=1999},
                        new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie9", Year=1999},
                     new MovieDomainModel{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie10", Year=1999},
                }

            };

            _mockMovieService.Setup(src => src.GetTopTenMoviesAsync()).ReturnsAsync(expectedMovies);

            //Act

            var result = await _moviesController.GetTopTenMoviesAsync();

            var movieResult = ((OkObjectResult)result).Value;

            //Assert

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
            Assert.AreSame(expectedMovies.DataList, movieResult);

        }

        [TestMethod]
        public async Task CreateMovieAsync_When_ModelState_Is_Invalid_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;

            var expectedErrorMessage = "The Title field is required.";

            var movieToCreate = new CreateMovieModel
            {
                 Rating=8,
                  Current= true,
                   Genre="comedy"
            };

            //Act

            _moviesController.ModelState.AddModelError("Title", "The Title field is required.");

            var result = await _moviesController.CreateMovieAsync(movieToCreate);


            var movieResult = ((BadRequestObjectResult)result.Result).Value;

            var errorResponse = ((SerializableError)movieResult).GetValueOrDefault("Title");
            var message = (string[])errorResponse;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(message[0], expectedErrorMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);

        }

        [TestMethod]
        public async Task CreateMovieAsync_When_IsSuccesfull_False_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;

            var expectedErrorMessage = Messages.MOVIE_CREATION_ERROR;

            var movieToCreate = new CreateMovieModel
            {
                Rating = 8,
                Current = true,
                Genre = "comedy",
                 Title="new Movie",
                  Year= 1994
            };
            

            var movieDomainModel = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_CREATION_ERROR
            };
            //Act

            _mockMovieService.Setup(srvc => srvc.AddMovieAsync(It.IsNotNull<MovieDomainModel>())).ReturnsAsync(movieDomainModel);


            var result = await _moviesController.CreateMovieAsync(movieToCreate);


            var movieResult = ((BadRequestObjectResult)result.Result).Value;

            var errorMessage = (ErrorResponseModel)movieResult;
            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
            Assert.AreEqual(expectedErrorMessage, errorMessage.ErrorMessage);

        }

        [TestMethod]
        public async Task CreateMovieAsync_When_IsSuccesfull_True_Returns_Movie()
        {
            //Arrange
            int expectedStatusCode = 201;
            var isSuccesfull = true;
            var movieToCreate = new CreateMovieModel
            {
                
                Rating = 8,
                Current = true,
                Genre = "comedy",
                Title = "new Movie",
                Year = 1994
            };


            var movieDomainModel = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data = new MovieDomainModel
                {
                     Current= movieToCreate.Current,
                    Rating = movieToCreate.Rating,
                     Genre= movieToCreate.Genre,
                    Id = Guid.NewGuid(),
                     Title= movieToCreate.Title,
                      Year= movieToCreate.Year
                }
            };
            //Act

            _mockMovieService.Setup(srvc => srvc.AddMovieAsync(It.IsNotNull<MovieDomainModel>())).ReturnsAsync(movieDomainModel);

            var result = await _moviesController.CreateMovieAsync(movieToCreate);


            var movieResult = ((CreatedAtActionResult)result.Result).Value;

            var errorStatusCode = (CreatedAtActionResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
           Assert.AreEqual(movieDomainModel.IsSuccessful, isSuccesfull);

        }


        [TestMethod]
        public async Task UpdateMovieAsync_When_ModelState_Is_Invalid_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;

            var expectedErrorMessage = "The Title field is required.";

            var movieToUpdate = new UpdateMovieModel
            {
                Rating = 8,
                Current = true,
            };

            //Act

            _moviesController.ModelState.AddModelError("Title", "The Title field is required.");

            var result = await _moviesController.UpdateMovieAsync(Guid.NewGuid(),movieToUpdate);


            var movieResult = ((BadRequestObjectResult)result.Result).Value;

            var errorResponse = ((SerializableError)movieResult).GetValueOrDefault("Title");
            var message = (string[])errorResponse;

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(message[0], expectedErrorMessage);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);

        }

        [TestMethod]
        public async Task UpdateMovieAsync_When_Id_Not_Found_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var expectedMessage = Messages.MOVIE_DOES_NOT_EXIST;
            var userId = Guid.NewGuid();
            var movieToUpdateModel = new UpdateMovieModel
            {
                 
            };
            var movieToUpdate = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage= Messages.MOVIE_DOES_NOT_EXIST
            };

            //Act

            _mockMovieService.Setup(srvc => srvc.GetMovieByIdAsync(userId)).ReturnsAsync(movieToUpdate);


            var result =await _moviesController.UpdateMovieAsync(Guid.Empty, movieToUpdateModel);

            var movieResultMessage = ((BadRequestObjectResult)result.Result).Value;


            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);
            Assert.AreEqual(expectedMessage, movieResultMessage);

        }

        [TestMethod]
        public async Task UpdateMovieAsync_When_IsSuccessful_False_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;

            var movieToUpdateModaimMode = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_UPDATE_ERROR
            };
            //Act

            _mockMovieService.Setup(srvc => srvc.GetMovieByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(movieToUpdateModaimMode);


            var result = await _moviesController.UpdateMovieAsync(Guid.NewGuid(), It.IsNotNull<UpdateMovieModel>());


            var movieResultMessage = ((BadRequestObjectResult)result.Result).Value;

            var errorMessage = (ErrorResponseModel)movieResultMessage;
            var statusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, statusCode.StatusCode);
            Assert.AreEqual(movieToUpdateModaimMode.ErrorMessage, errorMessage.ErrorMessage);

        }

        [TestMethod]
        public async Task UpdateMovieAsync_When_IsSuccessful_True_Returns_Accepted()
        {
            //Arrange
            int expectedStatusCode = 202;

            var movieToUpdateModaimMode = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data = new MovieDomainModel
                {
                    Current = false,
                    Genre = "comedy",
                    Rating = 8,
                    Title = "New Movie",
                    Year = 1994,
                }
            };
            var UpdateMovieModel = new UpdateMovieModel
            {
                Current= false,
                  Rating= 8,
                   Title="New Movie",
                    Year=1994,
            };

            //Act

            _mockMovieService.Setup(srvc => srvc.GetMovieByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(movieToUpdateModaimMode);


            var result = await _moviesController.UpdateMovieAsync(Guid.NewGuid(), UpdateMovieModel);



            var statusCode = (AcceptedResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, statusCode.StatusCode);

        }

        [TestMethod]
        public async Task DeleteMovieAsync_When_Id_Empty_Retunrs_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var expectedMessage = Messages.MOVIE_DELETE_ERROR;

            var movieToDeleteModaimMode = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data = new MovieDomainModel
                {
                    Current = false,
                    Genre = "comedy",
                    Rating = 8,
                    Title = "New Movie",
                    Year = 1994,
                }
            };


            var result = await _moviesController.Delete(It.IsNotNull<Guid>());


            var movieResult = ((BadRequestObjectResult)result.Result).Value;

           

            var errorStatusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedMessage, movieResult);
            Assert.AreEqual(expectedStatusCode, errorStatusCode.StatusCode);


        }

        [TestMethod]
        public async Task DeleteMovieAsync_When_IsSuccessful_False_Returns_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;
            var userId = Guid.NewGuid();

            var movieToDeleteModel = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_DELETE_ERROR
            };
            //Act

            _mockMovieService.Setup(srvc => srvc.DeleteMovieAsync(userId)).ReturnsAsync(movieToDeleteModel);


            var result = await _moviesController.Delete(userId);


            var movieResultMessage = ((BadRequestObjectResult)result.Result).Value;

            var errorMessage = (ErrorResponseModel)movieResultMessage;
            var statusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, statusCode.StatusCode);
            Assert.AreEqual(movieToDeleteModel.ErrorMessage, errorMessage.ErrorMessage);

        }

        [TestMethod]
        public async Task DeleteMovieAsync_When_IsSuccessful_True_Returns_Accepted()
        {
            //Arrange
            int expectedStatusCode = 202;
            var userId = Guid.NewGuid();

            var movieToDeleteModel = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true
                
            };
            //Act

            _mockMovieService.Setup(srvc => srvc.DeleteMovieAsync(userId)).ReturnsAsync(movieToDeleteModel);


            var result = await _moviesController.Delete(userId);

            var movieResultMessage = ((AcceptedResult)result.Result);


            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
           Assert.AreEqual(expectedStatusCode, movieResultMessage.StatusCode);

        }
        // DBException
        // i ovaj drugi
        [TestMethod]
        public async Task ActivateMovie_When_Movie_Has_Upcoming_Projection_Returns_BadRequest()
        {
            var expectedStatusCode = 400;
            var movieId = Guid.NewGuid();

            var moviToReturn = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_ACTIVATE_DEACTIVATE_ERROR
            };
            _mockMovieService.Setup(srvc => srvc.ActivateDeactivateMovie(movieId)).ReturnsAsync(moviToReturn);

            //Act
            var result = await _moviesController.ActivateMovie(movieId);



            var movieResultMessage = ((BadRequestObjectResult)result.Result).Value;

            var message = (ErrorResponseModel)movieResultMessage;
            var statusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, statusCode.StatusCode);
            Assert.AreEqual(moviToReturn.ErrorMessage, message.ErrorMessage);


        }

        [TestMethod]
        public async Task ActivateMovie_When_Movie_MovieId_Empty_Returns_BadRequest()
        {
            var expectedStatusCode = 400;
            var expectedMessage = Messages.MOVIE_DOES_NOT_EXIST;
            var movieId = Guid.Empty;



            //Act
            var result = await _moviesController.ActivateMovie(movieId);



            var movieResultMessage = ((BadRequestObjectResult)result.Result).Value;

            var message = (ErrorResponseModel)movieResultMessage;
            var statusCode = (BadRequestObjectResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, statusCode.StatusCode);
            Assert.AreEqual(expectedMessage, message.ErrorMessage);


        }

        [TestMethod]
        public async Task ActivateMovie_Returns_Accepted()
        {
            var expectedStatusCode = 202;
            var movieId = new Guid("0d9f4911-c93c-471a-9ded-9fe7475bcb78");
            GenericResult<MovieDomainModel> genericResult = new GenericResult<MovieDomainModel> {IsSuccessful=true};
            _mockMovieService.Setup(srvc => srvc.ActivateDeactivateMovie(movieId)).ReturnsAsync(genericResult);

            //Act
            var result = await _moviesController.ActivateMovie(movieId);
            var message = (AcceptedResult)result.Result;
        

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, message.StatusCode);

        }

        [TestMethod]
        public void CreateMovieAsync_When_Called_IsSuccesful_True_Calls_AddTagsForMovie()
        {
            var movieToCreateTags = new GenericResult<MovieDomainModel>
            {
                IsSuccessful= true,
                Data = new MovieDomainModel()
            };
            var movieToCreate = new MovieDomainModel();
            var movieDomainModel = new MovieDomainModel { };

            var movieCreateModel = new CreateMovieModel
            {
                 Title="new Movie",
                  Current= true,
                   Genre= "comedy",
                    Rating= 3,
                     HasOscar= true,
                      Year=1999,
                       UserRaitings= 2
            };
            _mockMovieService.Setup(srvc => srvc.AddMovieAsync(It.IsNotNull<MovieDomainModel>())).ReturnsAsync(movieToCreateTags);
            _mockMovieService.Setup(srvc => srvc.AddTagsForMovie(It.IsNotNull<MovieDomainModel>()));
            
            //Act
            var result = _moviesController.CreateMovieAsync(movieCreateModel);
            
            //Assert
           


        }
        [TestMethod]
        public async Task SearchMoviesByTags_When_isSuccesful_False_Returns_BadRequest()
        {
            var expectedMessage = Messages.MOVIE_SEARCH_BY_TAG_NOT_FOUND;

            var ResultModel = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_SEARCH_BY_TAG_NOT_FOUND
            };

            
            _mockMovieService.Setup(srvc => srvc.SearchMoviesByTag(It.IsAny<string>())).ReturnsAsync(ResultModel);

            var result = await _moviesController.SearchMoviesByTags(It.IsAny<string>());

            var movieResultMessage = ((BadRequestObjectResult)result.Result).Value;



            //Assert
            Assert.IsInstanceOfType(movieResultMessage, typeof(ErrorResponseModel));
            var message = (ErrorResponseModel)movieResultMessage;
            Assert.AreEqual(expectedMessage, message.ErrorMessage);

        }

        [TestMethod]
        public async Task GetByCinemaIdAsync_When_IsSuccessful_False_Returns_NotFound()
        {
            //Arrange
            int expectedStatusCode = 404;
            var expectedMovieResult = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
            };

            _mockMovieService.Setup(src => src.GetMoviesByCinemaId(It.IsAny<int>())).ReturnsAsync(expectedMovieResult);


            //Act

            var result = await _moviesController.GetByCinemaIdAsync(It.IsAny<int>());

            //Assert
            var movieResult = ((NotFoundObjectResult)result.Result).Value;
            var error = (ErrorResponseModel)movieResult;

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result.Result).StatusCode);

        }

        [TestMethod]
        public async Task GetByCinemaIdAsync_When_IsSuccessful_True_Returns_Ok()
        {
            //Arrange
            int expectedStatusCode = 200;

            var expectedMovieResult = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList=new List<MovieDomainModel> { }
            };

            _mockMovieService.Setup(src => src.GetMoviesByCinemaId(It.IsNotNull<int>())).ReturnsAsync(expectedMovieResult);


            //Act

            var result = await _moviesController.GetByCinemaIdAsync(It.IsNotNull<int>());

            //Assert
            var movieResult = (OkObjectResult)result.Result;

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, movieResult.StatusCode);

        }

        [TestMethod]
        public async Task GetByAuditoriumIdAsync_When_IsSuccessful_True_Returns_Ok()
        {
            //Arrange
            int expectedStatusCode = 200;
            var expectedMovieResult = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<MovieDomainModel> { }
            };

            _mockMovieService.Setup(src => src.GetMoviesByAuditoriumId(It.IsNotNull<int>())).ReturnsAsync(expectedMovieResult);


            //Act

            var result = await _moviesController.GetByAuditoriumIdAsync(It.IsNotNull<int>());

            //Assert
            var movieResult = (OkObjectResult)result.Result;

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, movieResult.StatusCode);

        }

        [TestMethod]
        public async Task GetByAuditoriumIdAsync_When_IsSuccessful_False_Returns_NotFound()
        {
            //Arrange
            int expectedStatusCode = 404;
            var expectedMovieResult = new GenericResult<MovieDomainModel>
            {
                IsSuccessful = false,
            };

            _mockMovieService.Setup(src => src.GetMoviesByAuditoriumId(It.IsAny<int>())).ReturnsAsync(expectedMovieResult);


            //Act

            var result = await _moviesController.GetByAuditoriumIdAsync(It.IsAny<int>());

            //Assert
            var movieResult = ((NotFoundObjectResult)result.Result).Value;
            var error = (ErrorResponseModel)movieResult;

            Assert.IsNotNull(movieResult);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result.Result).StatusCode);

        }
    }
}

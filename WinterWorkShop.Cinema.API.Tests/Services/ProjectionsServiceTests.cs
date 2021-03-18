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
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;


//Komentar Nikola test
//Komentar 2

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class ProjectionsServiceTests
    {
        private Mock<IProjectionsRepository> _mockProjectionsRepository;
        private Mock<ICinemasRepository> _mockCinemasRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumsRepository;
        private Mock<IMoviesRepository> _mockMoviesRepository;
        private Projection _projection;
        private Data.Cinema _cinema;
        private Auditorium _auditorium;
        private Movie _movie;
        private ProjectionDomainModel _projectionDomainModel;
        ProjectionService _projectionsService;
        Task<IEnumerable<Projection>> _responseTask;
        private DateTime _date;
        private Guid _movieId;
        private int _audirotiumId;
        private int _cinemaId;

       [TestInitialize]
        public void TestInitialize()
        {
            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockCinemasRepository = new Mock<ICinemasRepository>();
            _mockAuditoriumsRepository = new Mock<IAuditoriumsRepository>();
            _mockMoviesRepository = new Mock<IMoviesRepository>();
            _projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockCinemasRepository.Object,_mockAuditoriumsRepository.Object,_mockMoviesRepository.Object);
            _projection = new Projection
            {
                Id = Guid.NewGuid(),
                Auditorium = new Auditorium { AuditoriumName = "ImeSale" },
                Movie = new Movie { Title = "ImeFilma" },
                MovieId = Guid.NewGuid(),
                ShowingDate = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };

            _projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };

            List<Projection> projectionsModelsList = new List<Projection>();

            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            _responseTask = Task.FromResult(projections);

            _date = DateTime.Now.AddDays(1);
            _movieId = Guid.NewGuid();
            _audirotiumId = 1;
            _cinemaId = 1;

            _cinema = new Data.Cinema
            {
                Id = _cinemaId,
                Address = "adresa",
                Name = "Cinema",
                CityName = "ulica"
            };

            _auditorium = new Auditorium
            {
                AuditoriumName = "First Auditorium",
                Id = _audirotiumId,
                CinemaId = _cinemaId,
                Seats = new List<Seat>
                    {
                        new Seat
                        {
                            Id = new Guid("d76dcdc8-2632-4612-a5b3-a0ca8ef24459"),
                            AuditoriumId = 1,
                            Number = 3,
                            Row = 1
                        },
                    }
            };

            _movie = new Movie
            {
                Current = false,
                Genre = "comedy",
                Id = _movieId,
                Rating = 8,
                Title = "New_Movie1",
                Year = 1999,
                UserRaitings = 9,
                HasOscar=true,
                CoverPicture="/slika"
            };
        }

        [TestMethod]
        public void ProjectionService_GetAllAsync_ReturnListOfProjecrions()
        {
            //Arrange
            int expectedResultCount = 1;

            _mockProjectionsRepository.Setup(x => x.GetAllAsync()).Returns(_responseTask);
            //Act
            var resultAction = _projectionsService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<ProjectionDomainModel>)resultAction;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.AreEqual(_projection.Id, result[0].Id);
            Assert.IsInstanceOfType(result[0], typeof(ProjectionDomainModel));
        }

        [TestMethod]
        public void ProjectionService_GetAllAsync_ReturnNull()
        {
            //Arrange
            IEnumerable<Projection> projections = null;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            
            _mockProjectionsRepository.Setup(x => x.GetAllAsync()).Returns(responseTask);
            

            //Act
            var resultAction = _projectionsService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        // _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId) mocked to return list with projections
        // if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0) - true
        // return ErrorMessage
        [TestMethod]
        public void ProjectionService_CreateProjection_WithProjectionAtSameTime_ReturnErrorMessage() 
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            string expectedMessage = "Cannot create new projection, there are projections at same time alredy.";

            
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);
            

            //Act
            var resultAction = _projectionsService.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedMessage, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
        }

        // _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId) mocked to return empty list
        // if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0) - false
        // _projectionsRepository.Insert(newProjection) mocked to return null
        //  if (insertedProjection == null) - true
        // return CreateProjectionResultModel  with errorMessage
        [TestMethod]
        public void ProjectionService_CreateProjection_InsertMockedNull_ReturnErrorMessage()
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();
            _projection = null;
            string expectedMessage = "Error occured while creating new projection, please try again.";

           
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);
            _mockProjectionsRepository.Setup(x => x.InsertAsync(It.IsAny<Projection>())).ReturnsAsync(_projection);
            

            //Act
            var resultAction = _projectionsService.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedMessage, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
        }

        // _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId) mocked to return empty list
        // if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0) - false
        // _projectionsRepository.Insert(newProjection) mocked to return valid EntityEntry<Projection>
        //  if (insertedProjection == null) - false
        // return valid projection 
        [TestMethod]
        public void ProjectionService_CreateProjection_InsertMocked_ReturnProjection()
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();
            



            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);
            _mockProjectionsRepository.Setup(x => x.InsertAsync(It.IsAny<Projection>())).ReturnsAsync(_projection);
            _mockProjectionsRepository.Setup(x => x.Save());
        

            //Act
            var resultAction = _projectionsService.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_projection.Id, resultAction.Data.Id);
            Assert.IsNull(resultAction.ErrorMessage);
            Assert.IsTrue(resultAction.IsSuccessful);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void Projectionervice_CreateProjection_When_Updating_Non_Existing_Movie()
        {
            // Arrange
            List<Projection> projectionsModelsList = new List<Projection>();

            
            _mockProjectionsRepository.Setup(x => x.InsertAsync(It.IsAny<Projection>())).Throws(new DbUpdateException());
            _mockProjectionsRepository.Setup(x => x.Save());
           

            //Act
            var resultAction = _projectionsService.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public async Task FilterProjectionAsync_All_Parameters_Are_Valid_Return_GenericResult_DaraList()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                AuditoriumId = _audirotiumId,
                CinemaId = _cinemaId,
                DateTime = _date,
                MovieId = _movieId
            };
            var expectedResultCount = 1;
            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<ProjectionDomainModel>
                {
                    new ProjectionDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = _audirotiumId,
                        MovieId = _movieId,
                        ProjectionTime = _date,
                        Duration = 100,
                        Price = 300,
                        AuditoriumName = _auditorium.AuditoriumName,
                        MovieTitle =_movie.Title
                    }
                }
            };
            IEnumerable<Projection> responsProjection = new List<Projection> { new Projection
            {
                MovieId= (Guid)filter.MovieId,
                AuditoriumId = (int)filter.AuditoriumId,
                Movie = _movie,
                Auditorium = _auditorium,
                ShowingDate = _date,
                Duration = 100,
                Id = Guid.NewGuid(),
                Price = 300

            } };
            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriumResponse = auditoriumList;

            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            IEnumerable<Movie> movieResponse = movieList;

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_auditorium);
            _mockAuditoriumsRepository.Setup(repo => repo.GetAllByCinemaIdAsync(It.IsAny<int>())).ReturnsAsync(auditoriumResponse);
            _mockMoviesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
            _mockMoviesRepository.Setup(repo => repo.GetMoviesByAuditoriumId(It.IsAny<int>())).ReturnsAsync(movieResponse);
            _mockProjectionsRepository.Setup(repo => repo.FilterProjectionAsync(filter.CinemaId, filter.AuditoriumId, filter.MovieId, filter.DateTime)).ReturnsAsync(responsProjection);

            //Act
            var result =await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DataList[0].AuditoriumId, filter.AuditoriumId);
            Assert.AreEqual(result.DataList[0].MovieId, filter.MovieId);
            Assert.AreEqual(expectedResultCount, result.DataList.Count);
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task FilterProjectionAsync_CinemaId_Does_Not_Exists_In_Database_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                AuditoriumId = _audirotiumId,
                CinemaId = _cinemaId,
                DateTime = _date,
                MovieId = _movieId
            };

            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful=false,
                ErrorMessage = Messages.CINEMA_ID_NOT_FOUND
            };
            Data.Cinema cinema = null;

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(cinema);
            
            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.ErrorMessage, expectedResult.ErrorMessage);

        }

        [TestMethod]
        public async Task FilterProjectionAsync_AuditoriumId_Does_Not_Exists_In_Database_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                CinemaId = _cinemaId,
                AuditoriumId = _audirotiumId,
                DateTime = _date,
                MovieId = _movieId
            };

            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR
            };
            Auditorium auditorium = null;

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(auditorium);
            

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.ErrorMessage, expectedResult.ErrorMessage);

        }

        [TestMethod]
        public async Task FilterProjectionAsync_AuditoriumId_Is_Not_In_Cinema_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                CinemaId = _cinemaId,
                AuditoriumId = _audirotiumId,
                DateTime = _date,
                MovieId = _movieId
            };

            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_NOT_IN_CINEMA
            };
            IEnumerable<Auditorium> auditoriumResponse = new List<Auditorium>();

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_auditorium);
            _mockAuditoriumsRepository.Setup(repo => repo.GetAllByCinemaIdAsync(It.IsAny<int>())).ReturnsAsync(auditoriumResponse);

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.ErrorMessage, expectedResult.ErrorMessage);

        }

        [TestMethod]
        public async Task FilterProjectionAsync_MovieId_Does_Not_Exists_In_Database_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                CinemaId = _cinemaId,
                AuditoriumId = _audirotiumId,
                DateTime = _date,
                MovieId = _movieId
            };
            
            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_GET_BY_ID
            };
            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriumResponse = auditoriumList;

            Movie movie = null;

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_auditorium);
            _mockAuditoriumsRepository.Setup(repo => repo.GetAllByCinemaIdAsync(It.IsAny<int>())).ReturnsAsync(auditoriumResponse);
            _mockMoviesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(movie);

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.ErrorMessage, expectedResult.ErrorMessage);

        }

        [TestMethod]
        public async Task FilterProjectionAsync_MovieId_Is_Not_Projecting_In_Auditorium_Return_GenericResult_ErrorMessage()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                CinemaId = _cinemaId,
                AuditoriumId = _audirotiumId,
                DateTime = _date,
                MovieId = _movieId
            };
            
            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_NOT_IN_AUDITORIUM
            };
            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriumResponse = auditoriumList;

            IEnumerable<Movie> movieResponse = new List<Movie>();

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_auditorium);
            _mockAuditoriumsRepository.Setup(repo => repo.GetAllByCinemaIdAsync(It.IsAny<int>())).ReturnsAsync(auditoriumResponse);
            _mockMoviesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
            _mockMoviesRepository.Setup(repo => repo.GetMoviesByAuditoriumId(It.IsAny<int>())).ReturnsAsync(movieResponse);
            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.ErrorMessage, expectedResult.ErrorMessage);

        }

        [TestMethod]
        public async Task FilterProjectionAsync_Date_In_Past_Resturn__GenericResult_ErrorMessage()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                AuditoriumId = _audirotiumId,
                CinemaId = _cinemaId,
                DateTime = DateTime.Now.AddDays(-1),
                MovieId = _movieId
            };
            
            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage=Messages.PROJECTION_IN_PAST
            };
            IEnumerable<Projection> responsProjection = new List<Projection> { new Projection
            {
                MovieId= (Guid)filter.MovieId,
                AuditoriumId = (int)filter.AuditoriumId,
                Movie = _movie,
                Auditorium = _auditorium,
                ShowingDate = _date,
                Duration = 100,
                Id = Guid.NewGuid(),
                Price = 300

            } };
            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriumResponse = auditoriumList;

            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            IEnumerable<Movie> movieResponse = movieList;

            _mockCinemasRepository.Setup(repo => repo.GetByIdAsync(It.IsNotNull<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_auditorium);
            _mockAuditoriumsRepository.Setup(repo => repo.GetAllByCinemaIdAsync(It.IsAny<int>())).ReturnsAsync(auditoriumResponse);
            _mockMoviesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
            _mockMoviesRepository.Setup(repo => repo.GetMoviesByAuditoriumId(It.IsAny<int>())).ReturnsAsync(movieResponse);
            _mockProjectionsRepository.Setup(repo => repo.FilterProjectionAsync(filter.CinemaId, filter.AuditoriumId, filter.MovieId, filter.DateTime)).ReturnsAsync(responsProjection);

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.ErrorMessage, expectedResult.ErrorMessage);
        }

        [TestMethod]
        public async Task FilterProjectionAsync_Filter_WithOut_CinemaId_Are_Valid_Return_GenericResult_DaraList()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            {
                AuditoriumId = _audirotiumId,
                
                DateTime = _date,
                MovieId = _movieId
            };
            int expectedResultCount = 1;
            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<ProjectionDomainModel>
                {
                    new ProjectionDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = _audirotiumId,
                        MovieId = _movieId,
                        ProjectionTime = _date,
                        Duration = 100,
                        Price = 300,
                        AuditoriumName = _auditorium.AuditoriumName,
                        MovieTitle =_movie.Title
                    }
                }
            };
            IEnumerable<Projection> responsProjection = new List<Projection> { new Projection
            {
                MovieId= (Guid)filter.MovieId,
                AuditoriumId = (int)filter.AuditoriumId,
                Movie = _movie,
                Auditorium = _auditorium,
                ShowingDate = _date,
                Duration = 100,
                Id = Guid.NewGuid(),
                Price = 300

            } };
            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriumResponse = auditoriumList;

            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            IEnumerable<Movie> movieResponse = movieList;

            _mockAuditoriumsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_auditorium);
            _mockAuditoriumsRepository.Setup(repo => repo.GetAllByCinemaIdAsync(It.IsAny<int>())).ReturnsAsync(auditoriumResponse);
            _mockMoviesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
            _mockMoviesRepository.Setup(repo => repo.GetMoviesByAuditoriumId(It.IsAny<int>())).ReturnsAsync(movieResponse);
            _mockProjectionsRepository.Setup(repo => repo.FilterProjectionAsync(filter.CinemaId, filter.AuditoriumId, filter.MovieId, filter.DateTime)).ReturnsAsync(responsProjection);

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DataList[0].AuditoriumId, filter.AuditoriumId);
            Assert.AreEqual(result.DataList[0].MovieId, filter.MovieId);
            Assert.AreEqual(expectedResultCount, result.DataList.Count);
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task FilterProjectionAsync_Filter_WithOut_CinemaId_and_AuditoriumId_Are_Valid_Return_GenericResult_DaraList()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel
            { 
                DateTime = _date,
                MovieId = _movieId
            };
            int expectedResultCount = 1;
            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<ProjectionDomainModel>
                {
                    new ProjectionDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = _audirotiumId,
                        MovieId = _movieId,
                        ProjectionTime = _date,
                        Duration = 100,
                        Price = 300,
                        AuditoriumName = _auditorium.AuditoriumName,
                        MovieTitle =_movie.Title
                    }
                }
            };
            IEnumerable<Projection> responsProjection = new List<Projection> { new Projection
            {
                MovieId= _movieId,
                AuditoriumId = _audirotiumId,
                Movie = _movie,
                Auditorium = _auditorium,
                ShowingDate = _date,
                Duration = 100,
                Id = Guid.NewGuid(),
                Price = 300

            } };
           

            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            IEnumerable<Movie> movieResponse = movieList;

           
            _mockMoviesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
            _mockMoviesRepository.Setup(repo => repo.GetMoviesByAuditoriumId(It.IsAny<int>())).ReturnsAsync(movieResponse);
            _mockProjectionsRepository.Setup(repo => repo.FilterProjectionAsync(filter.CinemaId, filter.AuditoriumId, filter.MovieId, filter.DateTime)).ReturnsAsync(responsProjection);

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DataList[0].ProjectionTime, filter.DateTime);
            Assert.AreEqual(result.DataList[0].MovieId, filter.MovieId);
            Assert.AreEqual(expectedResultCount, result.DataList.Count);
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task FilterProjectionAsync_Filter_None_Parametars_Are_Valid_Return_GenericResult_DaraList()
        {
            //Arrange
            var filter = new FilterProjectionDomainModel();
            int expectedResultCount = 1;

            var expectedResult = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<ProjectionDomainModel>
                {
                    new ProjectionDomainModel
                    {
                        Id = Guid.NewGuid(),
                        AuditoriumId = _audirotiumId,
                        MovieId = _movieId,
                        ProjectionTime = _date,
                        Duration = 100,
                        Price = 300,
                        AuditoriumName = _auditorium.AuditoriumName,
                        MovieTitle =_movie.Title
                    }
                }
            };

            IEnumerable<Projection> responsProjection = new List<Projection> { new Projection
            {
                MovieId= _movieId,
                AuditoriumId = _audirotiumId,
                Movie = _movie,
                Auditorium = _auditorium,
                ShowingDate = _date,
                Duration = 100,
                Id = Guid.NewGuid(),
                Price = 300

            } };


            _mockProjectionsRepository.Setup(repo => repo.FilterProjectionAsync(filter.CinemaId, filter.AuditoriumId, filter.MovieId, filter.DateTime)).ReturnsAsync(responsProjection);

            //Act
            var result = await _projectionsService.FilterProjectionAsync(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DataList[0].ProjectionTime, _date);
            Assert.AreEqual(result.DataList[0].MovieId, _movieId);
            Assert.AreEqual(result.DataList[0].AuditoriumId, _audirotiumId);
            Assert.AreEqual(expectedResultCount, result.DataList.Count);
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}

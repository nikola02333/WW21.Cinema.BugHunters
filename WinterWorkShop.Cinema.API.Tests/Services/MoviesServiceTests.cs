using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class MoviesServiceTests
    {
        private Mock<IMoviesRepository> _mockMovieRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private MovieService _moviesService;

           
        [TestInitialize]
        public void TestInit()
        {
            _mockMovieRepository = new Mock<IMoviesRepository>();
            _moviesService = new MovieService(_mockMovieRepository.Object, _mockAuditoriumRepository.Object);
        }

        [TestMethod]
        public async Task GetAllMoviesAsync_When_IsCurent_False_Retusns_AllMovies()
        {
            var isCurrent = false;
            var expectedMovies = new  List<Movie>
            {
                new Movie{ Current= false, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie1", Year=1999},
                new Movie{ Current= false, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie2", Year=1999},
            };

            _mockMovieRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedMovies);

            // Act

            var result = await _moviesService.GetAllMoviesAsync(isCurrent);

            //Arrange
            Assert.IsInstanceOfType(result.DataList, typeof(List<MovieDomainModel>));
            Assert.AreEqual(result.DataList.Count, expectedMovies.Count);
            _mockMovieRepository.Verify(srvc => srvc.GetAllAsync(), Times.Once);
            Assert.AreEqual(isCurrent, result.DataList[0].Current);

        }

        [TestMethod]
        public async Task GetAllMoviesAsync_When_IsCurent_True_Retusns_AllMovies()
        {
            var isCurrent = true;
            var expectedMovies = new List<Movie>
            {
                new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie1", Year=1999},
                new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie2", Year=1999},
            };

            _mockMovieRepository.Setup(srvc => srvc.GetCurrentMoviesAsync()).ReturnsAsync(expectedMovies);

            // Act

            var result = await _moviesService.GetAllMoviesAsync(isCurrent);

            //Arrange
            Assert.IsInstanceOfType(result.DataList, typeof(List<MovieDomainModel>));
            Assert.AreEqual(result.DataList.Count, expectedMovies.Count);
            _mockMovieRepository.Verify(srvc => srvc.GetCurrentMoviesAsync(), Times.Once);
            Assert.AreEqual(isCurrent, result.DataList[0].Current);

        }


        [TestMethod]
        public async Task GetAllMoviesAsync_When_IsCurrent_False_Retusns_EmptyList()
        {
            var isCurrent = false;
            var expectedMovies = new List<Movie>
            {
            };

            _mockMovieRepository.Setup(srvc => srvc.GetAllAsync()).ReturnsAsync(expectedMovies);

            // Act

            var result = await _moviesService.GetAllMoviesAsync(isCurrent);

            //Arrange
            Assert.IsInstanceOfType(result.DataList, typeof(List<MovieDomainModel>));
            Assert.AreEqual(result.DataList.Count, expectedMovies.Count);
            _mockMovieRepository.Verify(srvc => srvc.GetAllAsync(), Times.Once);

        }

        [TestMethod]
        public async Task GetMovieById_When_Id_NotFound_Retuns_NotFound()
        {
            var movieId = Guid.NewGuid();
            var expectedMessage = Messages.MOVIE_DOES_NOT_EXIST;
            var isSuccesful = false;
            

            _mockMovieRepository.Setup(srvc => srvc.GetByIdAsync(movieId)).ReturnsAsync(It.IsAny<Movie>);

            // Act

            var result = await _moviesService.GetMovieByIdAsync(movieId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(result.ErrorMessage, expectedMessage);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);

        }
        [TestMethod]
        public async Task GetMovieById_When_Called_Returns_Movie()
        {
            var movieId = Guid.NewGuid();
            var isSuccesful = true;

            var movieToReturn =new Movie
            {
                Id= movieId,
                Current= true,
                Title= "Movie name"
            };

            _mockMovieRepository.Setup(srvc => srvc.GetByIdAsync(movieId)).ReturnsAsync(movieToReturn);

            // Act

            var result = await _moviesService.GetMovieByIdAsync(movieId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(result.Data.Id, movieId);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }

        [TestMethod]
        public async Task AddMovieAsync_Returns_Movie()
        {
            var movieId = Guid.NewGuid();
            var isSuccesful = true;

            var movieToCreate = new MovieDomainModel
            {
                Title="New Movie",
                Current= true
            };

            var movieToReturn = new Movie
            {
                Id = movieId,
                Current = movieToCreate.Current,
                Title = movieToCreate.Title
            };
            

            _mockMovieRepository.Setup(srvc => srvc.InsertAsync(It.IsNotNull<Movie>())).ReturnsAsync(movieToReturn);
            // Act

            var result = await _moviesService.AddMovieAsync(movieToCreate);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(result.Data.Id, movieId);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }

        [TestMethod]
        public  void UpdateMovie_Returns_Updated_Movie()
        {
            var movieId = Guid.NewGuid();
            var isSuccesful = true;

            var movieToUpdate = new MovieDomainModel
            {
                Title = "Update Movie",
                Current = true
            };

            var movieToReturn = new Movie
            {
                Id = movieId,
                Current = movieToUpdate.Current,
                Title = movieToUpdate.Title
            };


            _mockMovieRepository.Setup(srvc => srvc.Update(It.IsNotNull<Movie>())).Returns(movieToReturn);
            // Act

            var result =  _moviesService.UpdateMovie(movieToUpdate);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(result.Data.Id, movieId);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }

        [TestMethod]
        public async  Task DeleteMovieAsync_When_Id_NotFound_Returns_Movie_NotFound()
        {
            var movieId = Guid.NewGuid();
            var isSuccesful = false;
            var expectedMessage = Messages.MOVIE_DOES_NOT_EXIST;

            

           

            _mockMovieRepository.Setup(srvc => srvc.Delete(movieId)).Returns(It.IsNotNull<Movie>());
            // Act

            var result =await _moviesService.DeleteMovieAsync(movieId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
            Assert.AreEqual(expectedMessage, result.ErrorMessage);
        }

        [TestMethod]
        public async Task DeleteMovieAsync_When_Called_Returns_IsSuccesful_True()
        {
            var movieId = Guid.NewGuid();
            var isSuccesful = true;
            var movieToDelete = new Movie();

            _mockMovieRepository.Setup(srvc => srvc.GetByIdAsync(movieId)).ReturnsAsync(movieToDelete);
            _mockMovieRepository.Setup(srvc => srvc.Delete(movieId)).Returns(movieToDelete);
            // Act

            var result = await _moviesService.DeleteMovieAsync(movieId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }

        [TestMethod]
        public async Task GetTopTenMoviesAsync_Returns_EmptyList()
        {
            var expectedMovies = new List<Movie>();

            _mockMovieRepository.Setup(srvc => srvc.GetTopTenMovies()).ReturnsAsync(expectedMovies);

            // Act

            var result = await _moviesService.GetTopTenMoviesAsync();

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(expectedMovies.Count, result.DataList.Count);
        }

        [TestMethod]
        public async Task GetTopTenMoviesAsync_Returns_TopTenMovies()
        {
            var expectedMovies = new List<Movie>
            {
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie1", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie2", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie3", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie4", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie5", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie6", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 10, Title="New_Movie7", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 9, Title="New_Movie8", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie9", Year=1999},
             new Movie{ Current= true, Genre = "comedy", Id = Guid.NewGuid(), Rating= 8, Title="New_Movie10", Year=1999}
            };

            _mockMovieRepository.Setup(srvc => srvc.GetTopTenMovies()).ReturnsAsync(expectedMovies);

            // Act

            var result = await _moviesService.GetTopTenMoviesAsync();

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(expectedMovies.Count, result.DataList.Count);
        }

        [TestMethod]
        public async Task ActivateMovieAsync_When_Movie_Has_Upcoming_Projection_Retuns_MOVIE_ACTIVATE_DEACTIVATE_ERROR()
        {
            var expectedMessage = Messages.MOVIE_ACTIVATE_DEACTIVATE_ERROR;
            var isSuccesful = false;
            var movieToDeactivateId = Guid.NewGuid();

          

            var moveToDeactivate = new Movie
            {
                Id = movieToDeactivateId,
                Current = true,
                Projections = new List<Projection>
                {
                    new Projection
                    {
                        ShowingDate = DateTime.Now.AddDays(10)
                    }
                }
            };
            _mockMovieRepository.Setup(srvc => srvc.GetByIdAsync(movieToDeactivateId)).ReturnsAsync(moveToDeactivate);

            // Act

            var result = await _moviesService.ActivateDeactivateMovie(movieToDeactivateId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(expectedMessage, result.ErrorMessage);
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }

        [TestMethod]
        public async Task ActivateMovieAsync_When_Movie_Has_NO_Upcoming_Projection_Retuns_IsSuccesful_True()
        {
            var isSuccesful = true;
            var movieToDeactivateId = Guid.NewGuid();


            var moveToDeactivate = new Movie
            {
                Id = movieToDeactivateId,
                Current = true,
                Projections = new List<Projection>
                {
                    new Projection
                    {
                      
                    }
                }
            };
            _mockMovieRepository.Setup(setup => setup.ActivateDeactivateMovie(It.IsNotNull<Movie>())).ReturnsAsync(moveToDeactivate);
            _mockMovieRepository.Setup(srvc => srvc.GetByIdAsync(movieToDeactivateId)).ReturnsAsync(moveToDeactivate);

            // Act

            var result = await _moviesService.ActivateDeactivateMovie(movieToDeactivateId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }

        [TestMethod]
        public async Task ActivateMovieAsync_When_MovieID_NotFound_Retuns_MovieNotExiests()
        {
            var isSuccesful = false;
            var movieToDeactivateId = Guid.NewGuid();


            var moveToDeactivate = new Movie
            {
                Id = movieToDeactivateId,
                Current = true,
                Projections = new List<Projection>
                {
                    new Projection
                    {

                    }
                }
            };
            _mockMovieRepository.Setup(setup => setup.ActivateDeactivateMovie(It.IsNotNull<Movie>())).ReturnsAsync(moveToDeactivate);
            _mockMovieRepository.Setup(srvc => srvc.GetByIdAsync(movieToDeactivateId)).ReturnsAsync(default(Movie));

            // Act

            var result = await _moviesService.ActivateDeactivateMovie(movieToDeactivateId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(GenericResult<MovieDomainModel>));
            Assert.AreEqual(isSuccesful, result.IsSuccessful);
        }
    }
}

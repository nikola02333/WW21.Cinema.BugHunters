using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public MovieService(IMoviesRepository moviesRepository, IAuditoriumsRepository auditoriumsRepository)
        {
            _moviesRepository = moviesRepository;
            _auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<GenericResult<MovieDomainModel>> GetAllMoviesAsync(bool? isCurrent)
        {
            var allMovies = isCurrent != null && isCurrent == true
                ? await _moviesRepository.GetCurrentMoviesAsync()
                : await _moviesRepository.GetAllAsync();
                            
            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (var item in allMovies)
            {
                model = new MovieDomainModel
                {
                    Current = item.Current,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title,
                    Year = item.Year,
                    Genre = item.Genre,
                    CoverPicture = item.CoverPicture,
                    HasOscar = item.HasOscar,
                    UserRaitings = item.UserRaitings ?? 0

                };
                result.Add(model);
            }

            return new GenericResult<MovieDomainModel>
            { 
                IsSuccessful= true,
                 DataList= result
            };

        }

        public async Task<GenericResult<MovieDomainModel>> GetMovieByIdAsync(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);

           if(movie == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = movie.Id,
                Current = movie.Current,
                Rating = movie.Rating ?? 0,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                CoverPicture = movie.CoverPicture,
                HasOscar = movie.HasOscar,
                UserRaitings = movie.UserRaitings ?? 0
            };

            return  new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data = domainModel
            }; 
        }
        //Fale testovi
        public async Task<GenericResult<MovieDomainModel>> GetMoviesByAuditoriumId(int id)
        {
            var auditorium = await _auditoriumsRepository.GetByIdAsync(id);
            if(auditorium == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR
                };
            }

            var movies = await _moviesRepository.GetMoviesByAuditoriumId(id);

            if (movies == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = true
                };
            }

            return new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList = movies.Select(item => new MovieDomainModel
                {
                    Current = item.Current,
                    Genre = item.Genre,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title,
                    Year = item.Year,
                    CoverPicture = item.CoverPicture,
                    HasOscar = item.HasOscar,
                    UserRaitings = item.UserRaitings ?? 0
                }).ToList()
            };

        }


        public async Task<GenericResult<MovieDomainModel>> AddMovieAsync(MovieDomainModel newMovie)
        {
            Movie movieToCreate = new Movie()
            {
                Title = newMovie.Title,
                Current = newMovie.Current,
                Year = newMovie.Year,
                Rating = newMovie.Rating,
                Genre= newMovie.Genre,
                CoverPicture = newMovie.CoverPicture,
                HasOscar = newMovie.HasOscar,
                UserRaitings = newMovie.UserRaitings
            };

            var movie = await _moviesRepository.InsertAsync(movieToCreate);

          

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Current = movie.Current,
                Year = movie.Year,
                Genre= movie.Genre,
                Rating = movie.Rating ?? 0,
                CoverPicture = movie.CoverPicture,
                HasOscar = movie.HasOscar,
                UserRaitings = movie.UserRaitings ?? 0
            };

            return new GenericResult<MovieDomainModel> 
            {
                IsSuccessful= true,
                Data = domainModel
            };
        }

        public  GenericResult<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie) {

            Movie movieToUpdate = new Movie()
            {
                Id = updateMovie.Id,
                Title = updateMovie.Title,
                Current = updateMovie.Current,
                Year = updateMovie.Year,
                Rating = updateMovie.Rating,
                Genre= updateMovie.Genre,
                CoverPicture = updateMovie.CoverPicture,
                HasOscar = updateMovie.HasOscar,
                UserRaitings = updateMovie.UserRaitings 
            };
            
            var movieUpdated = _moviesRepository.Update(movieToUpdate);

          
            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = movieUpdated.Id,
                Title = movieUpdated.Title,
                Current = movieUpdated.Current,
                Year = movieUpdated.Year,
                Genre= movieToUpdate.Genre,
                Rating = movieUpdated.Rating ?? 0,
                CoverPicture = movieUpdated.CoverPicture,
                HasOscar = movieUpdated.HasOscar,
                UserRaitings = movieUpdated.UserRaitings ?? 0
            };

            return new GenericResult<MovieDomainModel>
            {
                IsSuccessful=true,
                Data = domainModel
            };
        }

        public async Task<GenericResult<MovieDomainModel>> DeleteMovieAsync(Guid id)
        {
            var movieToDelete =await _moviesRepository.GetByIdAsync(id);
           
            if(movieToDelete == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }
           var moveDeleted =  _moviesRepository.Delete(id);

            if(moveDeleted ==null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage= Messages.MOVIE_DELETE_ERROR
                };
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = movieToDelete.Id,
                Title = movieToDelete.Title,
                Current = movieToDelete.Current,
                Year = movieToDelete.Year,
                Rating = movieToDelete.Rating ?? 0,
                Genre = movieToDelete.Genre,
                CoverPicture = movieToDelete.CoverPicture,
                HasOscar = movieToDelete.HasOscar,
                UserRaitings = movieToDelete.UserRaitings ?? 0

            };

            return new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data =domainModel
            };
        }

        public async Task<GenericResult<MovieDomainModel>> GetTopTenMoviesAsync()
        {
            var topTenMovies =await _moviesRepository.GetTopTenMovies();

            var movies = topTenMovies.Select(movie => new MovieDomainModel
            {
                 Current= movie.Current,
                 Genre= movie.Genre,
                 Id= movie.Id,
                 Rating= movie.Rating?? 0,
                 Title= movie.Title,
                 Year= movie.Year,
                 CoverPicture = movie.CoverPicture,
                 HasOscar = movie.HasOscar,
                 UserRaitings = movie.UserRaitings ?? 0
            }).ToList();
            return new GenericResult<MovieDomainModel> 
            { 
            DataList= movies
            };
        }

        public async Task<GenericResult<MovieDomainModel>> ActivateDeactivateMovie(Guid movieId)
        {
            var movieToUpdate =await _moviesRepository.GetByIdAsync(movieId);

            if(movieToUpdate == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            var dataTimeNow = DateTime.Now;

            var upcomingProjections = movieToUpdate.Projections.Where(project => project.ShowingDate > dataTimeNow)
                .ToList();

            if(upcomingProjections.Count > 0)
            {
               return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_ACTIVATE_DEACTIVATE_ERROR
                };
            }

           var updatedMovie =  await _moviesRepository.ActivateDeactivateMovie(movieToUpdate);

            return new GenericResult<MovieDomainModel>
            {
            
            IsSuccessful= true,
            Data=  new MovieDomainModel
            {
                 Current= updatedMovie.Current,
                 Genre= updatedMovie.Genre,
                 Id= updatedMovie.Id,
                 Rating= updatedMovie.Rating ?? 0,
                 Title= updatedMovie.Title,
                 Year= updatedMovie.Year,
                CoverPicture = updatedMovie.CoverPicture,
                HasOscar = updatedMovie.HasOscar,
                UserRaitings = updatedMovie.UserRaitings ?? 0
            }
            };

        }
    }
}

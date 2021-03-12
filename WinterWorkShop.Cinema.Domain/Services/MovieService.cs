using System;
using System.Collections.Generic;
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

        public MovieService(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public async Task<IEnumerable<MovieDomainModel>> GetAllMoviesAsync(bool? isCurrent)
        {
            var data = await _moviesRepository.GetCurrentMoviesAsync();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (var item in data)
            {
                model = new MovieDomainModel
                {
                    Current = item.Current,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title,
                    Year = item.Year
                };
                result.Add(model);
            }

            return result;

        }

        public async Task<GenericResult<MovieDomainModel>> GetMovieByIdAsync(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful=false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = movie.Id,
                Current = movie.Current,
                Rating = movie.Rating ?? 0,
                Title = movie.Title,
                Year = movie.Year
            };

            return  new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data = domainModel
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
                Genre= newMovie.Genre
            };

            var movie = await _moviesRepository.InsertAsync(movieToCreate);

            if (movie == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Current = movie.Current,
                Year = movie.Year,
                Genre= movie.Genre,
                Rating = movie.Rating ?? 0
            };

            return new GenericResult<MovieDomainModel> 
            {
                IsSuccessful= true,
                Data = domainModel
            };
        }

        public async Task<GenericResult<MovieDomainModel>> UpdateMovie(MovieDomainModel updateMovie) 
        {
            var movieToUpdate =await _moviesRepository.GetByIdAsync(updateMovie.Id);

            if(movieToUpdate == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_UPDATE_ERROR
                };
            }


            movieToUpdate.Id = updateMovie.Id;
            movieToUpdate.Title = updateMovie.Title;
            movieToUpdate.Current = updateMovie.Current;
            movieToUpdate.Year = updateMovie.Year;
            movieToUpdate.Rating = updateMovie.Rating;
            movieToUpdate.Genre = updateMovie.Genre;
            
            
            var movieUpdated = _moviesRepository.Update(movieToUpdate);

            if (movieUpdated == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }
            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = movieUpdated.Id,
                Title = movieUpdated.Title,
                Current = movieUpdated.Current,
                Year = movieUpdated.Year,
                Genre= movieToUpdate.Genre,
                Rating = movieUpdated.Rating ?? 0
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

            if (movieToDelete == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }
            _moviesRepository.Delete(id);

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = movieToDelete.Id,
                Title = movieToDelete.Title,
                Current = movieToDelete.Current,
                Year = movieToDelete.Year,
                Rating = movieToDelete.Rating ?? 0

            };

            return new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                Data =domainModel
            };
        }
    }
}

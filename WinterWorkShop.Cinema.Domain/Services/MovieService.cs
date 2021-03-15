using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
  
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly ITagsMoviesRepository _tagsMoviesRepository;

        public MovieService(IMoviesRepository moviesRepository,
                            ITagsRepository tagsRepository,
                            ITagsMoviesRepository tagsMoviesRepository)
        {
            _moviesRepository = moviesRepository;
           _tagsRepository = tagsRepository;
            _tagsMoviesRepository = tagsMoviesRepository;
        }

        public async Task<GenericResult<MovieDomainModel>> GetAllMoviesAsync(bool? isCurrent)
        {
            var allMovies = await _moviesRepository.GetCurrentMoviesAsync();
           
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
                    Year = item.Year
                    
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
                Genre = newMovie.Genre
            };

            var movie = await _moviesRepository.InsertAsync(movieToCreate);



            _moviesRepository.Save();
            // proveravam da li tag postoji u bazi, ako nepostoji  kreiraj onda novi
            // 
          

           await AddTagsForMovie(movie);

           

          

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

        private async Task AddTagsForMovie(Movie movie)
        {
            var tagExistGenre =  _tagsRepository.GetTagByValue(movie.Genre);

            if(tagExistGenre != null)
            {
                var genreTagsMovies = new TagsMovies
                {
                    Movie = movie,
                    Tag = tagExistGenre,
                    MovieId= movie.Id,
                    TagId = tagExistGenre.TagId
                };
                await _tagsMoviesRepository.InsertAsync(genreTagsMovies);
                _tagsMoviesRepository.Save();


            }
            else
            {
                
                var tagGenreToCreate = new Tag
                {
                    TagValue = movie.Genre,
                    TagName = "Genre"
                };
                var newGenreTag = await _tagsRepository.InsertAsync(tagGenreToCreate);
                _tagsRepository.Save();
                
                
                var genreTagsMovies = new TagsMovies
                {
                    Movie = movie,
                    Tag = newGenreTag,
                    TagId= newGenreTag.TagId
                };
                await _tagsMoviesRepository.InsertAsync(genreTagsMovies);
                
                _tagsMoviesRepository.Save();

           

            }
            var tagExistYear = _tagsRepository.GetTagByValue(movie.Genre);
            if (tagExistYear !=null)
            {
                /// Tag Year to create 
                var tagYearToCreate = new Tag
                {
                    TagName = "Year",
                    TagValue = movie.Year.ToString()
                };



                var newYearTag = await _tagsRepository.InsertAsync(tagYearToCreate);
                _tagsRepository.Save();

                var yearTagsMovies = new TagsMovies
                {
                    Movie = movie,
                    Tag = newYearTag
                };
                await _tagsMoviesRepository.InsertAsync(yearTagsMovies);

                _tagsMoviesRepository.Save();
            }

        }

        public  GenericResult<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie) {

            Movie movieToUpdate = new Movie()
            {
                Id = updateMovie.Id,
                Title = updateMovie.Title,
                Current = updateMovie.Current,
                Year = updateMovie.Year,
                Rating = updateMovie.Rating,
                Genre= updateMovie.Genre
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
                Rating = movieToDelete.Rating ?? 0

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
                      Year= movie.Year
            }).ToList();
            return new GenericResult<MovieDomainModel> 
            { 
            DataList= movies
            };
        }

        public async Task<GenericResult<MovieDomainModel>> ActivateMovie(object movieId)
        {
            var movie =await _moviesRepository.GetByIdAsync(movieId);

            var dataTimeNow = DateTime.Now;

            var upcomingProjections = movie.Projections.Where(project => project.ShowingDate > dataTimeNow)
                .ToList();

            if(upcomingProjections.Count > 0)
            {
               return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_ACTIVATE_DEACTIVATE_ERROR
                };
            }

            movie.Current = !movie.Current;
            _moviesRepository.Update(movie);

            _moviesRepository.SaveAsync();

            return new GenericResult<MovieDomainModel>
            {
            
            IsSuccessful= true,
            Data=  new MovieDomainModel
            {
                 Current= movie.Current,
                  Genre= movie.Genre,
                   Id= movie.Id,
                    Rating= movie.Rating ?? 0,
                     Title= movie.Title,
                      Year= movie.Year
            }
            };

        }
    }
}

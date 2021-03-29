using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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

        enum DefaultTags
        {
           Title,
           Genre,
           Year
        }
        private readonly IMoviesRepository _moviesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly ITagsMoviesRepository _tagsMoviesRepository;
        private readonly ICinemasRepository _cinemasRepository;
        public MovieService(IMoviesRepository moviesRepository,
                            ITagsRepository tagsRepository,
                            ITagsMoviesRepository tagsMoviesRepository,
                            IAuditoriumsRepository auditoriumsRepository,
                            ICinemasRepository cinemasRepository)
        {
            _moviesRepository = moviesRepository;
           _tagsRepository = tagsRepository;
            _tagsMoviesRepository = tagsMoviesRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _cinemasRepository = cinemasRepository;
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
                    Rating = item.Rating,
                    Title = item.Title,
                    Year = item.Year,
                    Genre = item.Genre,
                    CoverPicture = item.CoverPicture,
                    HasOscar = item.HasOscar
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
                Rating = movie.Rating,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                CoverPicture = movie.CoverPicture,
                HasOscar = movie.HasOscar,
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
                    IsSuccessful = false,
                    ErrorMessage =Messages.MOVIE_NOT_IN_AUDITORIUM
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
                    Rating = item.Rating,
                    Title = item.Title,
                    Year = item.Year,
                    CoverPicture = item.CoverPicture,
                    HasOscar = item.HasOscar,
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
                ImdbId= newMovie.Imdb,
                Description=  newMovie.Description
                
            };

            var movie = await _moviesRepository.InsertAsync(movieToCreate);
            
            //_moviesRepository.Save();

            //ovde imam  MOVIE   model koji ima id-0 , a ja sam vec setvao jedan movie gore 
            AddTagsForMovie(newMovie, movie);
            //_moviesRepository.Detach(movie);

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Current = movie.Current,
                Year = movie.Year,
                Genre = movie.Genre,
                Rating = movie.Rating,
                CoverPicture = movie.CoverPicture,
                HasOscar = movie.HasOscar,
                Tags = newMovie.Tags,
                Actors= newMovie.Actors,
                Imdb= newMovie.Imdb
                
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
                Rating = movieUpdated.Rating,
                CoverPicture = movieUpdated.CoverPicture,
                HasOscar = movieUpdated.HasOscar,
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
                Rating = movieToDelete.Rating,
                Genre = movieToDelete.Genre,
                CoverPicture = movieToDelete.CoverPicture,
                HasOscar = movieToDelete.HasOscar,

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
                 Rating= movie.Rating,
                 Title= movie.Title,
                 Year= movie.Year,
                 CoverPicture = movie.CoverPicture,
                 HasOscar = movie.HasOscar,
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
                 Rating= updatedMovie.Rating,
                 Title= updatedMovie.Title,
                 Year= updatedMovie.Year,
                CoverPicture = updatedMovie.CoverPicture,
                HasOscar = updatedMovie.HasOscar,
            }
            };

        }

        public async Task<GenericResult<MovieDomainModel>> SearchMoviesByTag(string tagValue)
        {
          

            var searchResult = await _moviesRepository.SearchMoviesByTags( tagValue);
            
            if(searchResult == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_SEARCH_BY_TAG_NOT_FOUND
                };
            }

            var movies = searchResult.Select(movie => 
                        new MovieDomainModel { 
                            Current= movie.Current, 
                             Genre= movie.Genre,
                              Id= movie.Id,
                               Rating= movie.Rating,
                                Title= movie.Title,
                                 Year= movie.Year
                        }).ToList();
            return new GenericResult<MovieDomainModel> {
            
                IsSuccessful= true,
                DataList= movies
            };
        }
        public async void AddTagsForMovie(MovieDomainModel movieDomainModel, Movie movie)
        {

            foreach (var tag in movieDomainModel.Tags)
            {
                if (tag == "")
                {
                    break;
                }
                var tagExists = _tagsRepository.GetTagByValue(tag);

                if (tagExists != null)
                {
                    var tagsMovies = new TagsMovies
                    {
                        Movie = movie,
                        Tag = tagExists,
                        MovieId = movie.Id,
                        TagId = tagExists.TagId
                    };

                    _tagsRepository.Attach(tagExists);
                    await _tagsMoviesRepository.InsertAsync(tagsMovies);
                }
                else
                {
                    var tagToCreate = new Tag
                    {
                        TagValue = tag,
                        TagName = ""
                    };
                    var newTag = await _tagsRepository.InsertAsync(tagToCreate);

                    var tagsMovies = new TagsMovies
                    {
                        Movie = movie,
                        Tag = newTag,
                        TagId = newTag.TagId
                    };
                    await _tagsMoviesRepository.InsertAsync(tagsMovies);

                   // _tagsMoviesRepository.Save();
                }
            }

            foreach (var actor in movieDomainModel.Actors)
            {
                if (actor == "")
                {
                    break;
                }
                var actorExists = _tagsRepository.GetTagByValue(actor);

                if (actorExists != null)
                {
                    var actorTagsMovies = new TagsMovies
                    {
                        Movie = movie,
                        Tag = actorExists,
                        MovieId = movie.Id,
                        TagId = actorExists.TagId
                    };

                    _tagsRepository.Attach(actorExists);
                    await _tagsMoviesRepository.InsertAsync(actorTagsMovies);
                }
                else
                {
                    var actorTagToCreate = new Tag
                    {
                        TagValue = actor,
                        TagName = ""
                    };
                    var newTagActor = await _tagsRepository.InsertAsync(actorTagToCreate);

                    var tagsMovies = new TagsMovies
                    {
                        Movie = movie,
                        Tag = newTagActor,
                        TagId = newTagActor.TagId
                    };
                    await _tagsMoviesRepository.InsertAsync(tagsMovies);

                   // _tagsMoviesRepository.Save();
                }
            }


            var tagExistGenre = _tagsRepository.GetTagByValue(movie.Genre);
            var tagExistYear = _tagsRepository.GetTagByValue(movie.Year.ToString());
            var tagExistTitle = _tagsRepository.GetTagByValue(movie.Title);

          
            await AddTags(movie, tagExistGenre,"Genre");
            await AddTags(movie, tagExistYear,"Year");
            await AddTags(movie, tagExistTitle,"Title");


            _tagsMoviesRepository.Save();
        }
       private async Task AddTags(Movie movie, Tag tagExist, string tagName)
        {
            if (tagExist != null)
            {
                var genreTagsMovies = new TagsMovies
                {
                    Movie = movie,
                    Tag = tagExist,
                    MovieId = movie.Id,
                    TagId = tagExist.TagId
                };

                _tagsRepository.Attach(tagExist);
                await _tagsMoviesRepository.InsertAsync(genreTagsMovies);
            }
            else
            {
                Tag tagToCreate;
                if (tagName == "Genre")
                {
                     tagToCreate = new Tag
                    {
                        TagValue = movie.Genre,
                        TagName = tagName
                     };
                 }
                else if (tagName == "Year")
                {
                    tagToCreate = new Tag
                    {
                        TagValue = movie.Year.ToString(),
                        TagName = tagName
                    };
                }
                else
                {
                    tagToCreate = new Tag
                    {
                        TagValue = movie.Title,
                        TagName = tagName
                    };
                }
                
                var newTag = await _tagsRepository.InsertAsync(tagToCreate);

                var TagsMovies = new TagsMovies
                {
                    Movie = movie,
                    Tag = newTag,
                    TagId = newTag.TagId
                };
                await _tagsMoviesRepository.InsertAsync(TagsMovies);

            }
        }

        public async Task<GenericResult<MovieDomainModel>> GetMoviesByCinemaId(int id)
        {
            var cinema =await _cinemasRepository.GetByIdAsync(id);
            if (cinema == null)
            {
                return new GenericResult<MovieDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.CINEMA_ID_NOT_FOUND
                };
            }

            var movies =await _moviesRepository.GetMoviesByCinemaId(id);
            if(movies == null)
            {
                return null;
            }

            return new GenericResult<MovieDomainModel>
            {
                IsSuccessful = true,
                DataList =  movies.Select(item => new MovieDomainModel
                {
                    Current = item.Current,
                    Genre = item.Genre,
                    Id = item.Id,
                    Rating = item.Rating,
                    Title = item.Title,
                    Year = item.Year,
                    CoverPicture = item.CoverPicture,
                    HasOscar = item.HasOscar,
                }).ToList()
            };

        }

        public async Task<List<int>> GetAllMoviesBySortedYear()
        {
            var movies = await GetAllMoviesAsync(true);

            if(movies== null)
            {
                return null;
            }

            var result = movies.DataList.OrderByDescending(movie => movie.Year).Select(movie=> movie.Year);

            // 
            return result.Distinct().ToList();
        }

        public async Task<GenericResult<MovieDomainModel>> GetTopTenMoviesBySpecificYear(int year)
        {
            var topTenMoviesBySpecificYear =await _moviesRepository.GetTopTenMoviesBySpecificYear(year);


            var moviesToReturn = topTenMoviesBySpecificYear.Select(movie => new MovieDomainModel {
                Title = movie.Title,
                Rating = movie.Rating,
                Genre = movie.Genre,
                Current = movie.Current,
                CoverPicture = movie.CoverPicture,
                HasOscar = movie.HasOscar,
                Year = movie.Year,
                Id= movie.Id,
                Imdb= movie.ImdbId
                
            }).ToList();

            return new GenericResult<MovieDomainModel>
            {
                IsSuccessful= true,
                DataList = moviesToReturn
            };
        }
    }
}

using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IMovieService
    {
        Task<GenericResult<MovieDomainModel>> GetAllMoviesAsync(bool? isCurrent);
       
        Task<GenericResult<MovieDomainModel>> GetMovieByIdAsync(Guid id);

        Task<GenericResult<MovieDomainModel>> AddMovieAsync(MovieDomainModel newMovie);
       
        GenericResult<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie);

        Task<GenericResult<MovieDomainModel>> DeleteMovieAsync(Guid id);

        Task<GenericResult<MovieDomainModel>> GetTopTenMoviesAsync(string searchCriteria, int year);

        Task<GenericResult<MovieDomainModel>> ActivateDeactivateMovie(Guid movieId);
        Task<GenericResult<MovieDomainModel>> GetMoviesByAuditoriumId(int id);
        //Task<GenericResult<MovieDomainModel>> ActivateMovie(object movieId);

        void AddTagsForMovie(MovieDomainModel movieDomainModel);

        Task<GenericResult<MovieDomainModel>> SearchMoviesByTag(string query);

        Task<GenericResult<MovieDomainModel>> GetMoviesByCinemaId(int id);
    }
}

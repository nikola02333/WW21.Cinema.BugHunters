using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDomainModel>> GetAllMoviesAsync(bool? isCurrent);
       
        Task<GenericResult<MovieDomainModel>> GetMovieByIdAsync(Guid id);

        Task<GenericResult<MovieDomainModel>> AddMovieAsync(MovieDomainModel newMovie);
       
        Task<GenericResult<MovieDomainModel>> UpdateMovie(MovieDomainModel updateMovie);

        Task<GenericResult<MovieDomainModel>> DeleteMovieAsync(Guid id);
    }
}

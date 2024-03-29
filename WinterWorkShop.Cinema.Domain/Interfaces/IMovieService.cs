﻿using Microsoft.Extensions.Primitives;
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


        Task<List<int>> GetAllMoviesBySortedYear();

        Task<GenericResult<MovieDomainModel>> GetTopTenMoviesBySpecificYear(int year);


        Task<GenericResult<MovieDomainModel>> GetMovieByIdAsync(Guid id);

        Task<GenericResult<MovieDomainModel>> AddMovieAsync(MovieDomainModel newMovie);
       
        GenericResult<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie);

        Task<GenericResult<MovieDomainModel>> DeleteMovieAsync(Guid id);

        Task<GenericResult<MovieDomainModel>> GetTopTenMoviesAsync();

        Task<GenericResult<MovieDomainModel>> ActivateDeactivateMovie(Guid movieId);
        Task<GenericResult<MovieDomainModel>> GetMoviesByAuditoriumId(int id);

        void AddTagsForMovie(MovieDomainModel movieDomainModel, Movie movie);

        Task<GenericResult<MovieDomainModel>> SearchMoviesByTag(string query);

        Task<GenericResult<MovieDomainModel>> GetMoviesByCinemaId(int id);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IMoviesRepository : IRepository<Movie> 
    {
        Task<IEnumerable<Movie>> GetCurrentMoviesAsync();

        Task<Movie> GetMovieByNameAsync(string movieName);

        Task<IEnumerable<Movie>> GetTopTenMovies();

        Task<Movie> ActivateDeactivateMovie(Movie movieToActivateDeactivate);

        Task<IEnumerable<Movie>> GetMoviesByAuditoriumId(int id);

        Task<IEnumerable<Movie>> SearchMoviesByTags(string query);
    }
    
    public class MoviesRepository : IMoviesRepository
    {
        private CinemaContext _cinemaContext;

        public MoviesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Movie Delete(object id)
        {
            Movie existing = _cinemaContext.Movies.Find(id);

            if (existing == null)
            {
                return null;
            }

            var result = _cinemaContext.Movies.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _cinemaContext.Movies.ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(object id)
        {
         
            var data = await _cinemaContext.Movies.Where(movie=> movie.Id ==(Guid)id) .Include(p => p.Projections).FirstOrDefaultAsync();

            return data;
        }

        public async Task<IEnumerable<Movie>> GetCurrentMoviesAsync()
        {
            var data = await _cinemaContext.Movies
                .Where(x => x.Current). Include(p => p.Projections).ToListAsync();            

            return data;
        }

        public async Task<Movie> InsertAsync(Movie obj)
        {
            var data = await _cinemaContext.Movies.AddAsync(obj);

            return data.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Movie Update(Movie obj)
        {
            var updatedEntry = _cinemaContext.Movies.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
        public void SaveAsync()
        {
            _cinemaContext.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByNameAsync(string movieName)
        {
           return await _cinemaContext.Movies.Where(movie => movie.Title == movieName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Movie>> GetTopTenMovies()
        {
            var topTenMovies =await _cinemaContext.Movies.OrderByDescending(m => m.Rating).Take(10).ToListAsync();

            return topTenMovies;
        }

        public async Task<Movie> ActivateDeactivateMovie(Movie movieToActivateDeactivatemovieId)
        {
            movieToActivateDeactivatemovieId.Current = !movieToActivateDeactivatemovieId.Current;

            _cinemaContext.Update(movieToActivateDeactivatemovieId);

            await _cinemaContext.SaveChangesAsync();
            // ovde
            return movieToActivateDeactivatemovieId;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByAuditoriumId(int id)
        {
            var movies = await _cinemaContext.Movies.Where(x => x.Projections.Any(p => p.AuditoriumId == id)).ToListAsync();

            return movies;
        }
        public async Task<IEnumerable<Movie>> SearchMoviesByTags(string query)
        {

            
            var searchTag =  _cinemaContext.Tags
                .Where(t => t.TagValue == query)
                .SingleOrDefault();
            

            var listMovies = await _cinemaContext.TagsMovies
                        .Where(tm => tm.TagId == searchTag.TagId)
                        .Include(m => m.Movie)
                        .Select(m => m.Movie)
                        .ToListAsync();


            return listMovies;
        }
    }
}

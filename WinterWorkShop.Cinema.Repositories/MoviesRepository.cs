using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            var data = await _cinemaContext.Movies.FindAsync(id);

            return data;
        }

        public async Task<IEnumerable<Movie>> GetCurrentMoviesAsync()
        {
            var data = await _cinemaContext.Movies
                .Where(x => x.Current).ToListAsync();            

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
    }
}

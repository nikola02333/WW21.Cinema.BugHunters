using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ITagsMoviesRepository : IRepository<TagsMovies>
    {
        void Attach(TagsMovies TagsMovie);

        public Task<TagsMovies> GetTagByMovieIDAsync(Guid movieId);
    }
    public class TagsMoviesRepository : ITagsMoviesRepository
    {
        private readonly CinemaContext _cinemaContext;

        public TagsMoviesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public void Attach(TagsMovies TagsMovie)
        {
            _cinemaContext.TagsMovies.Attach(TagsMovie);
        }

        public TagsMovies Delete(object id)
        {
            var tagMovieToDelete = _cinemaContext.TagsMovies.Find(id);

            return _cinemaContext.TagsMovies.Remove(tagMovieToDelete).Entity;
        }

        public async Task<IEnumerable<TagsMovies>> GetAllAsync()
        {
            return await _cinemaContext.TagsMovies.ToListAsync();
        }

        public async Task<TagsMovies> GetByIdAsync(object id)
        {
            return await _cinemaContext.TagsMovies.FindAsync(id);
        }

        public async Task<TagsMovies> GetTagByMovieIDAsync(Guid movieId)
        {
            //var tagResult =await _cinemaContext.Movies.Where(movie => movie.Id == movieId).Include(tm => tm.TagsMovies).FirstOrDefaultAsync();
            //tagResult.TagsMovies.
            //var tagResult = await _cinemaContext.Tags.Where().Include(tm => tm.TagsMovies);
            return null;
        }

        public async Task<TagsMovies> InsertAsync(TagsMovies obj)
        {
            var tagsMovie = await _cinemaContext.TagsMovies.AddAsync(obj);

            return tagsMovie.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public async void SaveAsync()
        {
          await  _cinemaContext.SaveChangesAsync();
        }

        public TagsMovies Update(TagsMovies obj)
        {
            var updatedEntry = _cinemaContext.TagsMovies.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}

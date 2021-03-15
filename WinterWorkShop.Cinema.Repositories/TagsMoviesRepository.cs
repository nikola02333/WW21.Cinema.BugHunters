using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ITagsMoviesRepository : IRepository<TagsMovies>
    {
        // search tag by specific name and value
       // Task<List<GenericResult<TagsMovies>>> GetTagById();
    }
    public class TagsMoviesRepository : ITagsMoviesRepository
    {
        private readonly CinemaContext _cinemaContext;

        public TagsMoviesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public TagsMovies Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagsMovies>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TagsMovies> GetByIdAsync(object id)
        {
            
            throw new NotImplementedException();
            //var tagMovie = await _cinemaContext.TagsMovies
            // au ovde moram da imam dva id-ja, tagname i movieId, 
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
            throw new NotImplementedException();
        }
    }
}

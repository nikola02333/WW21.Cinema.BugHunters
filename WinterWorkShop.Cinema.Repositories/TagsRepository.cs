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
    public interface ITagsRepository : IRepository<Tag> 
    {
        Tag GetTagByValue(string tagValue);

        Tag GetTagByValue(double tagValue);
    }
    public class TagsRepository : ITagsRepository
    {
        private CinemaContext _cinemaContext;

        public TagsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        public Tag Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tag>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> GetByIdAsync(object id)
        {
            return await _cinemaContext.Tags.FindAsync(id);
        }

        public Tag GetTagByValue(string tagValue)
        {

            return  _cinemaContext.Tags.Where(tag => tag.TagValue == tagValue).FirstOrDefault();
        }

        public Tag GetTagByValue(double tagValue)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> InsertAsync(Tag obj)
        {
            var resilt= await _cinemaContext.Tags.AddAsync(obj);

            return resilt.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public async void SaveAsync()
        {
            await _cinemaContext.SaveChangesAsync();
        }

        public Tag Update(Tag obj)
        {
            throw new NotImplementedException();
        }
    }
}

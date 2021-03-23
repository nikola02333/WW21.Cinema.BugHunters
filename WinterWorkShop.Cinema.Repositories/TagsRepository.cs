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
        Task<IEnumerable<Tag>> GetTagsByValue(string tagValue);

        Tag GetTagByValue(string tagValue);

        Tag GetTagByYear(int tagYear);

        Tag GetTagByRating(double tagRating);

        Task<IEnumerable<Tag>> GetTagsByTagNameAndTagValue(string tagName, string tagValue);

        void Attach(Tag tag);
    }
    public class TagsRepository : ITagsRepository
    {
        private CinemaContext _cinemaContext;

        public TagsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public void Attach(Tag tag)
        {
            _cinemaContext.Tags.Attach(tag);
        }

        public Tag Delete(object id)
        {
           var tagToRemove= _cinemaContext.Tags.Find(id);

            return _cinemaContext.Tags.Remove(tagToRemove).Entity;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _cinemaContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(object id)
        {
            return await _cinemaContext.Tags.FindAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetTagsByValue(string tagValue)
        {
            return await _cinemaContext.Tags.Where(tag => tag.TagValue == tagValue).ToListAsync();
        }
        public Tag GetTagByValue(string tagValue)
        {
            return  _cinemaContext.Tags.Where(tag => tag.TagValue == tagValue).SingleOrDefault();
        }

        public Tag GetTagByRating(double tagRating)
        {
            return _cinemaContext.Tags.Where(tag => tag.TagValue == tagRating.ToString()).FirstOrDefault();
        }

        public Tag GetTagByYear(int tagYear)
        {
            return _cinemaContext.Tags.Where(tag => tag.TagValue == tagYear.ToString()).FirstOrDefault();
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
            var updatedEntry = _cinemaContext.Tags.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }

        public async Task<IEnumerable<Tag>> GetTagsByTagNameAndTagValue(string tagName, string tagValue)
        {
            var result = await _cinemaContext.Tags.Where(t => t.TagName == tagName).Where(t => t.TagValue == tagValue).ToListAsync();
            return result;
        }
    }
}

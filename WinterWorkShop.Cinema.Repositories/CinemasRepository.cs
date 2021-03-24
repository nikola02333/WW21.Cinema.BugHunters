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
    public interface ICinemasRepository : IRepository<Data.Cinema> { }

    public class CinemasRepository : ICinemasRepository
    {
        private CinemaContext _cinemaContext;

        public CinemasRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Data.Cinema Delete(object id)


        {
            Data.Cinema existing = _cinemaContext.Cinemas.Where(c => c.Id ==(int)id).FirstOrDefault();

            var result = _cinemaContext.Cinemas.Remove(existing);


            return result.Entity;
        }

        public async Task<IEnumerable<Data.Cinema>> GetAllAsync()
        {
            var data = await _cinemaContext.Cinemas.ToListAsync();

            return data;
        }

        public async Task<Data.Cinema> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Cinemas.FindAsync(id);

            return data;
        }

        public async Task<Data.Cinema> InsertAsync(Data.Cinema obj)
        {

            var result = await _cinemaContext.Cinemas.AddAsync(obj);
           return result.Entity;
           
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Data.Cinema Update(Data.Cinema obj)
        {
            var updatedEntry = _cinemaContext.Cinemas.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
        public void SaveAsync()
        {
            _cinemaContext.SaveChangesAsync();
        }
    }
}

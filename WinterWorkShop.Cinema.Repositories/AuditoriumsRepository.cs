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
    public interface IAuditoriumsRepository : IRepository<Auditorium> 
    {
      
        Task<IEnumerable<Auditorium>> GetAllByCinemaIdAsync(int cinemaId);
        Task<IEnumerable<Auditorium>> GetByAuditoriumNameAsync(string auditoriumName, int cinemaId);
    }
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private CinemaContext _cinemaContext;

        public AuditoriumsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }


       

        public Auditorium Delete(object id)
        {
           

            Auditorium existing = _cinemaContext.Auditoriums.Include(p => p.Projections).Include(c=>c.Seats).Where(a => a.Id == (int)id).FirstOrDefault();
           
            var result = _cinemaContext.Auditoriums.Remove(existing);



            return result.Entity;
        }

        public async Task<IEnumerable<Auditorium>> GetAllAsync()
        {
            var data = await _cinemaContext.Auditoriums.Include(x => x.Seats).ToListAsync();

            return data;
        }

        public async Task<Auditorium> GetByIdAsync(object id)
        {


            var auditorium = await _cinemaContext.Auditoriums
                .Where(auditorium => auditorium.Id == (int)id)
                .Include(auditorium => auditorium.Seats)
                .FirstOrDefaultAsync();



            return auditorium;


           
        }

        public async Task<Auditorium> InsertAsync(Auditorium obj)
        {
            var data = await _cinemaContext.Auditoriums.AddAsync(obj);

            return data.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Auditorium Update(Auditorium obj)
        {
            var updatedEntry = _cinemaContext.Auditoriums.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
        public async Task<IEnumerable<Auditorium>> GetAllByCinemaIdAsync(int cinemaId)
        {
            return await _cinemaContext.Auditoriums.Where(x => x.CinemaId == cinemaId).ToListAsync();
        }



        public async Task<IEnumerable<Auditorium>> GetByAuditoriumNameAsync(string auditoriumName, int cinemaId)
        {
            var data = await _cinemaContext.Auditoriums
                .Where(x => x.AuditoriumName.Equals(auditoriumName)
                            && x.CinemaId.Equals(cinemaId))
                .ToListAsync();



            return data;
        }
        public void SaveAsync()
        {
            _cinemaContext.SaveChangesAsync();
        }
    }
}

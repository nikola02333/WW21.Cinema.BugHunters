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
    public interface ITicketsRepository : IRepository<Ticket>
    {

    }
    public class TicketsRepository : ITicketsRepository
    {
        private CinemaContext _cinemaContext;

        public TicketsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Ticket Delete(object id)
        {
            var existing = _cinemaContext.Tickets.Find(id);
            var result = _cinemaContext.Tickets.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            var data = await _cinemaContext.Tickets.Include(x => x.Projection).ThenInclude(x => x.Movie)
                                                    .Include(x => x.Projection).ThenInclude(x => x.Auditorium)
                                                    .Include(x => x.User).Include(x => x.Seat).ToListAsync();
            return data;
        }

        public async Task<Ticket> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Tickets.Where(x=>x.Id == (Guid)id).Include(x => x.Projection).ThenInclude(x => x.Movie)
                                                    .Include(x => x.Projection).ThenInclude(x => x.Auditorium)
                                                    .Include(x => x.User).Include(x => x.Seat).FirstAsync();
            return data;
        }

        public async Task<Ticket> InsertAsync(Ticket obj)
        {
            var data =await _cinemaContext.Tickets.AddAsync(obj);

            return data.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Ticket Update(Ticket obj)
        {
            var updatedEntry = _cinemaContext.Tickets.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}

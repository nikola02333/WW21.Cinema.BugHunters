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
    public interface ISeatsRepository : IRepository<Seat> 
    {
        Task<Seat> isSeatInProjectionAuditoriumAsync(Guid seatId, Guid projectionId);

        Task<IEnumerable<Seat>> getReservedSeatsForProjection(Guid projectionId);
    }
    public class SeatsRepository : ISeatsRepository
    {
        private CinemaContext _cinemaContext;

        public SeatsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Seat Delete(object id)
        {
            Seat existing = _cinemaContext.Seats.Find(id);
            var result = _cinemaContext.Seats.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            var data = await _cinemaContext.Seats.ToListAsync();

            return data;
        }

        public async Task<Seat> GetByIdAsync(object id)
        {
            return await _cinemaContext.Seats.FindAsync(id);
        }

        public async Task<Seat> InsertAsync(Seat obj)
        {
            var data = await _cinemaContext.Seats.AddAsync(obj);

            return data.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Seat Update(Seat obj)
        {
            var updatedEntry = _cinemaContext.Seats.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
        public void SaveAsync()
        {
            _cinemaContext.SaveChangesAsync();
        }

        public async Task<Seat> isSeatInProjectionAuditoriumAsync(Guid seatId, Guid projectionId)
        {
            var seat =await _cinemaContext.Seats.Include(x => x.Auditorium).Include(x => x.Auditorium.Seats).Include(x => x.Auditorium.Projections)
                .Where(x => x.Id == (seatId) && x.Auditorium.Projections.Select(proj => proj.Id).SingleOrDefault() == projectionId).SingleOrDefaultAsync();

            return seat;
        }

        public async Task<IEnumerable<Seat>> getReservedSeatsForProjection(Guid projectionId)
        {
            //var tickets = _cinemaContext.Seats.Include(x => x.Tickets).Where(x => x.Tickets.Any(x=>x.ProjectionId == projectionId)).ToList();

            var seats = _cinemaContext.Seats.Include(x => x.Tickets).ToList();

            List<Seat> result = new List<Seat>();
            foreach (var item in seats)
            {
                var x = item.Tickets.Any(x => x.ProjectionId == projectionId);
                if (x)
                {
                    result.Add(item);
                }

            }
            return result;
        }
    }
}

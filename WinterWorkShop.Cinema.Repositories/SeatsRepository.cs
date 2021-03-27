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
        Task<Seat> GetSeatInProjectionAuditoriumAsync(Guid seatId, Guid projectionId);
        Task<IEnumerable<Seat>> GetReservedSeatsForProjectionAsync(Guid projectionId);
        Task<IEnumerable<Seat>> GetSeatsByAuditoriumIdAsync(int auditoriumId);
        void Attach(Seat seat);
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

        public async Task<Seat> GetSeatInProjectionAuditoriumAsync(Guid seatId, Guid projectionId)
        {
            var seat =await _cinemaContext.Seats.Include(x => x.Auditorium).Include(x => x.Auditorium.Projections)
                .Where(x => x.Id == (seatId) && x.Auditorium.Projections.Any(proj => proj.Id == projectionId)).SingleOrDefaultAsync();

            return seat;
        }

        public async Task<IEnumerable<Seat>> GetReservedSeatsForProjectionAsync(Guid projectionId)
        {
            var seats = await _cinemaContext.Seats.Where(x => x.Tickets.Any(x => x.Projection.Id == projectionId)).ToListAsync();

            return seats;
        }

        public async Task<IEnumerable<Seat>> GetSeatsByAuditoriumIdAsync(int auditoriumId)
        {
            var seats =await _cinemaContext.Seats.Where(x => x.AuditoriumId == auditoriumId).OrderBy(x=>x.Row).ThenBy(x=>x.Number).ToListAsync();

            return seats;
        }

        public void Attach(Seat seat)
        {
            _cinemaContext.Attach(seat);
        }
    }
}

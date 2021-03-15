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
    public interface IProjectionsRepository : IRepository<Projection> 
    {
        IEnumerable<Projection> GetByAuditoriumId(int salaId);
        void Attach(Projection projection);

        Task<IEnumerable<Projection>> FilterProjection(int? CinemaId, int? AuditoriumId, Guid? MovieId, DateTime? DateTime);
    }

    public class ProjectionsRepository : IProjectionsRepository
    {
        private CinemaContext _cinemaContext;

        public ProjectionsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Projection Delete(object id)
        {
            Projection existing = _cinemaContext.Projections.Find(id);
            var result = _cinemaContext.Projections.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<Projection>> GetAllAsync()
        {
            var data = await _cinemaContext.Projections.Include(x => x.Movie).Include(x => x.Auditorium).ToListAsync();
            
            return data;           
        }

        public async Task<Projection> GetByIdAsync(object id)
        {
            return await _cinemaContext.Projections.FindAsync(id);
        }

        public  IEnumerable<Projection> GetByAuditoriumId(int auditoriumId)
        {
            var projectionsData = _cinemaContext.Projections.Where(x => x.AuditoriumId == auditoriumId);

            return projectionsData;
        }

        public async Task<Projection> InsertAsync(Projection obj)
        {
            var data = await _cinemaContext.Projections.AddAsync(obj);

            return data.Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Projection Update(Projection obj)
        {
            var updatedEntry = _cinemaContext.Projections.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
        public void SaveAsync()
        {
            _cinemaContext.SaveChangesAsync();
        }
        public void Attach(Projection projection)
        {
            _cinemaContext.Attach(projection);
        }

        public async Task<IEnumerable<Projection>> FilterProjection(int? CinemaId, int? AuditoriumId, Guid? MovieId, DateTime? DateTime)
        {
            var projections =await _cinemaContext.Projections.Where(x => (CinemaId == null || x.Auditorium.CinemaId == CinemaId)
                                               && (AuditoriumId == null || x.AuditoriumId == AuditoriumId)
                                               && (MovieId == null || x.MovieId == MovieId)
                                               && (DateTime == null || x.ShowingDate.Date == DateTime.Value.Date)).ToListAsync();
            return projections;
        }
    }
}

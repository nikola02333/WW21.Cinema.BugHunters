using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IUsersRepository : IRepository<User> 
    {
        Task<User> GetByUserName(string username);
        void Attach(User user);
    }
    public class UsersRepository : IUsersRepository
    {
        private CinemaContext _cinemaContext;

        public UsersRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public User Delete(object id)
        {
            User existing = _cinemaContext.Users.Find(id);
            var result = _cinemaContext.Users.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var data = await _cinemaContext.Users.ToListAsync();

            return data;
        }

        public async Task<User> GetByIdAsync(object id)
        {
            return await _cinemaContext.Users.FindAsync(id);
        }

        public async Task<User> GetByUserNameAsync(string username)
        {
            var data = await _cinemaContext.Users.Where(x => x.UserName == username).SingleOrDefaultAsync();

            return data;
        }

        public async Task<User> InsertAsync(User obj)
        {
            var result = await _cinemaContext.Users.AddAsync(obj);

            return result.Entity;
       
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }
        

        public User Update(User obj)
        {
            var updatedEntry = _cinemaContext.Users.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
        public void SaveAsync()
        {
            _cinemaContext.SaveChangesAsync();
        }

        public void Attach(User user)
        {
            _cinemaContext.Attach(user);
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Delete(object id);

        Task<IEnumerable<T>> GetAll();

        Task<T> GetByIdAsync(object id);

        Task<T> InsertAsync(T obj);

        void Save();

        T Update(T obj);
    }
}

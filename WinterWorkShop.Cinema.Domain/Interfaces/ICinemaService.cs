using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ICinemaService
    {
        Task<GenericResult<CinemaDomainModel>> GetAllAsync();
        Task<GenericResult<CinemaDomainModel>> AddCinemaAsync(CinemaDomainModel newCinema);

        Task<GenericResult<CinemaDomainModel>> DeleteCinemaAsync(int id);
        Task<GenericResult<CinemaDomainModel>> GetCinemaById(int id);

        Task<GenericResult<CinemaDomainModel>> UpdateCinema(CinemaDomainModel updateCinema);
    }
}

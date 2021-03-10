using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDomainModel>> GetAllAsync();
        Task<GenericResult<CinemaDomainModel>> AddCinemaAsync(CinemaDomainModel newCinema);

        GenericResult<CinemaDomainModel> DeleteCinema(int id);
        Task<GenericResult<CinemaDomainModel>> GetCinemaById(int id);

        Task<GenericResult<CinemaDomainModel>> UpdateCinema(CinemaDomainModel updateCinema);
    }
}

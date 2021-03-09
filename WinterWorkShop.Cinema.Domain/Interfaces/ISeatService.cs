using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ISeatService
    {
        Task<GenericResult<SeatDomainModel>> GetAllAsync();
        Task<GenericResult<SeatDomainModel>> ReservedSeatsAsync(Guid projectionId);
        Task<GenericResult<SeatDomainModel>> GetByAuditoriumIdAsync(int auditoriumId);
    }
}

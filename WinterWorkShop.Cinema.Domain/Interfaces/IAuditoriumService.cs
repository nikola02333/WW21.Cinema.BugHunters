﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IAuditoriumService
    {
        Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync();

        Task<GenericResult<AuditoriumDomainModel>> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats);

        Task<GenericResult<AuditoriumDomainModel>> GetAllAuditoriumByCinemaId(int id);
       Task<GenericResult<AuditoriumDomainModel>> DeleteAsync(int id);
        Task<GenericResult<AuditoriumDomainModel>> GetByIdAsync(int id);

    }
}

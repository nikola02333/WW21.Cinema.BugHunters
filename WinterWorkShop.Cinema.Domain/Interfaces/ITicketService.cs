using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ITicketService
    {
        Task<GenericResult<TicketDomainModel>> GetAllAsync();

        Task<GenericResult<TicketDomainModel>> GetTicketByIdAsync(Guid id);

        Task<GenericResult<TicketDomainModel>> CreateTicketAsync(CreateTicketModel ticketToCreate);

        Task<GenericResult<TicketDomainModel>> UpdateTicketAsync(TicketDomainModel updatedTicket);

        Task<GenericResult<TicketDomainModel>> DeleteTicketAsync(Guid id);

    }
}

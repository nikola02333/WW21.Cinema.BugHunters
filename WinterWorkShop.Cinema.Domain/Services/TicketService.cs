using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ISeatsRepository _seatsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProjectionsRepository _projectionsRepository;

        public TicketService(ITicketsRepository ticketsRepository,ISeatsRepository seatsRepository,IUsersRepository usersRepository, IProjectionsRepository projectionsRepository)
        {
            _ticketsRepository = ticketsRepository;
            _seatsRepository = seatsRepository;
            _usersRepository = usersRepository;
            _projectionsRepository = projectionsRepository;
        }
        public async Task<GenericResult<TicketDomainModel>> CreateTicketAsync(CreateTicketModel ticketToCreate)
        {
            string error = null;

            var reservedSeats = await _seatsRepository.GetReservedSeatsForProjectionAsync(ticketToCreate.ProjectionId);
            var exist = reservedSeats.FirstOrDefault(x => x.Id == ticketToCreate.SeatId);
            if (exist!=null)
            {
                return new GenericResult<TicketDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.SEAT_RESERVED
                };
            }
            var seat = await _seatsRepository.GetByIdAsync(ticketToCreate.SeatId);
            if (seat==null){
                error += Messages.SEAT_GET_BY_ID + "   ";
            }
            else
            {
                _seatsRepository.Attach(seat);
            }
            

            var checkSeatiInProjection = await _seatsRepository.GetSeatInProjectionAuditoriumAsync(ticketToCreate.SeatId, ticketToCreate.ProjectionId);
            if (checkSeatiInProjection == null )
            {
                error += Messages.SEAT_NOT_IN_AUDITORIUM_OF_PROJECTION +  "   ";
            }

            var user = await _usersRepository.GetByIdAsync(ticketToCreate.UserId);
            if (user == null)
            {
                error += Messages.USER_GET_BY_ID + "   ";
            }
            else
            {
                _usersRepository.Attach(user);
            }
            

            var projection = await _projectionsRepository.GetByIdAsync(ticketToCreate.ProjectionId);
            if (projection == null)
            {
                error += Messages.PROJECTION_GET_BY_ID + "\n";
            }
            else
            {
                _projectionsRepository.Attach(projection);
            }
            

            if (error != null)
            {
                return new GenericResult<TicketDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = error
                };
            }

            Ticket ticket = new Ticket
            {
                Created = DateTime.Now,
                Id = Guid.NewGuid(),
                ProjectionId=projection.Id,
                Projection=projection,
                SeatId=seat.Id,
                Seat=seat,
                UserId=user.Id,
                User=user
            };

            var insertedTicket = await _ticketsRepository.InsertAsync(ticket);
            if (insertedTicket == null)
            {
                return new GenericResult<TicketDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_CREATION_ERROR
                };
            }
            _ticketsRepository.Save();

            return new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                Data = createTicketDomainModel(insertedTicket)
            };
        }

        public async Task<GenericResult<TicketDomainModel>> DeleteTicketAsync(Guid id)
        {
            var ticketForDelete = await _ticketsRepository.GetByIdAsync(id);

            if (ticketForDelete==null)
            {
                return new GenericResult<TicketDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_GET_BY_ID
                };
            }

           var deletedTicket = _ticketsRepository.Delete(id);
            _ticketsRepository.Save();

            return new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                Data = createTicketDomainModel(deletedTicket)
            };
        }

        public async Task<GenericResult<TicketDomainModel>> GetAllAsync()
        {
            var tickets = await _ticketsRepository.GetAllAsync();


            if (tickets == null)
            {
                return new GenericResult<TicketDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_GET_ALL_TICKETS_ERROR
                };
            }

            List<TicketDomainModel> result = new List<TicketDomainModel>();
            TicketDomainModel model;
            foreach(var item in tickets)
            {
                model = createTicketDomainModel(item);
                result.Add(model);
            }


            return new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                DataList = result
            };

        }

        public async Task<GenericResult<TicketDomainModel>> GetTicketByIdAsync(Guid id)
        {
            var ticket = await _ticketsRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                return new GenericResult<TicketDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_GET_BY_ID
                };
            }

            return new GenericResult<TicketDomainModel>
            {
                IsSuccessful = true,
                Data = createTicketDomainModel(ticket)
            };
        }

        public Task<GenericResult<TicketDomainModel>> UpdateTicketAsync(TicketDomainModel updatedTicket)
        {
            throw new NotImplementedException();
        }
        public TicketDomainModel createTicketDomainModel(Ticket ticket)
        {
            return new TicketDomainModel
            {
                Created = ticket.Created,
                Id = ticket.Id,
                ProjectionId = ticket.ProjectionId,
                Projection = new ProjectionDomainModel
                {
                    Id = ticket.Projection.Id,
                    AditoriumName = ticket.Projection.Auditorium.AuditoriumName,
                    AuditoriumId = ticket.Projection.AuditoriumId,
                    MovieId = ticket.Projection.MovieId,
                    MovieTitle = ticket.Projection.Movie.Title,
                    ProjectionTime = ticket.Projection.ShowingDate,
                    Duration = ticket.Projection.Duration
                    
                },
                SeatId = ticket.SeatId,
                Seat = new SeatDomainModel
                {
                    AuditoriumId = ticket.Seat.AuditoriumId,
                    Id = ticket.Seat.Id,
                    Number = ticket.Seat.Number,
                    Row = ticket.Seat.Row
                },
                UserId = ticket.UserId,
                User = new UserDomainModel
                {
                    Id = ticket.User.Id,
                    FirstName = ticket.User.FirstName,
                    LastName = ticket.User.LastName,
                    UserName = ticket.User.UserName,
                    Role = ticket.User.Role
                }
            };
        }
    }
}

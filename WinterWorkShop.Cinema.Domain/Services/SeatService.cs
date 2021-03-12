using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatsRepository _seatsRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IProjectionsRepository _projectionsRepository;

        public SeatService(ISeatsRepository seatsRepository,IAuditoriumsRepository auditoriumsRepository,IProjectionsRepository projectionsRepository)
        {
            _seatsRepository = seatsRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _projectionsRepository = projectionsRepository;
        }

        public async Task<GenericResult<SeatDomainModel>> GetAllAsync()
        {
            var data = await _seatsRepository.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            List<SeatDomainModel> result = new List<SeatDomainModel>();
            SeatDomainModel model;
            foreach (var item in data)
            {
                model = new SeatDomainModel
                {
                    Id = item.Id,
                    AuditoriumId = item.AuditoriumId,
                    Number = item.Number,
                    Row = item.Row
                };
                result.Add(model);
            }

            return new GenericResult<SeatDomainModel>
            {
                IsSuccessful=true,
                DataList = result
            };
        }

        public async Task<GenericResult<SeatDomainModel>> GetByAuditoriumIdAsync(int auditoriumId)
        {
            var auditorium = await _auditoriumsRepository.GetByIdAsync(auditoriumId);
            if (auditorium==null)
            {
                return new GenericResult<SeatDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR
                };
            }

            var seats = await _seatsRepository.GetSeatsByAuditoriumIdAsync(auditoriumId);

            if (seats == null)
            {
                return new GenericResult<SeatDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.SEATS_IN_AUDITORIUM
                };
            }

            return new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = seats.Select(item => new SeatDomainModel
                {
                    AuditoriumId=item.AuditoriumId,
                    Id= item.Id,
                    Number = item.Number,
                    Row = item.Row 
                }).ToList()
            };
        }

        public async Task<GenericResult<SeatDomainModel>> GetReservedSeatsAsync(Guid projectionId)
        {
            var projection = await _projectionsRepository.GetByIdAsync(projectionId);
            if (projection ==null)
            {
                return new GenericResult<SeatDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_GET_BY_ID
                };
            }

            var seats = await _seatsRepository.GetReservedSeatsForProjectionAsync(projectionId);

            if (seats == null)
            {
                return null;
            }

            return new GenericResult<SeatDomainModel>
            {
                IsSuccessful = true,
                DataList = seats.Select(item => new SeatDomainModel
                {
                    AuditoriumId = item.AuditoriumId,
                    Id = item.Id,
                    Number = item.Number,
                    Row = item.Row
                }).ToList()
            };
        }
    }
}

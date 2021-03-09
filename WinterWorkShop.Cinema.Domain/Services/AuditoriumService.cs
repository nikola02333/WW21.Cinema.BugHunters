using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ICinemasRepository _cinemasRepository;
        private readonly ISeatsRepository _seatsRepository;
        private readonly IProjectionsRepository _projectionsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository, ICinemasRepository cinemasRepository, ISeatsRepository seatsRepository, IProjectionsRepository projectionsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _cinemasRepository = cinemasRepository;
            _seatsRepository = seatsRepository;
            _projectionsRepository = projectionsRepository;

    }

        public async Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            var data = await _auditoriumsRepository.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            List<AuditoriumDomainModel> result = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel model;
            foreach (var item in data)
            {
                model = new AuditoriumDomainModel
                {
                    Id = item.Id,
                    CinemaId = item.CinemaId,
                    Name = item.AuditoriumName,
                    SeatsList= item.Seats.Select(seat => new SeatDomainModel
                    {
                        Id = seat.Id,
                        AuditoriumId = seat.AuditoriumId,
                        Number = seat.Number,
                        Row = seat.Row
                    }).ToList()
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<GenericResult<AuditoriumDomainModel>> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(domainModel.CinemaId);
            if (cinema == null)
            {
                return new GenericResult<AuditoriumDomainModel>
                {
                    IsSuccessful = false,
                     ErrorMessage = Messages.AUDITORIUM_UNVALID_CINEMAID
                };
            }

            var auditorium = await _auditoriumsRepository.GetByAuditoriumNameAsync(domainModel.Name, domainModel.CinemaId);
          
            if (auditorium != null)
            {
                return new GenericResult<AuditoriumDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_SAME_NAME
                };
            }

            Auditorium newAuditorium = new Auditorium
            {
                AuditoriumName = domainModel.Name,
                CinemaId = domainModel.CinemaId,
            };

            newAuditorium.Seats = new List<Seat>();

            for (int i = 1; i <= numberOfRows; i++)
            {
                for (int j = 1; j <= numberOfSeats; j++)
                {
                    Seat newSeat = new Seat()
                    {
                        Row = i,
                        Number = j
                        
                    };

                    newAuditorium.Seats.Add(newSeat);
                }
            }

            Auditorium insertedAuditorium = await _auditoriumsRepository.InsertAsync(newAuditorium);
            _auditoriumsRepository.Save();

            if (insertedAuditorium == null)
            {
                return new GenericResult<AuditoriumDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
                };
            }

            GenericResult<AuditoriumDomainModel> resultModel = new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Data = new AuditoriumDomainModel
                {
                    Id = insertedAuditorium.Id,
                    Name = insertedAuditorium.AuditoriumName,
                    CinemaId = insertedAuditorium.CinemaId,
                    SeatsList = new List<SeatDomainModel>()
                }
            };

            foreach (var item in insertedAuditorium.Seats)
            {
                resultModel.Data.SeatsList.Add(new SeatDomainModel
                {
                    AuditoriumId = insertedAuditorium.Id,
                    Id = item.Id,
                    Number = item.Number,
                    Row = item.Row
                });
            }

            return resultModel;
        }


        public async Task<GenericResult<AuditoriumDomainModel>> GetByIdAsync(int id)
        {
            var auditorium = await _auditoriumsRepository.GetByIdAsync(id);
            if (auditorium == null)
            {
                return new GenericResult<AuditoriumDomainModel>
                {
                    ErrorMessage =Messages.AUDITORIUM_GET_BY_ID_ERROR,
                    IsSuccessful = false
                };
            }

           

            var result = new AuditoriumDomainModel
            {
                Id = auditorium.Id,
                CinemaId = auditorium.CinemaId,
                Name = auditorium.AuditoriumName,
                SeatsList = auditorium.Seats.Select(seat => new SeatDomainModel
                {
                    Id = seat.Id,
                    AuditoriumId = seat.AuditoriumId,
                    Number = seat.Number,
                    Row = seat.Row
                }).ToList()
               
        };


            return new GenericResult<AuditoriumDomainModel>
            {
                Data = result,
                IsSuccessful = true
            };
        }

       

        public async Task<GenericResult<AuditoriumDomainModel>> DeleteAsync(int id)
        {
            // Seats deleted
            var existing =await _auditoriumsRepository.GetByIdAsync(id);

            if (existing.Seats.Count > 0)
            {

                foreach (var seat in existing.Seats)
                {
                    _seatsRepository.Delete(seat.Id);
          
                }
            }


            
            // Projections deleted
            var projections = _projectionsRepository.GetByAuditoriumId(id);

            if (projections != null)
            {
                foreach (var projection in projections)
                {
                    _projectionsRepository.Delete(projection.Id);
                }
            }
           



            // Auditorium deleted
            _auditoriumsRepository.Delete(id);

            _auditoriumsRepository.Save();


           AuditoriumDomainModel auditorium = new AuditoriumDomainModel
           {
              Id=existing.Id,
              CinemaId=existing.CinemaId,
              Name=existing.AuditoriumName,
              SeatsList=existing.Seats.Select(seat => new SeatDomainModel
              {
                  Id = seat.Id,
                  AuditoriumId = seat.AuditoriumId,
                  Number = seat.Number,
                  Row = seat.Row
              }).ToList()
           };

            return new GenericResult<AuditoriumDomainModel>
            {
                IsSuccessful = true,
                Data = auditorium
            };
        }
        

       
    }
}

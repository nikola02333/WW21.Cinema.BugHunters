using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemasRepository _cinemasRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public CinemaService(ICinemasRepository cinemasRepository, IAuditoriumsRepository auditoriumsRepository)
        {
            _cinemasRepository = cinemasRepository;
            _auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<GenericResult<CinemaDomainModel>> GetAllAsync()
        {
            var data = await _cinemasRepository.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            List<CinemaDomainModel>result = new List<CinemaDomainModel>();
            CinemaDomainModel model;
           
            
            foreach (var item in data)
            {
                model = new CinemaDomainModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Address=item.Address,
                    CityName=item.CityName

                };
                result.Add(model);
            }

            return new GenericResult<CinemaDomainModel>
            { 
            DataList=result,
            IsSuccessful=true
            };
        }

        public async Task<GenericResult<CinemaDomainModel>> AddCinemaAsync(CinemaDomainModel newCinema)
        { 
            var cinema = await _cinemasRepository.GetByIdAsync(newCinema.Id);

            if (cinema != null)
            {
                return new GenericResult<CinemaDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = ("Cinema already exists")

                };
            
            }

            var cinemaToAdd = new Data.Cinema
            {
             Id=newCinema.Id,
             CityName=newCinema.CityName,
             Address=newCinema.Address,
             Name=newCinema.Name

            };

            var result = await _cinemasRepository.InsertAsync(cinemaToAdd);
            _cinemasRepository.Save();


            CinemaDomainModel domainModel = new CinemaDomainModel
            {
               Id=result.Id,
                CityName = result.CityName,
                Address = result.Address,
                Name = result.Name


            };

            return new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = domainModel

            };
        }

        public async Task<GenericResult<CinemaDomainModel>> DeleteCinemaAsync(int id)
        {

            var existingAuditoriums =await _auditoriumsRepository.GetAllByCinemaIdAsync(id);
   

            foreach(var auditorium in existingAuditoriums)
            {
                _auditoriumsRepository.Delete(auditorium.Id);
            }
        
          
            var existingCinema = await _cinemasRepository.GetByIdAsync(id);

            var deletedCinema = _cinemasRepository.Delete(id);

            _cinemasRepository.Save();

            if (deletedCinema == null)
            {
                return null;
            }

            CinemaDomainModel cinema = new CinemaDomainModel
            {
                Id = deletedCinema.Id,
                Address = deletedCinema.Address,
                CityName = deletedCinema.CityName,
                Name=deletedCinema.Name
            };


            return new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data=cinema
            };
        }

        public async Task<GenericResult<CinemaDomainModel>> GetCinemaById(int id)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(id);

            if (cinema == null)
            {
                return new GenericResult<CinemaDomainModel>
                {
                    IsSuccessful = false,
                   ErrorMessage= Messages.CINEMA_ID_NOT_FOUND
                };

            }

            CinemaDomainModel domainModel = new CinemaDomainModel
            {
                Id = cinema.Id,
                Address = cinema.Address,
                CityName = cinema.CityName,
                Name = cinema.Name

            };

            return new GenericResult<CinemaDomainModel>
            {
                IsSuccessful=true,
                Data=domainModel
            };
        
        
        }


        public async Task<GenericResult<CinemaDomainModel>> UpdateCinema(CinemaDomainModel updateCinema)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(updateCinema.Id);
            if (cinema == null)
            {
                return new GenericResult<CinemaDomainModel>
                {
                    ErrorMessage="Cinema not found",
                    IsSuccessful=false
                };
            }


            cinema.Id = updateCinema.Id;
            cinema.Name = updateCinema.Name;
            cinema.Address = updateCinema.Address;
            cinema.CityName = updateCinema.CityName;
        

            _cinemasRepository.Update(cinema);
            _cinemasRepository.Save();

            return new GenericResult<CinemaDomainModel>
            {
                IsSuccessful = true,
                Data = new CinemaDomainModel
                {
                 Id=cinema.Id,
                 Name=cinema.Name,
                 CityName=cinema.CityName,
                 Address=cinema.Address

                }
            };

        }




    }

}

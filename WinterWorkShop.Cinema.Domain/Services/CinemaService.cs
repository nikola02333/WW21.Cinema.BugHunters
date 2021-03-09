using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemasRepository _cinemasRepository;

        public CinemaService(ICinemasRepository cinemasRepository)
        {
            _cinemasRepository = cinemasRepository;
        }

        public async Task<GenericResult<CinemaDomainModel>> GetAllAsync()
        {
            var data = await _cinemasRepository.GetAll();

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

        public GenericResult<CinemaDomainModel> DeleteCinema(int id)
        {
           

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
            var data = await _cinemasRepository.GetByIdAsync(id);

            CinemaDomainModel domainModel = new CinemaDomainModel
            {
                Id = data.Id,
                Address = data.Address,
                CityName = data.CityName,
                Name = data.Name

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
            cinema.Name = updateCinema.Name;

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

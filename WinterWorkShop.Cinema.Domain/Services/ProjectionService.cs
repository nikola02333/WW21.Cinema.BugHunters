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
    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionsRepository _projectionsRepository;
        private readonly ICinemasRepository _cinemasRepository;
        private readonly IAuditoriumsRepository _auditoruimsRepository;
        private readonly IMoviesRepository _moviesRepository;

        public ProjectionService(IProjectionsRepository projectionsRepository, 
                                ICinemasRepository cinemasRepository, 
                                IAuditoriumsRepository auditoruimsRepository,
                               IMoviesRepository moviesRepository)
        {
            _projectionsRepository = projectionsRepository;
            _cinemasRepository = cinemasRepository;
            _auditoruimsRepository = auditoruimsRepository;
            _moviesRepository = moviesRepository;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetAllAsync()
        {
            var data = await _projectionsRepository.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            List<ProjectionDomainModel> result = new List<ProjectionDomainModel>();
            ProjectionDomainModel model;
            foreach (var item in data)
            {
                model = new ProjectionDomainModel
                {
                    Id = item.Id,
                    MovieId = item.MovieId,
                    AuditoriumId = item.AuditoriumId,
                    ProjectionTime = item.ShowingDate,
                    MovieTitle = item.Movie.Title,
                    AditoriumName = item.Auditorium.AuditoriumName,
                    Duration = item.Duration,
                    Price = item.Price
                    
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<GenericResult<ProjectionDomainModel>> FilterProjectionAsync(FilterProjectionDomainModel filter)
        {
            var projections =await _projectionsRepository.GetAllAsync();
            Data.Cinema cinema=null;
            Auditorium auditorium = null;

            if (filter.CinemaId!=null)
            {
                cinema =await _cinemasRepository.GetByIdAsync(filter.CinemaId);
                if(cinema == null)
                {
                    return new GenericResult<ProjectionDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.CINEMA_ID_NOT_FOUND
                    };
                }

                projections = projections.Where(x => x.Auditorium.CinemaId == filter.CinemaId).ToList();
            }

            if (filter.AuditoriumId != null)
            {
                
                auditorium = await _auditoruimsRepository.GetByIdAsync(filter.AuditoriumId);
                if (auditorium == null)
                {
                    return new GenericResult<ProjectionDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.AUDITORIUM_GET_BY_ID_ERROR
                    };
                }

                if (cinema != null)
                {
                    var audititoriumInCinema = await _auditoruimsRepository.GetAllByCinemaIdAsync(cinema.Id);
                    if (!audititoriumInCinema.Any(x=>x.Id==filter.AuditoriumId))
                    {
                        return new GenericResult<ProjectionDomainModel>
                        {
                            IsSuccessful = false,
                            ErrorMessage = Messages.AUDITORIUM_NOT_IN_CINEMA
                        };
                    }
                }
                projections = projections.Where(x => x.AuditoriumId == filter.AuditoriumId).ToList();
            }


            if (filter.MovieId != null)
            {
                
                var movie = await _moviesRepository.GetByIdAsync(filter.MovieId);
                if (movie == null)
                {
                    return new GenericResult<ProjectionDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.MOVIE_GET_BY_ID
                    };
                }
                
                if (auditorium != null)
                {
                    var movieInAuditorium = await _moviesRepository.GetMoviesByAuditoriumId(auditorium.Id);
                    if (!movieInAuditorium.Any(x => x.Id == filter.MovieId))
                    {
                        return new GenericResult<ProjectionDomainModel>
                        {
                            IsSuccessful = false,
                            ErrorMessage = Messages.MOVIE_NOT_IN_AUDITORIUM
                        };
                    }
                }
                projections = projections.Where(x => x.MovieId == filter.MovieId).ToList();
            }

            if (filter.DateTime != null)
            {
                projections = projections.Where(x => x.ShowingDate.Date == filter.DateTime.Value.Date).ToList();
            }

            return new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                DataList = projections.Select(item => new ProjectionDomainModel
                {
                    Id = item.Id,
                    AuditoriumId = item.AuditoriumId,
                    MovieId = item.MovieId,
                    ProjectionTime = item.ShowingDate,
                    Duration = item.Duration,
                    Price = item.Price
                }).ToList()
            };

        }

        public async Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel)
        {
            int projectionTime = 3;

            var projectionsAtSameTime = _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId)
                .Where(x => x.ShowingDate < domainModel.ProjectionTime.AddHours(projectionTime) && x.ShowingDate > domainModel.ProjectionTime.AddHours(-projectionTime))
                .ToList();

            if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0)
            {
                return new CreateProjectionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTIONS_AT_SAME_TIME
                };
            }

            var newProjection = new Data.Projection
            {
                MovieId = domainModel.MovieId,
                AuditoriumId = domainModel.AuditoriumId,
                 ShowingDate = domainModel.ProjectionTime,
                 Duration = domainModel.Duration,
                 Price = domainModel.Price
                 
            };

            var insertedProjection = await _projectionsRepository.InsertAsync(newProjection);

            if (insertedProjection == null)
            {
                return new CreateProjectionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_CREATION_ERROR
                };
            }
            _projectionsRepository.Save();
            CreateProjectionResultModel result = new CreateProjectionResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Projection = new ProjectionDomainModel
                {
                    Id = insertedProjection.Id,
                    AuditoriumId = insertedProjection.AuditoriumId,
                    MovieId = insertedProjection.MovieId,
                    ProjectionTime = insertedProjection.ShowingDate,
                    Duration = insertedProjection.Duration,
                    Price = insertedProjection.Price
                }
            };

            return result;
        }
    }
}

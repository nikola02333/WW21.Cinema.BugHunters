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
        private readonly ITicketsRepository _ticketsRepository;

        public ProjectionService(IProjectionsRepository projectionsRepository, 
                                ICinemasRepository cinemasRepository, 
                                IAuditoriumsRepository auditoruimsRepository,
                               IMoviesRepository moviesRepository,
                               ITicketsRepository ticketsRepository)
        {
            _projectionsRepository = projectionsRepository;
            _cinemasRepository = cinemasRepository;
            _auditoruimsRepository = auditoruimsRepository;
            _moviesRepository = moviesRepository;
            _ticketsRepository = ticketsRepository;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetAllAsync()
        {
            var data = await _projectionsRepository.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            var result = data.Select(item => new ProjectionDomainModel
            {
                Id = item.Id,
                MovieId = item.MovieId,
                AuditoriumId = item.AuditoriumId,
                ProjectionTime = item.ShowingDate,
                MovieTitle = item.Movie.Title,
                AuditoriumName = item.Auditorium.AuditoriumName,
                Duration = item.Duration,
                Price = item.Price,
                CinemaId= item.Auditorium.CinemaId,
                CinemaName=item.Auditorium.Cinema.Name
            }).ToList();

            return result;
        }
        

        public async Task<GenericResult<ProjectionDomainModel>> FilterProjectionAsync(FilterProjectionDomainModel filter)
        {
            Data.Cinema cinema=null;
            Auditorium auditorium = null;

            //If CinemaId is used for filtering we are checking if CinemaId exists
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
            }

            //If AuditoriumId is used for filtering we are checking if AuditoriumId exists,
            // and after that we are checking if CinemaId is also used for filtering after which 
            // we check if AuditoriumId is in cinema with CinemaID
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
            }

            //If MovieId is used for filtering we are checking if MovieId exists,
            // and after that we are checking if AuditoriumId is also used for filtering after which 
            // we check if MovieId is projecting in auditorium with AuditoriumId
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
            }

            if (filter.DateTime != null && filter.DateTime < DateTime.Now)
            {
                return new GenericResult<ProjectionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_IN_PAST
                };
            }

            var projections = await _projectionsRepository.FilterProjectionAsync(filter.CinemaId, filter.AuditoriumId, filter.MovieId, filter.DateTime);

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
                    Price = item.Price,
                    AuditoriumName = item.Auditorium.AuditoriumName,
                    MovieTitle = item.Movie.Title,
                     CinemaId = item.Auditorium.CinemaId,
                    CinemaName = item.Auditorium.Cinema.Name
                }).ToList()
            };

        }

        public async Task<GenericResult<ProjectionDomainModel>> CreateProjection(ProjectionDomainModel domainModel)
        {
            //int projectionTime = 3;

            var projectionsAtSameTime = _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId)
                .Where(x => x.ShowingDate < domainModel.ProjectionTime.AddMinutes(domainModel.Duration) && x.ShowingDate > domainModel.ProjectionTime.AddMinutes(-domainModel.Duration))
                .ToList();

            if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0)
            {
                return new GenericResult<ProjectionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTIONS_AT_SAME_TIME
                };
            }

            var newProjection = new Projection
            {
                MovieId = domainModel.MovieId,
                AuditoriumId = domainModel.AuditoriumId,
                 ShowingDate = domainModel.ProjectionTime,
                 Duration = domainModel.Duration,
                 Price = domainModel.Price,
                 
            };

            var insertedProjection = await _projectionsRepository.InsertAsync(newProjection);

            if (insertedProjection == null)
            {
                return new GenericResult<ProjectionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_CREATION_ERROR
                };
            }
            _projectionsRepository.Save();

            var result = new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                Data = new ProjectionDomainModel
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

        public async Task<GenericResult<ProjectionDomainModel>> GetByIdAsync(Guid id)
        {
            var projection =await _projectionsRepository.GetByIdAsync(id);

            if (projection==null)
            {
                return new GenericResult<ProjectionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_GET_BY_ID
                };
            }

            return new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                Data = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.ShowingDate,
                    Duration = projection.Duration,
                    Price = projection.Price,
                    AuditoriumName = projection.Auditorium.AuditoriumName,
                    MovieTitle = projection.Movie.Title,
                    CinemaId = projection.Auditorium.CinemaId,
                    CinemaName = projection.Auditorium.Cinema.Name
                }
            };
        }

        public async Task<GenericResult<ProjectionDomainModel>> DeleteProjectionAsync(Guid id)
        {
            var projection = await _projectionsRepository.GetByIdAsync(id);

            if (projection==null)
            {
                return new GenericResult<ProjectionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_GET_BY_ID
                };
            }
            var tickets = await _ticketsRepository.GetByProjectionId(id);

            if (projection.ShowingDate.AddMinutes(projection.Duration) < DateTime.Now)
            {
               
                if (tickets != null)
                {
                    foreach (var ticket in tickets)
                    {
                       var dilitedTicket = _ticketsRepository.Delete(ticket.Id);
                        if (dilitedTicket == null)
                        {
                            return new GenericResult<ProjectionDomainModel>
                            {
                                IsSuccessful = false,
                                ErrorMessage = Messages.TICKET_DELTE_ERROR
                            };
                        }
                        _ticketsRepository.Save();
                    }     
                }

                

                var deletedProjection = _projectionsRepository.Delete(id);
                if (deletedProjection == null)
                {
                    return new GenericResult<ProjectionDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.PROJECTION_DELTE_ERROR
                    };
                }
                _projectionsRepository.Save();
            }
            else if(tickets==null || tickets.Count() == 0)
                {
                    var deletedProjection = _projectionsRepository.Delete(id);
                    if (deletedProjection == null)
                    {
                        return new GenericResult<ProjectionDomainModel>
                        {
                            IsSuccessful = false,
                            ErrorMessage = Messages.PROJECTION_DELTE_ERROR
                        };
                    }
                    _projectionsRepository.Save();
                } else
                    {
                        return new GenericResult<ProjectionDomainModel>
                        {
                            IsSuccessful = false,
                            ErrorMessage = Messages.PROJECTION_DELTE_IN_PAST
                        };
                    }
               

            
            

            return new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true
            };

        }

        public async Task<GenericResult<ProjectionDomainModel>> UpdateProjection(ProjectionDomainModel updateProjection)
        {
            var projection = await _projectionsRepository.GetByIdAsync(updateProjection.Id);

            if (projection == null)
            {
                return new GenericResult<ProjectionDomainModel>
                {
                    ErrorMessage = Messages.PROJECTION_GET_BY_ID,
                    IsSuccessful = false
                };
            }

            Projection newProjection = new Projection
            {

                Id = updateProjection.Id,
                AuditoriumId = updateProjection.AuditoriumId,
                MovieId = updateProjection.MovieId,
                ShowingDate = updateProjection.ProjectionTime,
                Duration = updateProjection.Duration,
                Price = updateProjection.Price,
            };
            
            _projectionsRepository.Update(newProjection);
            _projectionsRepository.Save();

            return new GenericResult<ProjectionDomainModel>
            {
                IsSuccessful = true,
                Data = new ProjectionDomainModel
                {
                    Id = newProjection.Id,
                    AuditoriumId = newProjection.AuditoriumId,
                    MovieId = newProjection.MovieId,
                    ProjectionTime = newProjection.ShowingDate,
                    Duration = newProjection.Duration,
                    Price = newProjection.Price
                }
            };

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IProjectionService
    {
        Task<IEnumerable<ProjectionDomainModel>> GetAllAsync(bool currant);

        Task<GenericResult<ProjectionDomainModel>> GetByIdAsync(Guid id);
        Task<GenericResult<ProjectionDomainModel>> CreateProjection(ProjectionDomainModel domainModel);

        Task<GenericResult<ProjectionDomainModel>> FilterProjectionAsync(FilterProjectionDomainModel filter);

        Task<GenericResult<ProjectionDomainModel>> DeleteProjectionAsync(Guid id);
        Task<GenericResult<ProjectionDomainModel>>  UpdateProjection(ProjectionDomainModel updateProjection);
    }
}

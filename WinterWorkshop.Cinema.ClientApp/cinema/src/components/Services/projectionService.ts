import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {IFilterProjectionModel,ICreateProjection} from '../../models/ProjectionModels';
import API from '../../axios';

export const projectionService = {
    getAllProjections,
    getProjectionById,
    getFilteredProjection,
    createProjection,
    deleteProjectino,
    updateProjection
};

async function getAllProjections(currant=false)
{
  return await API.get(`${serviceConfig.baseURL}/api/Projections/all?currant=${currant}`)
                        .then( response => {
                          return response.data;
                        })
                        .catch(err => {
                          if (err.response) {
                            NotificationManager.error(err.response.data.errorMessage);
                          } else if (err.request) {
                           // NotificationManager.error("Server Error");
                          } else {
                            NotificationManager.error("Error");
                          }
                      });
}

async function getProjectionById(id : string)
{
  return await API.get(`${serviceConfig.baseURL}/api/Projections/byprojectionid/${id}`)
                        .then( response => {
                          return response.data;
                        })
                        .catch(err => {
                          if (err.response) {
                            NotificationManager.error(err.response.data.errorMessage);
                          } else if (err.request) {
                            //NotificationManager.error("Server Error");
                          } else {
                            NotificationManager.error("Error");
                          }
                      });                
}

async function getFilteredProjection(filter : IFilterProjectionModel)
{
    let query = "";
    if (filter.CinemaId!==0 && filter.CinemaId!==undefined) {
      query = `cinemaId=${filter.CinemaId}`;
    }
    if (filter.AuditoriumId!==0 && filter.CinemaId!==undefined) {
      query += `${query.length ? "&" : ""}auditoriumId=${filter.AuditoriumId}`;
    }
    if (filter.MovieId!=="" && filter.CinemaId!==undefined) {
      query += `${query.length ? "&" : ""}movieId=${filter.MovieId}`;
    }
    if (filter.DateTime && filter.DateTime.getFullYear()!==1970 ) {
      query += `${query.length ? "&" : ""}dateTime=${filter.DateTime.toISOString()}`;
    }
    if (query.length) {
      query = `?${query}`;
    }
  return await API.get(`${serviceConfig.baseURL}/api/Projections/filter/${query}`)
                        .then( response => {
                          return response.data;
                        })
                        .catch(err => {
                          if (err.response) {
                            NotificationManager.error(err.response.data.errorMessage);
                          } else if (err.request) {
                           // NotificationManager.error("Server Error");
                          } else {
                            NotificationManager.error("Error");
                          }
                      });
}

async function createProjection(projection: ICreateProjection)
{
  return await API.post(`${serviceConfig.baseURL}/api/Projections`,projection)
                        .then( response=> {
                          return response.data;
                        })
                        .catch(err => {
                          if (err.response) {
                            NotificationManager.error(err.response.data.errorMessage);
                          } else if (err.request) {
                            //NotificationManager.error("Server Error");
                          } else {
                            NotificationManager.error("Error");
                          }
                      });
}
async function deleteProjectino(id: string)
{
    if (window.confirm('Are you sure you wish to delete this projection?'))
    {
        return await API.delete(`${serviceConfig.baseURL}/api/Projections/Delete/${id}`)
                        .then( response=> {
                          return response.data;
                        })
                        .catch(err => {
                          if (err.response) {
                            NotificationManager.error(err.response.data.errorMessage);
                          } else if (err.request) {
                           // NotificationManager.error("Server Error");
                          } else {
                            NotificationManager.error("Error");
                          }
                      });
    }
}

async function updateProjection(id: string, projectionToUpdate : ICreateProjection)
{
 return await API.put(`/api/Projections/Update/${id}`, projectionToUpdate)
                              .then( (res)=> {
                                NotificationManager.success("Projection updated successfuly");
                                return res.data;
                              })
                              .catch(err => {
                                if (err.response) {
                                  NotificationManager.error(err.response.data.errorMessage);
                                } else if (err.request) {
                                 // NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                            });
}
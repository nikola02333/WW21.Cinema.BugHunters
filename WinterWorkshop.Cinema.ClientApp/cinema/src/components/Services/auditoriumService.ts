import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {ICreateAuditorium} from '../../models/AuditoriumModels';
import API from '../../axios';


export const auditoriumService = {
    getAllAuditoriums,
    getAuditoriumById,
    getAuditoriumByCinemaId,
    deleteAuditorium,
    createAuditorium,
    updateAuditorium
};

async function getAllAuditoriums()
{
  return await API.get(`${serviceConfig.baseURL}/api/Auditoriums/all`)
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

async function getAuditoriumById(id:string)
{
  return await API.get(`${serviceConfig.baseURL}/api/Auditoriums/GetById/${id}`)
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

async function getAuditoriumByCinemaId(id:number)
{
  return await API.get(`${serviceConfig.baseURL}/api/Auditoriums/ByCinemaId/${id}`)
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

async function createAuditorium(auditorium: ICreateAuditorium)
{
  return  await API.post(`${serviceConfig.baseURL}/api/Auditoriums/Create`,auditorium)
                        .then( response=> {
                          return response.data;
                        })
                        .catch(err => {
                          if (err.response) {
                            NotificationManager.error(err.response.data.errorMessage);
                          } else if (err.request) {
                          //  NotificationManager.error("Server Error");
                          } else {
                            NotificationManager.error("Error");
                          }
                      });
}
async function updateAuditorium(auditoriumId: string, auditoriumToUpdate : ICreateAuditorium)
{
 return await API.put(`${serviceConfig.baseURL}/api/auditoriums/update/${auditoriumId}`, auditoriumToUpdate)
                              .then( (res)=> {
                                NotificationManager.success("Auditorium updated successfuly");
                                return res.data;
                              })
                              .catch(error => {
                                if (error.response) {
                                  NotificationManager.error(error.response.data.errorMessage);
                                } else if (error.request) {
                                //  NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
}
async function deleteAuditorium(id: string)
{
    if (window.confirm('Are you sure you wish to delete this auditorium?'))
    {
        return await API.delete(`${serviceConfig.baseURL}/api/Auditoriums/Delete/${id}`)
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
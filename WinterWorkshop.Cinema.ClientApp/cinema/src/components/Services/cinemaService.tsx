import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {ICinemaToCreateModel} from '../../models/ICinemaToCreateModel';
import {ICinemaToUpdateModel} from '../../models/ICinemaToUpdateModel';

import API from '../../axios';
import { ICinema } from "../../models";

export const cinemaService = {
    addCinema,
    getCinemas,
    removeCinema,
    getCinemaById,
    updateCinema,  
};

async function updateCinema(cinemaId: string, cinemaToUpdate : ICinemaToUpdateModel)
{
 return await API.put(`/api/cinemas/${cinemaId}`, cinemaToUpdate)
                              .then( (res)=> {
                                NotificationManager.success("Cinema updated successfuly");
                                return res.data;
                              })
                              .catch(error => {
                                if (error.response) {
                                  NotificationManager.error(error.response.data.errorMessage);
                                } else if (error.request) {
                                  NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
}

async function  getCinemaById(cinemaId: string)
{
 return await API.get(`/api/cinemas/GetById/${cinemaId}`)
                              .then( (res)=> {
                                return res.data;
                              })
                              .catch(error => {
                                if (error.response) {
                                  NotificationManager.error(error.response.data.errorMessage);
                                } else if (error.request) {
                                  NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
}

function removeCinema(id: string)
{
  if (window.confirm('Are you sure you wish to delete this item?'))
    {
      return  API.delete(`/api/cinemas/Delete/${id}`)
      .then( (response)=> {
        NotificationManager.success("Successfuly removed cinema!");     
        return id;
      })
      .catch(error => {
        if (error.response) {
          NotificationManager.error(error.response.data.errorMessage);
        } else if (error.request) {
          NotificationManager.error("Server Error");
        } else {
          NotificationManager.error("Error");
        }
        });
    }
}

async function getCinemas()
{
    return await API.get(`${serviceConfig.baseURL}/api/cinemas/all`)
                          .then( response=> {
                            return response.data;
                          })
                          .catch(error => {                       
                            if (error.response) {
                              NotificationManager.error(error.response.data.errorMessage);
                            } else if (error.request) {
                              NotificationManager.error("Server Error");
                            } else {
                              NotificationManager.error("Error");
                            }
                            });
      
  }
 
 function addCinema(cinematoCreateModel: ICinemaToCreateModel) 
{
  return API.post(`${serviceConfig.baseURL}/api/cinemas/Create`, cinematoCreateModel)
        .then( response=> {
          return response.data;
        })
        .catch(error => {
          if (error.response) {
            NotificationManager.error(error.response.data.errorMessage);
          } else if (error.request) {
            NotificationManager.error("Server Error");
          } else {
            NotificationManager.error("Error");
          }
          });    
}
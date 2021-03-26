import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {ICinemaToCreateModel} from '../../models/ICinemaToCreateModel';
import {ICinemaToUpdateModel} from '../../models/ICinemaToUpdateModel';

import API from '../../axios';

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
                                NotificationManager.error(error.response.data.errorMessage);
                                });
}

async function  getCinemaById(cinemaId: string)
{
 return await API.get(`/api/cinemas/GetById/${cinemaId}`)
                              .then( (res)=> {
                                return res.data;
                              })
                              .catch(error => {
                                NotificationManager.error(error.response.data.errorMessage);
                                });
}

function removeCinema(id: string)
{
  if (window.confirm('Are you sure you wish to delete this item?'))
    {
      return  API.delete(`/api/cinemas/Delete/${id}`)
      .then( (response)=> {

        console.log(response.data);
        NotificationManager.success("Successfuly removed cinema!");     
        return id;
      })
      .catch(error => {
        NotificationManager.error(error.response.data.errorMessage);
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
                           
                            NotificationManager.error(error.response.data.errorMessage || "Server error");
                            });
      
  }
 
async function addCinema(cinematoCreateModel: ICinemaToCreateModel) 
{
  console.log(cinematoCreateModel);
    await API.post(`${serviceConfig.baseURL}/api/cinemas/create`, JSON.stringify(cinematoCreateModel))
        .then( response=> {
             NotificationManager.success("Successfuly added cinema!");
        })
        .catch(error => {
          debugger
          NotificationManager.error(error.response.data.errorMessage);
          });    
}
import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";

import API from '../../axios';

export const imdbService = {
    searchImdb,
    searchImdbWitVideo,
    getTopTenMovies
};

async function getTopTenMovies(){
  return await API.get(`${serviceConfig.baseURL}/api/imdbs/GetTopTenMovies/`)
          .then( response=>{
            return response.data;
          })
          .catch(err => {
            if (err.response) {
              NotificationManager.error(err.response.data.errorMessage);
            } else if (err.request) {
              NotificationManager.error("Server Error");
            }
        });
            
}
async function searchImdb(id: string)
{
  return await API.get(`${serviceConfig.baseURL}/api/imdbs/Search/${id}`)
                    .then( (response)=> {
                        return response.data;
                    })
                    .catch(err => {
                        if (err.response) {
                          NotificationManager.error(err.response.data.errorMessage);
                        } else if (err.request) {
                          NotificationManager.error("Server Error");
                        }
                    });
}
async function searchImdbWitVideo(id: string)
{
  return await API.get(`${serviceConfig.baseURL}/api/imdbs/Search/video/${id}`)
                    .then( (response)=> {
                        return response.data;
                    })
                    .catch(err => {
                        if (err.response) {
                          NotificationManager.error(err.response.data.errorMessage);
                        } else if (err.request) {
                          NotificationManager.error("Server Error");
                        }
                    });
}
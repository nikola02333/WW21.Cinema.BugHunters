import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";

import API from '../../axios';

export const imdbService = {
    searchImdb
};

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
import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {IMovieToUpdateModel} from '../../models/IMovieToUpdateModel';
import { IMovieToCreateModel } from './../../models/IMovieToCreateModel';

import API from '../../axios';

export const movieService = {
    createMovie,
    searcMovie,
    getAllMovies,
    getCurrentMovies,
    removeMovie,
    searchMovieById,
    updateMovie,
    changeCurrent,
    getTopTen
   
};
async function getTopTen()
{
  //TopTenMovies
  return await API.get(`${serviceConfig.baseURL}/api/movies/TopTenMovies`)
                        .then( response => {
                          return response.data;
                        })
                        .catch((error) => {
      
                          NotificationManager.error(error.response);
                        });
                         
}
async function changeCurrent(movieId: string)
{
  return await API.post(`${serviceConfig.baseURL}/api/movies/ActivateMovie/${movieId}`)
                        .then( response=> {
                          return response.data;
                        })
                        .catch(error => {
                          NotificationManager.error(error.response.data.errorMessage);
                          });

}
async function updateMovie(movieId: string, movieToUpdate : IMovieToUpdateModel)
{
 return await API.put(`/api/movies/Update/${movieId}`, movieToUpdate)
                              .then( (res)=> {
                                NotificationManager.success("Movie updated successfuly");
                                return res.data;
                              })
                              .catch(error => {
                                NotificationManager.error(error.response.data.errorMessage);
                                });
}
async function searchMovieById(movieId: string)
{
  var movie = await API.get(`/api/movies/GetById/${movieId}`)
                        .then( (response)=> {
                          return response.data;
                        })
                        .catch(error => {
                          NotificationManager.error(error.response.data.errorMessage);
                          });
    
    return movie;
}
function removeMovie(id: string)
{
  if (window.confirm('Are you sure you wish to delete this item?'))
    {
      return  API.delete(`/api/movies/Delete/${id}`)
      .then( (response)=> {

        console.log(response.data);

        NotificationManager.success("Successfuly removed movie!");
       
        return id;
      })
      .catch(error => {
        NotificationManager.error(error.response.data.errorMessage);
        });

    }
}
async function getCurrentMovies()
{
  return await API.get(`${serviceConfig.baseURL}/api/movies/AllMovies/true`)
                  .then( response=> {
                    return response.data;
                  })
                  .catch(error => {
                    NotificationManager.error(error.response.data.errorMessage);
                    });
    
}
async function getAllMovies()
{
    return await API.get(`${serviceConfig.baseURL}/api/movies/AllMovies/false`)
                          .then( response=> {
                            return response.data;
                          })
                          .catch(error => {
                            NotificationManager.error(error.response.data.errorMessage);
                            });
      
  }
  
 async function searcMovie(tagToSearch: any): Promise<any>
{
  
  return   await API.get(`/api/movies/SearchMoviesByTag?query=${tagToSearch}`,{timeout: 50000})
      .then( (response)=> {

       return response.data;
      })
      .catch(error => {
      NotificationManager.error(error.response);
      });   
}

async function createMovie(moveModel: IMovieToCreateModel) 
  {
  
      await API.post(`${serviceConfig.baseURL}/api/movies`, JSON.stringify(moveModel))
          .then( response=> {
               NotificationManager.success("Successfuly added movie!");
          })
          .catch(error => {
            NotificationManager.error(error.response.data.errorMessage);
            });
        
}

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
  var movies = await API.get(`${serviceConfig.baseURL}/api/movies/TopTenMovies`)
                        .then( response => {
                          return response.data;
                        })
                        .catch(error => {
                          NotificationManager.error(error.response.data.errorMessage);
                          });
                          return movies;
}
async function changeCurrent(movieId: string)
{
  var result = await API.post(`${serviceConfig.baseURL}/api/movies/ActivateMovie/${movieId}`)
                        .then( response=> {
                          return response.data;
                        })
                        .catch(error => {
                          NotificationManager.error(error.response.data.errorMessage);
                          });
      return result;

}
async function updateMovie(movieId: string, movieToUpdate : IMovieToUpdateModel)
{
  var movieUpdated = await API.put(`/api/movies/Update/${movieId}`, movieToUpdate)
                              .then( (res)=> {
                                NotificationManager.success("Movie updated successfuly");
                                return res.data;
                              })
                              .catch(error => {
                                NotificationManager.error(error.response.data.errorMessage);
                                });
            return movieToUpdate;
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
      .catch( (resp)=> {
        console.log(resp);
        return undefined;
      });

    }
}
function getCurrentMovies()
{
    const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        }
      };
  
      fetch(`${serviceConfig.baseURL}/api/movies/AllMovies`, requestOptions)
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response;
        })
        .then((response) => {
              return response.json();
            })
        .then((result) => {
          NotificationManager.success("Successfuly added movie!");
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
        });
}
async function getAllMovies()
{
    let movies= await API.get(`${serviceConfig.baseURL}/api/movies/AllMovies/false`)
                          .then( response=> {
                            return response.data;
                          })
                          .catch((response) => {
                            NotificationManager.error(response.message || response.statusText);
                          });
  return movies;
      
  }
  
 async function searcMovie(tagToSearch: any): Promise<any>
{
  var result =
     await API.get(`/api/movies/SearchMoviesByTag?query=${tagToSearch}`,{timeout: 50000})
      .then( (response)=> {

       return response.data;
      })
      .catch(error => {
      NotificationManager.error(error.response.data.errorMessage);
      });   
      return result; 
}
function createMovie(moveModel: IMovieToCreateModel) : any
{
  const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(moveModel),
    };

    fetch(`${serviceConfig.baseURL}/api/movies`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response;
      })
      .then((response) => {
            return response.json();
          })
      .then((result) => {
        NotificationManager.success("Successfuly added movie!");
        //props.history.push(`AllMovies`);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        //setState({ ...state, submitted: false });
      });
}

import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {IUserToCreateModel} from '../../models/IUserToCreateModel';
import * as authChech from "../helpers/authCheck";
  

import API from '../../axios';
import { IUser } from './../../models/index';
import {IUserUpdate} from './../../models/IUserUpdate';

export const userService = {
    login,
    singUp,
    getUserByUsername,
    getTokenForGuest,
    editUser,
    
   
};

async function editUser(userToUpdate: IUserUpdate) :Promise<IUser>
{
  return await API.put(`${serviceConfig.baseURL}/api/users/Update/${userToUpdate.id}`, userToUpdate)
            .then( (response)=> {
              
              NotificationManager.success(`User, succesfuly  updated!`);
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
 async function getTokenForGuest()
 {
   return await API.get(`${serviceConfig.baseURL}/get-token/`)
                    .then( response => {
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
async function getUserByUsername(userName: string) : Promise<IUser>
{
  
 return await API.get(`${serviceConfig.baseURL}/api/users/byusername/${userName}`)
    .then((response) => {
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

async function  singUp (userToCreate: IUserToCreateModel) :  Promise<any>
{
 return await API.post(`${serviceConfig.baseURL}/api/users/Create`, JSON.stringify(userToCreate))
    .then((response) => {
      return response.data;
    })
    .then((data) => {
      
        NotificationManager.success(`User, ${data.firstName} succesfuly register!`);
        return data;
    })
    .catch(err => {
      if (err.response) {
        NotificationManager.error(err.response.data.errorMessage);
      } else if (err.request) {
        NotificationManager.error("Server Error");
      }
  });

}

function login (userName:string)  {

  API.post( `${serviceConfig.baseURL}/get-token`,{'UserName' :userName})
          .then( response=> {
           return response.data
          })
          .then( data =>{
            if (data.token) {
              localStorage.setItem("jwt", data.token);
              localStorage.setItem("userLoggedIn", "true");
              NotificationManager.success(`Welcome, ${data.firstName}!`);
              
              setTimeout(() => {
                window.location.reload();
              }, 500);
            }
          })
          .catch(err => {
            if (err.response) {
              NotificationManager.error(err.response.data.errorMessage);
            } else if (err.request) {
              NotificationManager.error("Server Error");
            }
        });
};

export const getUserByUsernameReservatino = (setState) => {
  let ussName = authChech.getUserName();

  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("jwt")}`,
    },
  };

  fetch(
    `${serviceConfig.baseURL}/api/users/byusername/${ussName}`,
    requestOptions
  )
    .then((response) => {
      if (!response.ok) {
        return;
      }
      return response.json();
    })
    .then((data) => {
      if (data) {
        setState((prev)=>({ ...prev, userId: data.id }));
      }
    })
    .catch(err => {
      if (err.response) {
        NotificationManager.error(err.response.data.errorMessage);
      } else if (err.request) {
        NotificationManager.error("Server Error");
      
      }
  });
};

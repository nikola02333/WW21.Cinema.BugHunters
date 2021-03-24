import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {IUserToCreateModel} from '../../models/IUserToCreateModel';
import * as authChech from "../helpers/authCheck";
  

import API from '../../axios';
import { IUser } from './../../models/index';

export const userService = {
    login,
    singUp,
    getUserByUsername,
    getTokenForGuest
   
};
 async function getTokenForGuest()
 {
   return await API.get(`${serviceConfig.baseURL}/get-token/`)
                    .then( response => {
                      return response.data;
                    })
                    .catch((response) => {
                      NotificationManager.error(response.message || response.statusText);
                    });

 } 
async function getUserByUsername(userName: string) : Promise<IUser>
{
  
 return await API.get(`${serviceConfig.baseURL}/api/users/byusername/${userName}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      NotificationManager.error(error.response.data.errorMessage || error.response.data.statusText);
    });
}

async function  singUp (userToCreate: IUserToCreateModel) :  Promise<any>
{
  console.log(JSON.stringify(userToCreate))

 var data =await API.post(`${serviceConfig.baseURL}/api/users/Create`, JSON.stringify(userToCreate))
    .then((response) => {
      return response.data;
    })
    .then((data) => {
      
      console.log(data)

        NotificationManager.success(`User, ${data.firstName} succesfuly register!`);
        return data;
    })
    .catch((error) => {
      
      NotificationManager.error(error.response.data.errorMessage);
    });

    return data;
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
          .catch((error) => {
            NotificationManager.error(error.response.data.errorMessage);
          });
};

export const getUserByUsername = (setState) => {
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
        console.log("getUserByUsername");
        console.log(data);
      }
    })
    .catch((response) => {
      NotificationManager.error(response.message || response.statusText);
    });
};

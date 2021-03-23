import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {IUserToCreateModel} from '../../models/IUserToCreateModel';
import * as authChech from "../helpers/authCheck";
  
 
export const userService = {
    login,
    singUp,
    getUserName
   
};
function getUserName(userName: string) : any
{
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("jwt")}`,
    },
  };

  fetch(
    `${serviceConfig.baseURL}/api/users/byusername/${userName}`,
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
        //setState({ ...state, user: data });

       // getReservationsByUserId(state.user.id);
       // jedino ovde return data
       
       return data;
      }
    })
    .catch((response) => {
      NotificationManager.error(response.message || response.statusText);
      //setState({ ...state, submitted: false });
    });
}

async function  singUp (userToCreate: IUserToCreateModel) :  Promise<any>
{
  console.log(JSON.stringify(userToCreate))

  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(userToCreate)
  };

 var data = await fetch(
      
    `${serviceConfig.baseURL}/api/users/Create`,
    requestOptions
  )

    .then((response) => {

      return response.json();
    })
   
    .then((data) => {
    
      if(data.statusCode === 400)
      {
        // ovde je bug, ispisuje  gresku i success !!
        NotificationManager.error(`${data.errorMessage}`);
      }
      
     return data;
    })
    .then((data) => {
      
      console.log(data)

        NotificationManager.success(`User, ${data.firstName} succesfuly register!`);
        return data;
    })
    .catch((response) => {

      //  kako da ovde uzmem data , a ne response
      
      NotificationManager.error(response.message || response.statusText);
    });

    return data;
}

function login (userName:string)  {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({'UserName' :userName})
  };

  fetch(
      //
    `${serviceConfig.baseURL}/get-token`,
    requestOptions
  )
    .then((response) => {
      if (!response.ok) {
        return Promise.reject(response);
      }
      return response.json();
    })
   
    .then((data) => {
        
      if (data.token) {
        localStorage.setItem("jwt", data.token);
        localStorage.setItem("userLoggedIn", "true");
        NotificationManager.success(`Welcome, ${data.firstName}!`);
        
        setTimeout(() => {
          window.location.reload();
        }, 500);
      }
    })
    .catch((response) => {
      NotificationManager.error(response.message || response.statusText);
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

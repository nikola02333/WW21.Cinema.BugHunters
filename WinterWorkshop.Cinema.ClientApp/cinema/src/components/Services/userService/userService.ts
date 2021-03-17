import { serviceConfig } from "../../../appSettings"
import { NotificationManager } from "react-notifications";

export const userService = {
    login,
   
};
function login() {
    const requestOptions = {
        method: "GET",
  
        headers:{ 
          "Content-Type": "application/json",
        },
         body: JSON.stringify({"UserName ": "nikola"} )
      };
      fetch(
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
            setTimeout(() => {
              window.location.reload();
            }, 500);
          }
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          //setState({ ...state, submitted: false });
        });
        
}

import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {Get} from "../helpers/RequestOptions"

export const getReservedSeats = (id: string, setState) => {
    fetch(
     `${serviceConfig.baseURL}/api/seats/reservedByProjectionId/${id}`,
     Get
   )
     .then((response) => {
       if (!response.ok) {
         return Promise.reject(response);
       }
       return response.json();
     })
     .then((data) => {
       if (data) {
         setState((prev)=>({
           ...prev,
           reservedSeats: data,
         }));
         console.log("getReservedSeats");
         console.log(data);
       }
     })
     .catch((response) => {
       NotificationManager.error(response.message || response.statusText);
    //    setState((prev)=>({ ...prev, submitted: false }));
     });
 };

 export const getSeatsForAuditorium = (auditId: string,setState) => {
    fetch(
      `${serviceConfig.baseURL}/api/seats/maxNumberOfSeatsByAuditoriumId/${auditId}`,
      Get
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState((prev)=>({
            ...prev,
            seats: data,
            maxRow: data.maxRow,
            maxNumberOfRow: data.maxNumber,
          }));
          console.log("getSeatsForAuditorium");
          console.log(data);
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        // setState((prev)=>({ ...prev, submitted: false }));
      });
  };

  export const getSeats = (auditId: string, setState) => {
    fetch(
      `${serviceConfig.baseURL}/api/seats/byauditoriumid/${auditId}`,
      Get
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState((prev)=>({
            ...prev,
            seats: data
          }));
          console.log("getSeats");
          console.log(data);
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        // setState((prev)=>({ ...prev, submitted: false }));
      });
  };
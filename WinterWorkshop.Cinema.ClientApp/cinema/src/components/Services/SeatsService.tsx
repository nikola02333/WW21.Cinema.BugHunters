import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {Get} from "../helpers/RequestOptions"

export async function  getReservedSeats (id: string, setState) {
    await fetch(
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
         
       }
     })
     .catch((response) => {
       NotificationManager.error(response.message || response.statusText);
    //    setState((prev)=>({ ...prev, submitted: false }));
     });
 };

 export async function getSeatsForAuditorium (auditId: string,setState)  {
   await fetch(
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
            maxRow: data.maxRow,
            maxNumberOfRow: data.maxNumber,
          }));
          console.log("getSeatsForAuditorium");
          
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        // setState((prev)=>({ ...prev, submitted: false }));
      });
  };

  export async function getSeats (auditId: string, setState) {
    await fetch(
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
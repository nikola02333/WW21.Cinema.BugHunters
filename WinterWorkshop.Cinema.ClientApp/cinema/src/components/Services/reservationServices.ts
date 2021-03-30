import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import {IUserToCreateModel} from '../../models/IUserToCreateModel';
import * as authChech from "../helpers/authCheck";
  

import API from '../../axios';

export const reservationService = {
    tryReservationn
};

async function tryReservationn(e: React.MouseEvent<HTMLButtonElement, MouseEvent>, seat,info)
{
 return await API.post(`${serviceConfig.baseURL}/api/levi9payment`)
                              .then( (res)=> {
                                makeReservationn(e,seat,info);
                              })
                              .catch(error => {
                                if (error.response) {
                                    NotificationManager.warning("Insufficient founds.");
                                } else if (error.request) {
                                  NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
}

async function makeReservationn(e: React.MouseEvent<HTMLButtonElement, MouseEvent>, seat,info)
{
    e.preventDefault();

    if (
        authChech.getRole() === "user" ||
        authChech.getRole() === "superUser" ||
        authChech.getRole() === "admin"
    ){

    var idFromUrl = window.location.pathname.split("/");
      var projectionId = idFromUrl[3];

      const { currentReservationSeats } = seat;

      const data = {
        projectionId: projectionId,
        seatId: currentReservationSeats.map((id)=>{
          return id['currentSeatId'];
        }),
        userId: info.userId,
      };

    
    return await API.post(`${serviceConfig.baseURL}/api/ticket/create`,data)
                              .then( (res)=> {
                                currentReservationSeats.forEach(x => {
                                    addPoints(info.userId);
                                });
                                NotificationManager.success(
                                    "Your reservation has been made successfully!"
                                  );
                                  setTimeout(() => {
                                    window.location.reload();
                                  }, 2000);
                              })
                              .catch(error => {
                                if (error.response) {
                                 NotificationManager.error(error.response.data.errorMessage);
                                } else if (error.request) {
                                  NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
    }else{
        NotificationManager.error("Please log in to make reservation.");
    }
}

async function addPoints(id)
{
    return await API.post(`${serviceConfig.baseURL}/api/Users/IncrementPoints/${id}`)
                              .then( (res)=> {
                                NotificationManager.success(
                                    "Your got point"
                                  );
                                  setTimeout(() => {
                                    window.location.reload();
                                  }, 2000);
                              })
                              .catch(error => {
                                if (error.response) {
                                    NotificationManager.error(error.response.data.errorMessage);
                                } else if (error.request) {
                                  NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
    
}


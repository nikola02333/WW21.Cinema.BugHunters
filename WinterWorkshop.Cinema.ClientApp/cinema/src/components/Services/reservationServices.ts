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
  if (
    authChech.getRole() === "user" ||
    authChech.getRole() === "superUser" ||
    authChech.getRole() === "admin"
  ){
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
  }else{
        NotificationManager.error("Please log in to make reservation.");
        }
}
interface ICreateSeats{
  projectionId:string;
  seatId:string[];
  userId:number;
}

async function makeReservationn(e: React.MouseEvent<HTMLButtonElement, MouseEvent>, seat,info)
{
    e.preventDefault();
    var idFromUrl = window.location.pathname.split("/");
    var projectionId = idFromUrl[3];

    const { currentReservationSeats } = seat;

    const data : ICreateSeats = {
        projectionId: projectionId,
        seatId: currentReservationSeats.map((id)=>{return id['currentSeatId'];}) ,
        userId: info.userId,
    };
    var points = data.seatId.length;

    return await API.post(`${serviceConfig.baseURL}/api/ticket/create`,data)
                              .then( (res)=> {
                                var result = addPoints(info.userId,points);
                                if(currentReservationSeats.lenght!==0){
                                    NotificationManager.success(
                                      points===1?"You got "+points+" bonus point":"You got "+points+" bonus points"
                                      );
                                }
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
                                  //NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
    
}

async function addPoints(id:string, number:number)
{
    return await API.post(`${serviceConfig.baseURL}/api/Users/IncrementPoints/${id}/${number}`)
                              .then( (res)=> {
                                return 1;
                              })
                              .catch(error => {
                                if (error.response) {
                                    NotificationManager.error(error.response.data.errorMessage);
                                } else if (error.request) {
                               //   NotificationManager.error("Server Error");
                                } else {
                                  NotificationManager.error("Error");
                                }
                                });
    
}


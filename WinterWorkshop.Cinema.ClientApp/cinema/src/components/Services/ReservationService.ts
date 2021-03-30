import { serviceConfig } from "../../appSettings"
import { NotificationManager } from "react-notifications";
import * as authChech from "../helpers/authCheck";

export const tryReservation = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>,seat,info) => {
    e.preventDefault();
    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: "",
    };

    fetch(`${serviceConfig.baseURL}/api/levi9payment`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        makeReservation(e,seat,info);
      })
      // then samo ovde jedan gde cu zvati IncrementPoints/{userId}
      .catch((response) => {
        NotificationManager.warning("Insufficient founds.");
        // setState((prev)=>({ ...prev, submitted: false }));
      });
  };

  const makeReservation = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>, seats,info
  ) => {
    e.preventDefault();

    if (
        authChech.getRole() === "user" ||
        authChech.getRole() === "superUser" ||
        authChech.getRole() === "admin"
    ) {
      var idFromUrl = window.location.pathname.split("/");
      var projectionId = idFromUrl[3];

      const { currentReservationSeats } = seats;

      const data = {
        projectionId: projectionId,
        seatId: currentReservationSeats.map((id)=>{
          return id['currentSeatId'];
        }),
        userId: info.userId,
      };
      console.log(data);

      const requestOptions = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
        body: JSON.stringify(data),
      };

      // fetch(`${serviceConfig.baseURL}/api/reservations`, requestOptions)
      fetch(`${serviceConfig.baseURL}/api/ticket/create`, requestOptions)
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.statusText;
        })
        .then((result) => {
          NotificationManager.success(
            "Your reservation has been made successfully!"
          );
          setTimeout(() => {
            window.location.reload();
          }, 2000);
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          // setState((prev)=>({ ...prev, submitted: false }));
        });
    } else {
      NotificationManager.error("Please log in to make reservation.");
    }
  };
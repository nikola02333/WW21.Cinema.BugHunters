import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { Row,Button, Container } from "react-bootstrap";
import { getUserName, getRole } from "../helpers/authCheck";
import { withRouter } from "react-router";
import { IProjection, IUser, IReservation } from "../../models";


import {userService} from '../Services/userService';
interface IState {
  user: IUser;
  reservations: IReservation[];
  projection: IProjection[];
  submitted: boolean;
}

const UserProfile: React.FC = () => {
  const [state, setState] = useState<IState>({
    user: {
      id: "",
      firstName: "",
      lastName: "",
      bonusPoints: "",
    },
    reservations: [
      {
        projectionId: "",
      },
    ],
    projection: [],
    submitted: false,
  });

  useEffect(() => {
    getUserByUsername();
    hideSingUpButtonElement();
  }, []);

  useEffect(  ()=> {
    
    let userName= getUserName();
    async function fetchUserByUsername(){
    let data = await userService.getUserByUsername(userName);

     let user123 :IUser= {
       id : data.id,
       firstName: data.firstName,
       lastName : data.lastName,
       bonusPoints: data.bonusPoints
     }
      setState((prevState)=> ({...prevState, user: data}));

    }
    if(userName != null)
    {
      fetchUserByUsername()
    }
    
   
  },[]);

  const hideSingUpButtonElement = () => {
    let singUpButton = document.getElementById("singUp");
    if (singUpButton) {
      singUpButton.style.display = "none";
    }
   
  };
  const getUserByUsername = async() => {
    let userName = getUserName();

  // setState({ ...state, user: data });

  //getReservationsByUserId(state.user.id);
    if( userName!= null)
    {
      const user = await userService.getUserByUsername(userName);
      if(user != null)
      {
        setState({ ...state, user: user });
      }
    }
    
  };

  const getReservationsByUserId = (userId: string) => {
    /**
     *  if (data) {
          setState({ ...state, reservations: data });
          state.reservations.map((reservation) => {
            getProjectionById(reservation.projectionId);
          });
        }
     */
   
  };

  const EditUser = ()=>{

    
  };
  const getProjectionById = (projectionId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/projections/byprojectionid/${projectionId}`,
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
          setState({ ...state, projection: data });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  return (
     <div className="container d-flex justify-content-center ">
    <div className="row">
    <div className="col-md-auto">
      <Row className="no-gutters pt-3">
        <h1 className="form-header form-heading">
          Hello, {state.user.firstName}!
        </h1>
      </Row>
      <Row className="no-gutters pr-5 pl-5">
        <div className="card mb-3 user-info-container">
          <div className="row no-gutters">
            <div className="col-md-4">
              <img
                src="https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcS8tVjlY8BQfSZg9SoudTWMCR6eHXpi-QHhQDUYSyjFmHYOTyyp"
                className="avatar-img"
                alt="..."
              />
            </div>
            <div className="col-md-8">
              <div className="card-body">
                <h5 className="card-title">User details:</h5>
                <p className="card-text">
                  <strong>Full name:</strong>{" "}
                  {`${state.user.firstName} ${state.user.lastName}`}
                </p>
                <p className="card-text">
                  <strong>Bonus points: </strong> {state.user.bonusPoints}
                </p>
                <p className="card-text">
                  <strong>Status: </strong> {getRole()}
                </p>
                <div className="d-flex justify-content-center">
                <Button type="submit" className="mx-1" variant="danger" id="delete" >Delete User</Button>
                <Button type="button" onClick={()=>EditUser()} className="mx-1" variant="primary" id="edit" >Edit User</Button>
                </div>
              </div>
            </div>
          </div>
        </div>
       
      </Row>
      </div>
        </div>
        </div>
  );
};

export default withRouter(UserProfile);

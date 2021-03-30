import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { Row,Button, FormControl } from "react-bootstrap";
import { getUserName, getRole } from "../helpers/authCheck";
import { withRouter } from "react-router";
import { IProjection, IUser, IReservation } from "../../models";


import {userService} from '../Services/userService';
import {IUserUpdate} from '../../models/IUserUpdate';

import { stat } from "node:fs";
interface IState {
  user: IUser;
  reservations: IReservation[];
  projection: IProjection[];
  isEdit: boolean;
  submitted: boolean;
  firstName:string;
  lastName:string;
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
    isEdit:false,
    firstName:'',
    lastName:''

  });

  useEffect(() => {
    getUserByUsername();
    hideSingUpButtonElement();
  }, []);

  useEffect(  ()=> {
    
    let userName= getUserName();
    async function fetchUserByUsername(){
    let data = await userService.getUserByUsername(userName);

    
    if(data === undefined)
    {
      return;
    }
     let user123 :IUser= {
       id : data.id,
       firstName: data.firstName,
       lastName : data.lastName,
       bonusPoints: data.bonusPoints
     }
      setState((prevState)=> ({...prevState, user: data}));

      //setState({ ...state, firstName: data.firstName });
     // da li postoji neke drugi nacin, da  menjam iz user objekta podatke
      setState( (prevState) => ({...prevState, firstName: data.firstName, lastName: data.lastName}));

      //setState({ ...state, lastName: data.lastName });

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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value, name } = e.target;
  
    setState({ ...state, [id]: value });
    // setState ( ...prev, user:{...prev, name:data.name})
  };

  const EditUser = async ()=>{

   
   //var result = await userService.edit
   setState( (prevState)=> ({...prevState, isEdit: true}))

   var userUpdate :IUserUpdate ={
    id : state.user.id,
    firstName : state.firstName,
    lastName : state.lastName
   };
     
   var editUser= await userService.editUser(userUpdate);

   if(editUser=== undefined)
   {
      return;
   }
   setState( (prevState)=> ({...prevState, user: editUser}));
      //window.location.reload();
     
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
                  <strong>First name:</strong>{" "}
                  <FormControl
            type="text"
            placeholder="First Name"
            id="firstName"
            value={state.firstName}
            onChange={handleChange}
            className="mr-sm-2"
          />
          <strong>Last name:</strong>{" "}
                  <FormControl
            type="text"
            placeholder="Last Name"
            id="lastName"
            value={state.lastName}
            onChange={handleChange}
            className="mr-sm-2"
          />
                </p>
                <p className="card-text">
                  <strong>Bonus points: </strong> {state.user.bonusPoints}
                </p>
                <p className="card-text">
                  <strong>Status: </strong> {getRole()}
                </p>
                <div className="d-flex justify-content-center">
                <Button type="submit" className="mx-1" variant="danger" id="delete" >Remove User</Button>
                <Button type="button"  onClick={()=>EditUser()} className="mx-1" variant="primary" id="edit" >Edit User</Button>
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

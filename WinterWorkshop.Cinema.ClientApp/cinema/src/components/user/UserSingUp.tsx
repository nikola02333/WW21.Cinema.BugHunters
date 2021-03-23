import React, { useEffect, useState } from "react";
import { withRouter,useHistory } from "react-router-dom";
import { Link } from "react-router-dom";
import { Navbar, Nav, Form, FormControl, Button } from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";

import {userService} from '../Services/userService';
import  {IUserToCreateModel} from '../../models/IUserToCreateModel';
  


import {
    FormGroup,
    Container,
    Row,
    Col,
    FormText,
  } from "react-bootstrap";

interface IState {
    userName: string;
    userNameError: string;
    firstName: string;
    firstNameError: string;
    lastName: string;
    lastNameError: string;
    submitted : boolean;
    canSubmit: boolean
  }

  
  const UserSingUp: React.FC = (props: any) => {

    const history = useHistory();
    const [state, setState] = useState<IState>({
        userName: "",
        userNameError:"",
        firstName: "",
        firstNameError:"",
        lastName: "",
        lastNameError :"",
        submitted: false,
        canSubmit: true
      });

useEffect( ()=> {
  hideSingUpButtonElement() ;
},[]);
      const hideSingUpButtonElement = () => {
        let singUpButton = document.getElementById("singUp");
        if (singUpButton) {
          singUpButton.style.display = "none";
        }
       
      };
      const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      
        const { id,value } = e.target;
        validate(id, value);
        setState({ ...state, [id]: value });
        console.log(id, value);
        
      };
    
      const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
    
     
        setState({ ...state, submitted: true });
      let userToCreate :IUserToCreateModel = 
      {
        userName: state.userName,
        firstName: state.firstName,
        lastName: state.lastName
      };
      if (state.userName && state.firstName &&  state.lastName) {
        
       
       var data= await userService.singUp(userToCreate);
        history.push('/dashboard/Projection');

      } else {
        NotificationManager.error("Please fill in data");
        setState({ ...state, submitted: false });
      }
      
      };

      
  const handleSubmitLogout = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    localStorage.removeItem("userLoggedIn");
    //getTokenForGuest();
  };


      const validate = (id: string, value: string | null) => {
      
        if (id === "userName") {
          if (value === "") {
            setState({
              ...state,
              userNameError: "Fill in Username ",
              canSubmit: false,
            });
          } else {
            setState({ ...state, userNameError: "", canSubmit: true });
          }
        } else if (id === "firstName") {
          
          if (value ==="") {
            setState({
              ...state,
              firstNameError: "Fill in FirstName.",
              canSubmit: false,
            });
          } else {
            setState({ ...state, firstNameError: "", canSubmit: true });
          }
        } else if (id === "lastNameError") {
          if (value ==="") {
            setState({
              ...state,
              lastNameError: "Fill in LastName.",
              canSubmit: false,
            });
          } else {
            setState({ ...state, lastNameError: "", canSubmit: true });
          }
        }
      };
      console.log(state)
    return (
        <Container>
      <Row>
        <Col>
          <h1 className="form-header">Create New User</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="userName"
                type="text"
                placeholder="User Name"
                value={state.userName}
                className="add-new-form"
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.userNameError}</FormText>
              <FormControl
                id="firstName"
                type="text"
                placeholder="First Name"
                value={state.firstName}
                onChange={handleChange}
                className="add-new-form"
              />
              <FormText className="text-danger">
                {state.firstNameError}
              </FormText>
              <FormControl
                id="lastName"
                className="add-new-form"
                type="text"
                placeholder="Last Name"
                value={state.lastName}
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.lastNameError}</FormText>
            
            </FormGroup>
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Create User
            </Button>
          </form>
        </Col>
      </Row>
    </Container>
    );
}
    export default withRouter(UserSingUp);

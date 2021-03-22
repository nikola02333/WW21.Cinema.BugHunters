import React, { useEffect, useState } from "react";
import { withRouter,useHistory } from "react-router-dom";
import { Link } from "react-router-dom";
import { Navbar, Nav, Form, FormControl, Button } from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";

import {userService} from '../Services/userService';

interface IState {
    userName: string;
    submitted: boolean;
    token: boolean;
    shouldHide: boolean;
  }

  
  const UserComponent: React.FC = (props: any) => {

    const history = useHistory();
    const [state, setState] = useState<IState>({
        userName: "",
        submitted: false,
        token: false,
        shouldHide: true,
      });
    useEffect(() => {

          if (localStorage.getItem("userLoggedIn") !== null) {
            hideLoginButtonElement();
            hideSingUpButtonElement();
           
          } else {
            hideLogoutButtonElement();
            hideUserProfileButtonElement();
          }
      }, []);

      const hideSingUpButtonElement = () => {
        let singUpButton = document.getElementById("singUp");
        if (singUpButton) {
          singUpButton.style.display = "none";
        }
       
      };
      const hideUserProfileButtonElement = () => {
        let userProfileButton = document.getElementById("userProfile");
        if (userProfileButton) {
          userProfileButton.style.display = "none";
        }
       
      };
      //hideUserProfileButtonElement
      const hideLoginButtonElement = () => {
        let loginButton = document.getElementById("login");
        if (loginButton) {
          loginButton.style.display = "none";
        }
        let logoutButton = document.getElementById("logout");
        if (logoutButton) {
          logoutButton.style.display = "block";
        }
        document.getElementById("username")!.style.display = "none";
      };
    
      const hideLogoutButtonElement = () => {
        let loginButton = document.getElementById("login");
    
        if (loginButton) {
          loginButton.style.display = "block";
        }
        let logoutButton = document.getElementById("logout");
        if (logoutButton) {
          logoutButton.style.display = "none";
        }
        document.getElementById("username")!.style.display = "block";
      };

      let shouldDisplayUserProfile = true;

  const shouldShowUserProfile = () => {
    if (shouldDisplayUserProfile === undefined) {
      //shouldDisplayUserProfile = !isGuest();
    }
    return shouldDisplayUserProfile;
  };


      const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      
        const { value } = e.target;
        
        //setState({ ...state, userName: value });

        setState( (prev) => ({
          ...prev,  userName: value
        }));
       
      };
    
      const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
    
        //setState({ ...state, submitted: true });
        setState( (prev) => ({
          ...prev, submitted: true
        }));
        const { userName } = state;
        if (userName) {
          userService.login(userName);
        } else {

          setState( (prev) => ({
            ...prev, submitted: false
          }));
        }
      };

      
  const handleSubmitLogout = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    localStorage.removeItem("userLoggedIn");

    //setState({ ...state, submitted: true });
    setState( (prev) => ({
      ...prev, submitted: true
    }));


    //setState({ ...state, token: false });
    setState( (prev) => ({
      ...prev, token: false
    }));
    
    window.location.reload();
    //getTokenForGuest();
  };

  const redicectToSingUp =()=>{
    history.push('/NewUser');
  }
  const redirectToUserPage = () => {
    history.push('/userprofile');
  };

      console.log(state.userName);

    return (
        
        <>
          <Form
          inline
          onSubmit={(e: React.FormEvent<HTMLFormElement>) => handleSubmit(e)}
        >
          <FormControl
            type="text"
            placeholder="Username"
            id="username"
            value={state.userName}
            onChange={handleChange}
            className="mr-sm-2"
          />
          <Button type="submit" variant="outline-success" id="login">
            Login
          </Button>
        </Form>
        <Form
          inline
          onSubmit={(e: React.FormEvent<HTMLFormElement>) => handleSubmit(e)}
        >
         
          <Button onClick={ redicectToSingUp} type="submit" variant="outline-success" id="singUp">
            SingUp
          </Button>
        </Form>
        {shouldShowUserProfile() && (
          <Button
          id="userProfile"
            style={{ backgroundColor: "transparent", marginRight: "10px" }}
            onClick={redirectToUserPage}
          >
            {/*getUserName()*/ "userProfile"}
          </Button>
        )}
        <Form
          inline
          onSubmit={(e: React.FormEvent<HTMLFormElement>) =>
            handleSubmitLogout(e)
          }
        >
          <Button  type="submit" variant="outline-danger" id="logout">
         {"Logout" + state.userName}
          </Button>
        </Form>
        </>
    );
}
    export default withRouter(UserComponent);

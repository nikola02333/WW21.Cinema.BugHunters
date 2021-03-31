import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Link } from "react-router-dom";
import { Navbar, Nav, Form, FormControl, Button } from "react-bootstrap";

import UserComponent from './user/UserComponent';
import {
  getTokenExp,
  isGuest,
  getUserName,
} from "../../src/components/helpers/authCheck";
import { userService } from "./Services/userService";
import { checkRole } from "./helpers/roleCheck";
interface IState {
  username: string;
  submitted: boolean;
  token: boolean;
  shouldHide: boolean;
}

const Header: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    username: "",
    submitted: false,
    token: false,
    shouldHide: true,
  });
   
  /*
  useEffect( ()=> {

    if(localStorage.getItem("userLoggedIn") === null)
    {
      getTokenForGuest();
    }
  },[]);
  */

  useEffect( ()=> {
   async function fetcsAsync(){

    if(localStorage.getItem("userLoggedIn") === null)
    {
      await getTokenForGuest();
    }
    else{
      setTimeout(()=> {}, 6000)
    }
   }
    
   fetcsAsync()
  },[]);


  const getTokenForGuest =async()=>{

    let guestTOken = await userService.getTokenForGuest();


    if(guestTOken === undefined)
    {
    localStorage.setItem("jwt", guestTOken);

    }
    else{
      setTimeout(()=> {}, 6000)
    }
  };
 



  useEffect(() => {
    let tokenExp = getTokenExp();
    let currentTimestamp = +new Date();
    if (!tokenExp || tokenExp * 1000 < currentTimestamp) {
      getTokenForGuest();
    }
  }, []);

  return (
    <Navbar className="nav-menu-bg" expand="lg">
      <Navbar.Brand className="text-info font-weight-bold text-capitalize">
        <Link className="text-decoration-none" to="/dashboard/Projections">
          Cinema by BugHunters
        </Link>
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" className="text-white" />
      <Navbar.Collapse id="basic-navbar-nav" className="text-white">
        <Nav className="mr-auto text-white"> </Nav>
        
        <UserComponent></UserComponent>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default withRouter(Header);

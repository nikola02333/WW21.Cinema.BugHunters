import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Link } from "react-router-dom";
import { Navbar, Nav, Form, FormControl, Button } from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../appSettings";

import UserComponent from './user/UserComponent';
import {
  getTokenExp,
  isGuest,
  getUserName,
} from "../../src/components/helpers/authCheck";

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

  useEffect(() => {
    let tokenExp = getTokenExp();
    let currentTimestamp = +new Date();
    if (!tokenExp || tokenExp * 1000 < currentTimestamp) {
      //getTokenForGuest();
    }
  }, []);
/*
  const getTokenForGuest = () => {
    const requestOptions = {
      method: "GET",
    };
    fetch(`${serviceConfig.baseURL}/get-token?guest=true`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        setState({ ...state, shouldHide: true });
        if (data.token) {
          localStorage.setItem("jwt", data.token);
          window.location.reload();
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
    state.token = true;
  };
*/
  return (
    <Navbar bg="dark" expand="lg">
      <Navbar.Brand className="text-info font-weight-bold text-capitalize">
        <Link className="text-decoration-none" to="/dashboard/Projection">
          Cinema 9
        </Link>
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" className="text-white" />
      <Navbar.Collapse id="basic-navbar-nav" className="text-white">
        <Nav className="mr-auto text-white"></Nav>


        <UserComponent></UserComponent>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default withRouter(Header);

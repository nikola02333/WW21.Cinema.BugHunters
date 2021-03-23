import React, { useState,Fragment } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";
import { ImportsNotUsedAsValues } from "typescript";




interface IState {
  name: string;
  address:string;
  cityName:string; 
  auditName: string,
  seatRows: number,
  numberOfSeats: number;
  auditNameError: string;
  seatRowsError: string;
  numOfSeatsError: string;
  nameError: string; 
  submitted: boolean;
  canSubmit: boolean;

};

const NewCinema: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    name: "",
    address:"",
    cityName:"",
    nameError:"",   
    auditName:"",
    seatRows: 0,
    numberOfSeats: 0,  
    auditNameError: "",
    seatRowsError: "",
    numOfSeatsError: "",
    submitted: false,
    canSubmit: true,
    
  });


  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    validate(id, value);
    setState({ ...state, [id]: value });
    
  };

  const validate = (id: string, value: string) => {
    if (id === "name") {
      if (value === "") {
        setState({
          ...state,
          nameError: "Fill in cinema name",
          canSubmit: false,
        });
      } else {
        setState({ ...state, nameError: "", canSubmit: true });
      }
    }
    if (id === "address") {
      if (value === "") {
        setState({
          ...state,
          auditNameError: "Fill in cinema address",
          canSubmit: false,
        });
      } else {
        setState({ ...state,auditNameError: "", canSubmit: true });
      }
    }
      if (id === "cityName") {
        if (value === "") {
          setState({
            ...state,
           auditNameError: "Fill in cinema city name",
            canSubmit: false,
          });
        } else {
          setState({ ...state, auditNameError: "", canSubmit: true });
        }
      }
      
    if (id === "auditName") {
      if (value === "") {
        setState({
          ...state,
          auditNameError: "Fill in auditorium name",
          canSubmit: false,
        });
      } else {
        setState({ ...state, auditNameError: "", canSubmit: true });
      }
    } else if (id === "numberOfSeats") {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState({
          ...state,
         numOfSeatsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        });
      } else {
        setState({ ...state,numOfSeatsError: "", canSubmit: true });
      }
    } else if (id === "seatRows") {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState({
          ...state,
         seatRowsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        });
      } else {
        setState({ ...state,seatRowsError: "", canSubmit: true });
      }
    }
    
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    if (state.name && state.address && state.cityName && state.auditName && state.numberOfSeats && state.seatRows) {
      addCinema();
    } 
    else if(state.name && state.address && state.cityName)
    {
      addCinemaWithoutAuditorium();
    }
    else {
      NotificationManager.error("Please fill in data");
      setState({ ...state, submitted: false });
    }
  };

 
  const addCinema = () => {

      const data = {
        Name: state.name, 
        address:state.address,
        cityName:state.cityName,  
        createAuditoriumModel:[{
          numberOfSeats: +state.numberOfSeats, 
          seatRows: +state.seatRows,
          auditName: state.auditName
        }] 
    };
    
    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/cinemas/create`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("Successfuly added cinema!");
        props.history.push(`AllCinemas`);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };


  const addCinemaWithoutAuditorium = () => {

    const data = {
      Name: state.name, 
      address:state.address,
      cityName:state.cityName,   
  };
  
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("jwt")}`,
    },
    body: JSON.stringify(data),
  };

  fetch(`${serviceConfig.baseURL}/api/cinemas/create`, requestOptions)
    .then((response) => {
      if (!response.ok) {
        return Promise.reject(response);
      }
      return response.statusText;
    })
    .then((result) => {
      NotificationManager.success("Successfuly added cinema!");
      props.history.push(`AllCinemas`);
    })
    .catch((response) => {
      NotificationManager.error(response.message || response.statusText);
      setState({ ...state, submitted: false });
    });
};

  const renderRows = (rows: number, seats: number) => {
    const rowsRendered: JSX.Element[] = [];
    for (let i = 0; i < rows; i++) {
      rowsRendered.push(<tr key={i}>{renderSeats(seats, i)}</tr>);
    }
    return rowsRendered;
  };

  const renderSeats = (seats: number, row: React.Key) => {
    let renderedSeats: JSX.Element[] = [];
    for (let i = 0; i < seats; i++) {
      renderedSeats.push(
        <td id="test" className="rendering-seats" key={`row${row}-seat${i}`}>
          <FontAwesomeIcon className="fa-2x couch-icon" icon={faCouch} />
        </td>
      );
    }
    return renderedSeats;
  };

 
  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add New Cinema</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="name"
                type="text"
                placeholder="Cinema name"
                value={state.name} 
                className="add-new-form"             
                onChange={handleChange}
              />          
              <FormControl
                id="address"
                type="text"
                placeholder="Cinema address"
                value={state.address} 
                className="add-new-form"             
                onChange={handleChange}
              />
              <FormControl
                id="cityName"
                type="text"
                placeholder="Cinema city name"
                value={state.cityName} 
                className="add-new-form"             
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.nameError}</FormText>
              <h1 className="form-header">Add Auditorium For Cinema</h1>
              <FormControl
                id="auditName"
                type="text"
                placeholder="Auditorium name"
                value={state.auditName} 
                onChange={handleChange}
                className="add-new-form"
              />
              <FormText className="text-danger">
                {state.auditNameError}
              </FormText>
              <FormControl
                id="seatRows"
                className="add-new-form"
                type="number"
                placeholder="Number Of rows"
                value={state.seatRows.toString()} 
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.seatRowsError}</FormText>
              <FormControl
                id="numberOfSeats"
                className="add-new-form"
                type="number"
                placeholder="Number Of seats"
                value={state.numberOfSeats.toString()} 
                onChange={handleChange}
                max="36"
              />
              <FormText className="text-danger">
                {state.numOfSeatsError}
              </FormText>
             
            </FormGroup>
           
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Add
            </Button>
          </form>
        </Col>
      </Row>
      <Row className="mt-2">
        <Col className="justify-content-center align-content-center">
          <h1 className="form-header">Auditorium Preview</h1>
          <div>
            <Row className="justify-content-center">
              <table className="table-cinema-auditorium">
                <tbody>{renderRows(state.seatRows, state.numberOfSeats)}</tbody>
              </table>
            </Row>
            <Row className="justify-content-center mb-4">
              <div className="text-center text-white font-weight-bold cinema-screen">
                CINEMA SCREEN
              </div>
            </Row>
          </div>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewCinema);

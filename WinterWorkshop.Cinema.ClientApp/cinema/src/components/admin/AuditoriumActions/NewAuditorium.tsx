import React, { useEffect, useState } from "react";
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
import { Typeahead } from "react-bootstrap-typeahead";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";
import "./../../../index.css";
import { ICinema } from "../../../models";
import {auditoriumService} from './../../Services/auditoriumService';
import {cinemaService} from './../../Services/cinemaService';
import { ICreateAuditorium} from './../../../models/AuditoriumModels';

interface IState {
  cinemaId: string;
  auditoriumName: string;
  seatRows: number;
  numberOfSeats: number;
  cinemas: ICinema[];
  cinemaIdError: string;
  auditoriumNameError: string;
  seatRowsError: string;
  numOfSeatsError: string;
  submitted: boolean;
  canSubmit: boolean;
  isLoading: boolean;
}

const NewAuditorium: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    cinemaId: "",
    auditoriumName: "",
    seatRows: 0,
    numberOfSeats: 0,
    cinemas: [
      {
        id: "",
        name: "",
        address:"",
        cityName:""
      },
    ],
    cinemaIdError: "",
    auditoriumNameError: "",
    seatRowsError: "",
    numOfSeatsError: "",
    submitted: false,
    canSubmit: true,
    isLoading: true
  });

  const getCinemas = async () => {
    setState({ ...state, isLoading: true });
    var cinemas= await cinemaService.getCinemas();
    setState({ ...state, cinemas: cinemas, isLoading: false });
  };

  useEffect(() => {
    getCinemas();
  },[]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {

    const { id, value } = e.target;
    validate(id, value);
    setState({ ...state, [id]: value });
    
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    if (
      state.auditoriumName &&
      state.numberOfSeats &&
      state.cinemaId &&
      state.seatRows
    ) {
      addAuditorium();
    } else {
      NotificationManager.error("Please fill form with data.");
      setState({ ...state, submitted: false });
    }
  };

  const validate = (id: string, value: string | null) => {
    if (id === "auditoriumName") {
      if (value === "") {
        setState({
          ...state,
          auditoriumNameError: "Fill in auditorium name",
          canSubmit: false,
        });
      } else {
        setState({ ...state, auditoriumNameError: "", canSubmit: true });
      }
    } else if (id === "numberOfSeats" && value) {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState({
          ...state,
          numOfSeatsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, numOfSeatsError: "", canSubmit: true });
      }
    } else if (id === "seatRows" && value) {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState({
          ...state,
          seatRowsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, seatRowsError: "", canSubmit: true });
      }
    } else if (id === "cinemaId") {
      if (!value) {
        setState({
          ...state,
          cinemaIdError: "Please chose cinema from dropdown list.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, cinemaIdError: "", canSubmit: true });
      }
    }
  };

  const addAuditorium = async () => {
    var auditoriumToCreate : ICreateAuditorium = {
      cinemaId : state.cinemaId,
      auditoriumName: state.auditoriumName,
      numberOfSeats : +state.numberOfSeats,
      seatRows : +state.seatRows
    }; 
   var created=await auditoriumService.createAuditorium(auditoriumToCreate);
   if(created===undefined){
    setState((prev)=>({ ...prev, submitted: false }));
    return;
    }
    NotificationManager.success("New auditorium added!");
    props.history.push(`AllAuditoriums`); 
  };

  const onCinemaChange = (cinemas: ICinema[]) => {
    if (cinemas[0]) {
      validate("cinemaId", cinemas[0].id);
      setState({ ...state, cinemaId: cinemas[0].id});
      
    } else {
      validate("cinemaId", null);     
    }
  };

  const renderSeats = (seats, row) => {
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

  const renderRows = (rows: number, seats: number) => {
    const rowsRendered: JSX.Element[] = [];
    for (let i = 0; i < rows; i++) {
      rowsRendered.push(<tr key={i}>{renderSeats(seats, i)}</tr>);
    }
    return rowsRendered;
  };

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add Auditorium</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="auditoriumName"
                type="text"
                placeholder="Auditorium Name"
                value={state.auditoriumName}
                className="add-new-form"
                onChange={handleChange}             
              />
              <FormText className="text-danger">
                {state.auditoriumNameError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <Typeahead
                labelKey="name"
                className="add-new-form"
                options={state.cinemas}
                placeholder="Choose a cinema..."
                id="browser"
                onChange={(e: ICinema[]) => {
                onCinemaChange(e);
                }}
              />
              <FormText className="text-danger">{state.cinemaIdError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="seatRows"
                className="add-new-form"
                type="number"
                placeholder="Number Of Rows"
                value={state.seatRows.toString()}
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.seatRowsError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="numberOfSeats"
                className="add-new-form"
                type="number"
                placeholder="Number Of Seats"
                value={state.numberOfSeats.toString()}
                onChange={handleChange}
                max="36"
              />
              <FormText className="text-danger">
                {state.numOfSeatsError}
              </FormText>
            </FormGroup>
            <Row className="d-flex justify-content-center">
            <Button
              type="submit"
              className="col-1"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Add
            </Button>
            </Row>
          </form>
        </Col>
      </Row>
      <Row className="mt-2">
        <Col className="justify-content-center align-content-center">
          <h1 className="form-header">Auditorium Preview</h1>
          <div>
            <Row className="justify-content-center mb-4">
              <div className="text-center text-white font-weight-bold cinema-screen">
                CINEMA SCREEN
              </div>
            </Row>
            <Row className="justify-content-center">
              <table className="table-cinema-auditorium">
                <tbody>{renderRows(state.seatRows, state.numberOfSeats)}</tbody>
              </table>
            </Row>
          </div>
        </Col>
      </Row>
    </Container>
  );
};
export default withRouter(NewAuditorium);

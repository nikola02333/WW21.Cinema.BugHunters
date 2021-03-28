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
import { ICinema } from "../../../models";
import {cinemaService} from '../../Services/cinemaService'

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

const EditAuditorium: React.FC = (props: any): JSX.Element => {
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
    isLoading: true,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
    //validate(id, value);
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
      editAuditorium();
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

  const editAuditorium = () => {
    const idFromUrl = window.location.pathname.split("/");
    const id = idFromUrl[3];

    const data = {
      cinemaId: state.cinemaId,
      numberOfSeats: +state.numberOfSeats,
      seatRows: +state.seatRows,
      auditoriumName: state.auditoriumName,
    };

    const requestOptions = {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(
      `${serviceConfig.baseURL}/api/auditoriums/editauditorium/${id}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("Successfuly edited auditorium!");
        props.history.push("AllAuditoriums");
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getCinemas = async () => {
    setState({ ...state, isLoading: true });
     
    var cinemas= await cinemaService.getCinemas();
    setState({ ...state, cinemas: cinemas, isLoading: false });
  };

  useEffect(() => {
    getCinemas();
  }, []);

  const onCinemaChange = (cinemas: ICinema[]) => {
    if (cinemas[0]) {
      setState({ ...state, cinemaId: cinemas[0].id });
      validate("cinemaId", cinemas[0].id);
    } else {
      validate("cinemaId", null);
    }
  };

  const renderRows = (rows: number, seats: number) => {
    const rowsRendered: JSX.Element[] = [];
    for (let i = 0; i < rows; i++) {
      rowsRendered.push(<tr key={i}>{renderSeats(seats, i)}</tr>);
    }
    return rowsRendered;
  };

  const renderSeats = (seats: number, row: number) => {
    let renderedSeats: JSX.Element[] = [];
    for (let i = 0; i < seats; i++) {
      renderedSeats.push(<td key={`row${row}-seat${i}`}></td>);
    }
    return renderedSeats;
  };

  return (
    <Container>
      <Row className="d-flex justify-content-center mt-3">
        <Col xs={12}>
          <h1 className="form-header">Edit Auditorium</h1>
        </Col>
        <Col xs={11} md={9} lg={7} xl={5}>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="auditoriumName"
                type="text"
                placeholder="Auditorium Name"
                value={state.auditoriumName}
                onChange={handleChange}
              />
              <FormText className="text-danger">
                {state.auditoriumNameError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <Typeahead
                labelKey="name"
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
                min="1"
                id="seatRows"
                type="number"
                placeholder="Number Of Rows"
                value={state.seatRows.toString()}
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.seatRowsError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                min="1"
                id="numberOfSeats"
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
            <Button
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Edit Auditorium
            </Button>
          </form>
        </Col>
      </Row>
      <Row className="mt-2">
        <Col className="justify-content-center align-content-center">
          <h1>Auditorium Preview</h1>
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

export default withRouter(EditAuditorium);

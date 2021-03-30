import React, { useEffect, useState } from "react";
import { useHistory,useParams,withRouter } from "react-router-dom";
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
import {auditoriumService} from '../../Services/auditoriumService'
interface IParams {
  id: string;
}
interface IState {
  cinemaId: string;
  id:string;
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
  var id = window.location.pathname.split("/")[3];
  const history = useHistory();
  
  const [state, setState] = useState<IState>({
    id:"",
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
    validate(id, value);
    setState((prev)=>({ ...prev, [id]: value }));
  
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState((prev)=>({ ...prev, submitted: true }));
    if (
      state.auditoriumName &&
      state.numberOfSeats &&
      state.cinemaId &&
      state.seatRows
    ) {
      editAuditorium();
    } else {
      NotificationManager.error("Please fill form with data.");
      setState((prev)=>({ ...prev, submitted: false }));
    }
  };

  const validate = (id: string, value: string | null) => {
    if (id === "auditoriumName") {
      if (value === "") {
        setState((prev)=>({
          ...prev,
          auditoriumNameError: "Fill in auditorium name",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, auditoriumNameError: "", canSubmit: true }));
      }
    } else if (id === "numberOfSeats" && value) {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState((prev)=>({
          ...prev,
          numOfSeatsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, numOfSeatsError: "", canSubmit: true }));
      }
    } else if (id === "seatRows" && value) {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState((prev)=>({
          ...prev,
          seatRowsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, seatRowsError: "", canSubmit: true }));
      }
    } else if (id === "cinemaId") {
      if (value==="") {
        setState((prev)=>({
          ...prev,
          cinemaIdError: "Please chose cinema from dropdown list.",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, cinemaIdError: "", canSubmit: true }));
      }
    }
  };

  const editAuditorium = async () => {
    const data = {
      cinemaId: state.cinemaId,
      numberOfSeats: +state.numberOfSeats,
      seatRows: +state.seatRows,
      auditoriumName: state.auditoriumName,
    };
    await auditoriumService.updateAuditorium(id,data);
    history.push('/dashboard/AllAuditoriums'); 
  };

  const getAuditoriumById = async (auditoriumId: string) => {

    var auditorium = await auditoriumService.getAuditoriumById(auditoriumId);
    if(auditorium  != undefined)
    {
      setState((prev)=>({
        ...prev,
        cinemaId:auditorium.cinemaId,
        auditoriumName: auditorium.auditoriumName,
        id: auditorium.id + ""
      }));
    }
  };

  const getCinemas = async () => {
    setState((prev)=>({ ...prev, isLoading: true }));
     
    var cinemas= await cinemaService.getCinemas();
    setState((prev)=>({ ...prev, cinemas: cinemas, isLoading: false }));
  };

  useEffect(() => {
    getAuditoriumById(id);
  }, [id]);
  useEffect(() => {
    getCinemas();
  }, []);

  const onCinemaChange = (e) => {
    const { id, value } = e;
    if (value!==null) {
      setState((prev)=>({ ...prev, cinemaId: value }));
      validate("cinemaId", value);
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
              <FormControl
                as="select"
                id="cinemaId"
                onChange={(e) => {
                  onCinemaChange(e);
                }}
              >
                <option value={""}>Choose a cinema...</option>
                {state.cinemas.map((cinema)=>{
                return(
                  <option key={cinema.id} value={cinema.id}>{cinema.name}</option>
                );
              })}
              </FormControl>
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

import React, { useEffect, useState } from "react";
import { useHistory, useParams, withRouter } from "react-router-dom";
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
import {cinemaService} from '../../Services/cinemaService'
interface IParams {
  id: string;
}
interface IState {
  name: string;
  id: string;
  address: string;
  cityName: string;
  nameError: string;
  submitted: boolean;
  canSubmit: boolean;
}

const EditCinema: React.FC = (props: any) => {
  const history = useHistory();
  const { id } = useParams<IParams>();
  const [state, setState] = useState<IState>({
    name: "",
    id: "",
    address:"",
    cityName:"",
    nameError: "",
    submitted: false,
    canSubmit: true,
  });

  useEffect(() => {
    getCinemaById(id);
  }, [id]);

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
          nameError: "Fill in cinema name.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, nameError: "", canSubmit: true });
      }
    }
    if (id === "cityName") {
      if (value === "") {
        setState({
          ...state,
          nameError: "Fill in cinema city name.",
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
          nameError: "Fill in cinema address.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, nameError: "", canSubmit: true });
      }
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    if (state.name && state.address && state.cityName) {
      updateCinema();
    } else {
      NotificationManager.error("Please fill in data");
      setState({ ...state, submitted: false });
    }
  };

  const getCinemaById = async (cinemaId: string) => {

    var cinema = await cinemaService.getCinemaById(cinemaId);
    if(cinema  != undefined)
    {
      setState({
        ...state,
        cityName:cinema.cityName,
        name: cinema.name,
        address: cinema.address,
        id: cinema.id + "",
      });
    }
  };

  const updateCinema = async() => {
    const data = {
      Name: state.name,
      address:state.address,
      cityName:state.cityName
    };
    await cinemaService.updateCinema(id,data);
    history.push('/dashboard/AllCinemas');
  };

  return (
    <Container>
      <Row className="d-flex justify-content-center">
        <Col xs={5}>
          <h1 className="form-header">Edit Existing Cinema</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup >
              <FormControl
              className="my-2"
                id="name"
                type="text"
                placeholder="Cinema name"
                value={state.name}
                onChange={handleChange}
              />
               <FormControl
               className="my-2"
                id="address"
                type="text"
                placeholder="Cinema address"
                value={state.address}
                onChange={handleChange}
              />
               <FormControl
               className="my-2"
                id="cityName"
                type="text"
                placeholder="Cinema city mame"
                value={state.cityName}
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.nameError}</FormText>
            </FormGroup>
            <Button
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Edit Cinema
            </Button>
          </form>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(EditCinema);

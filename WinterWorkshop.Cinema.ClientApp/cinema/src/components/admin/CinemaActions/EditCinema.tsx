import React, { useEffect, useState } from "react";
import { useParams, withRouter } from "react-router-dom";
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
    getCinema(id);
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

  const getCinema = (cinemaId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/cinemas/GetById/${cinemaId}`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({ ...state, name: data.name, id: data.id ,address:data.address,cityName:data.cityName});
         
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const updateCinema = () => {
    const data = {
      Name: state.name,
      address:state.address,
      cityName:state.cityName
    };

    const requestOptions = {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/cinemas/${state.id}`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        props.history.goBack();
        NotificationManager.success("Successfuly edited cinema!");
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Edit Existing Cinema</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="name"
                type="text"
                placeholder="Cinema name"
                value={state.name}
                onChange={handleChange}
              />
               <FormControl
                id="address"
                type="text"
                placeholder="Cinema address"
                value={state.address}
                onChange={handleChange}
              />
               <FormControl
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

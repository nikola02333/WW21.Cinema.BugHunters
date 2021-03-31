import React, { useState,useEffect } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  Form,
  Container,
  Button,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";
import {cinemaService} from './../../Services/cinemaService';
import {ICinemaToCreateModel} from './../../../models/ICinemaToCreateModel';
import { ICreateAuditoriumWithCinema } from "../../../models/AuditoriumModels";
import DinamicAddAuditorium from "../AuditoriumActions/DinamicAddAuditorium/DinamicAddAuditorium"



interface IState {
  createCinema:ICinemaToCreateModel;
  submitted:boolean;
  auditoriums:ICreateAuditoriumWithCinema[]
  nameError:string;
  addressError:string;
  cityNameError:string;
  nameSubmit:boolean;
  addressSubmit:boolean;
  cityNameSubmit:boolean;
};

const NewCinema: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    createCinema:{
      name:"",
      address:"",
      cityName:"",
      createAuditoriumModel:[{
        auditoriumName:"",
        seatRows:1,
        numberOfSeats:1,
        id:1
      }]
    },
    auditoriums:[],
    nameError:"",
    addressError:"",
    cityNameError:"",
    nameSubmit:false,
    addressSubmit:false,
    cityNameSubmit:false,
    submitted: false
  });

  const handleChange = (e) => {
    const { id, value } = e;
    if (value!==null) {
      setState((prev)=>({ ...prev, createCinema:{...prev.createCinema, [id]: value}}));
      validate(id, value);
    } else {
      validate(id, null);
    }
  };

  const validate = (id, value) => {
    if (id === "name") {
      if (!value || value==="") {
        setState((prev)=>({ ...prev,
          nameError: "Please insert cinema name",
          nameSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, nameError: "",nameSubmit:true}));
      }
    } else if (id === "address") {
      if (!value || value==="") {
        setState((prev)=>({ ...prev,
          addressError: "Please insert address",
          addressSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, addressError: "" ,addressSubmit:true}));
      }
    } else if (id === "cityName"){
      if (!value ||value==="") {
        setState((prev)=>({ ...prev,
          cityNameError: "Please insert city name",
          cityNameSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, cityNameError:"",cityNameSubmit:true}));
      }
    }
  };

 
  const addCinema = (e) => {
    e.preventDefault();
    var data:ICinemaToCreateModel = { 
      name : state.createCinema.name,
      address : state.createCinema.address,
      cityName : state.createCinema.cityName,
      createAuditoriumModel :state.auditoriums
    }
    // setState((prev)=>({...prev,createCinema:{...prev.createCinema,createAuditoriumModel:prev.auditoriums}}));
    var created=cinemaService.addCinema(data);

    if(created===undefined){
    
      setState((prev)=>({ ...prev, submitted: false }));
      return;
      }
      setState((prev)=>({ ...prev, submitted: false }));
      
      NotificationManager.success("New cinema added!");
      props.history.push(`AllCinemas`);
      window.location.reload();
      
  };


  return (
    <Container>
      <Row className="d-flex justify-content-center">
        <Col xs={5}>
          <h2 className="form-header">Add New Cinema</h2>
          <Form onSubmit={addCinema} name="dynamic_form_nest_item" autoComplete="off">
          
          <FormGroup >
              <Form.Control
              type="text"
              placeholder={"Cinema name"}
              id="name"
              onChange={(e) => {
                handleChange(e.target);
              }}
              />
              <FormText className="text-danger text-center">
                {state.nameError}
              </FormText>
            </FormGroup>
            <FormGroup >
              <Form.Control
              type="text"
              placeholder={"Address"}
              id="address"
              onChange={(e) => {
                handleChange(e.target);
              }}
              />
              <FormText className="text-danger text-center">
                {state.addressError}
              </FormText>
            </FormGroup>
            <FormGroup >
              <Form.Control
              type="text"
              placeholder={"City name"}
              id="cityName"
              onChange={(e) => {
                handleChange(e.target);
              }}
              />
              <FormText className="text-danger text-center">
                {state.cityNameError}
              </FormText>
            </FormGroup>

          <h2 className="form-header">Add Auditorium</h2>
          <DinamicAddAuditorium auditoriums={state.auditoriums} setState={setState} />
          <Row className="d-flex justify-content-center">
            <Button 
            type="submit" 
            onClick={(e)=> addCinema(e)}
            disabled={(state.addressSubmit && state.nameSubmit && state.cityNameSubmit ) ? false : true}>
              Create cinema
            </Button>
          </Row>
        </Form>
        </Col>
      </Row>
      
     
    </Container>
  );
};

export default withRouter(NewCinema);

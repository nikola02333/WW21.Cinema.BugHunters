import React, { useEffect, useState ,useContext} from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import FilterProjections from "./FilterProjections";
import {ProjectionProvider} from "./ProjectionProvider";
import {ProjectionContext} from "./ProjectionProvider";


const Projections : React.FC = () => {
  
     const context = useContext(ProjectionContext);
    const state = context[0];
    const setState = context[1];
    useEffect(()=>{
      getAllCinemas();
      getAllAuditoriums();
    },[]);
    console.log("render CINEMAS");

    const getAllCinemas = () => {
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
      };
  
      setState((prev)=>({ ...prev, isLoading: true }));
      fetch(`${serviceConfig.baseURL}/api/Cinemas/all`, requestOptions)
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.json();
        })
        .then((data) => {
          if (data) {
            setState((prev)=>({
              ...prev,
              cinemas: data,
              isLoading: false,
            }));
            
          }
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          setState({ ...state, isLoading: false });
        });
    };

    const getAllAuditoriums = () => {
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
      };
  
      setState((prev)=>({ ...prev, isLoading: true }));
      fetch(`${serviceConfig.baseURL}/api/auditoriums/all`, requestOptions)
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.json();
        })
        .then((data) => {
          if (data) {
            setState((prev)=>({ ...prev, auditoriums: data, isLoading: false }));
          }
          
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          setState({ ...state, isLoading: false });
        });
    };
    return (
     
        <Container>
          <h1 className="projections-title">Current projections</h1>
          <FilterProjections></FilterProjections>
          <Row className="justify-content-center">
            <Col>
            
            <Card key={1} className="card-width"></Card>
            </Col>
          </Row>
        </Container>
        
        
      );
};

export default withRouter(Projections);
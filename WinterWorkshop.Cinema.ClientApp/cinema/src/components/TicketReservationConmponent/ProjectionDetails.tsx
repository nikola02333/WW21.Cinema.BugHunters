import React,{useState,useEffect,memo} from 'react';
import {IMovie,IProjection} from "../../models";
import {  Row, Col, Card } from "react-bootstrap";
import {getRoundedRating} from "../helpers/functions";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";

interface IState {
  projections:IProjection
  movie:IMovie
}

const ProjectionDeatils = (props:{setInfo,getSeatData})=>{

    const [state, setState]= useState<IState>({
        projections:{
            auditoriumId: 0,
            auditoriumName: "",
            duration: 0,
            id: "",
            movieId: "",
            movieTitle: "",
            price: 0,
            projectionTime: "",
            cinemaId:0,
            cinemaName:""
        } ,
        movie:{
            id: "",
            coverPicture: "",
            title: "",
            rating: 0,
            year: "",
            hasOscar:false
           } 
    });
    
    useEffect(()=>{
        getMovie();
        getProjection();
    },[]);

    const getMovie = () => {
        var id = window.location.pathname.split("/")[4];
        const requestOptions = {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
          },
        };
    
        fetch(`${serviceConfig.baseURL}/api/movies/getbyid/${id}`, requestOptions)
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
                movie: data 
              }));
            }
          })
          .catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            
          });
      };
      
      const getProjection = () => {
        var projectionId = window.location.pathname.split("/")[3];
        
        const requestOptions = {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          },
        };
        fetch(`${serviceConfig.baseURL}/api/projections/byprojectionid/` + projectionId, requestOptions)
          .then((response) => {
            if (!response.status) {
              return Promise.reject(response);
            }
            return response.json();
          })
          .then((data) => {
            if (data){
              setState((prev)=>({
                ...prev,
                projections: data 
              }));
              props.setInfo((prev)=>({
                ...prev,
                projectionPrice: data.price
              }));
    
              props.getSeatData(data.auditoriumId,projectionId);
              
            }
          })
          .catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            
          });
          
      };

    return(
          <Row className="justify-content-center align-items-center">
          <Col md={3} className="justify-content-center ">
          <img className="img-responsive img-fluid shadow rounded"  src={state.movie.coverPicture} />
          </Col>
          <Col sm={9}>
          <Card.Title>
            <span className="card-title-font">{state.movie.title}</span>
            <span className="float-right">
              {getRoundedRating(state.movie.rating)}
            </span>
          </Card.Title>
          <hr />
          <Card.Subtitle className="mb-2 text-muted">
            Date: {state.projections.projectionTime.slice(0, 10)}
            <span className="float-right">
              Cinema: {state.projections.cinemaName}
            </span>
          </Card.Subtitle>
          <Card.Subtitle className="mb-2 mt-3 text-muted">
            Time: {state.projections.projectionTime.slice(11, 16)}h
            <span className="float-right">
            Auditorium: {state.projections.auditoriumName}
            </span>
          </Card.Subtitle>
          <hr />
          </Col>
         
        </Row>
        
    );
};

export default ProjectionDeatils;
import React, { useEffect, useState , useContext} from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import {ProjectionContext} from "./ProjectionProvider";

const FilterProjections : React.FC = () =>{
    
    const context = useContext(ProjectionContext);
    const state = context[0];
    const setState = context[1];


    const getAuditoriumsBySelectedCinema = (selectedCinemaId: string) => {
        setState({ ...state, cinemaId: selectedCinemaId });
    
        const requestOptions = {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
          },
        };
    
        setState({ ...state, isLoading: true });
        fetch(
          `${serviceConfig.baseURL}/api/Auditoriums/bycinemaid/${selectedCinemaId}`,
          requestOptions
        )
          .then((response) => {
            if (!response.ok) {
              return Promise.reject(response);
            }
            return response.json();
          })
          .then((data) => {
            if (data) {
              setState({
                ...state,
                filteredAuditoriums: data,
                isLoading: false,
                selectedCinema: true,
              });
            }
          })
          .catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            setState({ ...state, isLoading: false });
          });
      };

      const getMoviesBySelectedAuditorium = (selectedAuditoriumId: string) => {
        setState({ ...state, auditoriumId: selectedAuditoriumId });
        const requestOptions = {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
          },
        };
    
        setState({ ...state, isLoading: true });
        fetch(
          `${serviceConfig.baseURL}/api/Movies/byauditoriumid/${selectedAuditoriumId}`,
          requestOptions
        )
          .then((response) => {
            if (!response.ok) {
              return Promise.reject(response);
            }
            return response.json();
          })
          .then((data) => {
            if (data) {
              setState({
                ...state,
                filteredMovies: data,
                isLoading: false,
                selectedAuditorium: true,
              });
            }
          })
          .catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            setState({ ...state, isLoading: false });
          });
      };

      const fillFilterWithCinemas = () => {
        return state.cinemas.map((cinema) => {
          return (
            <option value={cinema.id} key={cinema.id}>
              {cinema.name}
            </option>
          );
        });
      };
    
      const fillFilterWithAuditoriums = () => {
          console.log("AUDITORIUM FUNKCIJA RENDER");
        if (state.selectedCinema) {
          return state.filteredAuditoriums.map((auditorium) => {
            return <option key={auditorium.id} value={auditorium.id}>{auditorium.name}</option>;
          });
        } else {
          return state.auditoriums.map((auditorium) => {
            return (
              <option   value={auditorium.id} key={auditorium.id}>
                {auditorium.name}
              </option>
            );
          });
        }
      };
    
    console.log("render FILTER");
    return(
        <form
        id="name"
        name={state.name}
        // onSubmit={handleSubmit}
        className="filter"
      >
        <span className="filter-heading">Filter by:</span>
        <select
          onChange={(e) => getAuditoriumsBySelectedCinema(e.target.value)}
          name="cinemaId"
          id="cinema"
          className="select-dropdown"
        >
          <option value="none">Cinema</option>
          {fillFilterWithCinemas()}
        </select>
        <select
          onChange={(e) => getMoviesBySelectedAuditorium(e.target.value)}
          name="auditoriumId"
          id="auditorium"
          className="select-dropdown"
          
        >
          <option value="none">Auditorium</option>
          {fillFilterWithAuditoriums()}
        </select>
        <select
        
          name="movieId"
          id="movie"
          className="select-dropdown"
          
        >
           {/* <option value="" selected disabled hidden>Movie</option>  */}
          <option value="none">Movie</option>
          {/* {fillFilterWithMovies()} */}
        </select>
        <input
        //   onChange={(e) =>
        //     // setState({ ...state, selectedDate: true, dateTime: e.target.value })
        //   }
          name="dateTime"
          type="date"
          id="date"
          className="input-date select-dropdown"
          disabled
        />
        <button
          id="filter-button"
          className="btn-search"
          type="submit"
          onClick={() => setState({ ...state, submitted: true })}
        >
          Submit
        </button>
      </form>
    );
};

export default FilterProjections;
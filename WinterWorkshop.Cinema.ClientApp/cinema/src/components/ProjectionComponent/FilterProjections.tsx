import React, { useEffect, useState , memo, useMemo} from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";

import SelectAuditoriums from "./SelectFilters/SelectAuditoriums"
import SelectCinenma from "./SelectFilters/SelectCinemas"
import SelectMovies from "./SelectFilters/SelectMovies"
import * as Service from "./ProjectionService"
import {IMovie, IProjection, IAuditorium,  ICinema} from "../../models";

import { classicNameResolver } from "typescript";

interface IState {

  dateTime: string;
  id: string;
  current: boolean;
  tag: string;
  titleError: string;
  yearError: string;
  submitted: boolean;
  isLoading: boolean;
  selectedCinema: boolean;
  selectedAuditorium: boolean;
  selectedMovie: boolean;
  selectedDate: boolean;
  date: Date;
  cinemaId: string;
  auditoriumId: string;
  movieId: string;
  name: string;
}

interface Props{
  movies: IMovie[],
  setMovies,
  info: IState,
  setInfo,
  handleSubmit
}

const FilterProjections = memo((props : Props) =>{
    
    const[cinemas,setCinemas]=useState({
      cinemas: [
        { id: "",
         name: "",
        }]
    });
    const[auditoriums,setAuditoriums]=useState({
      auditoriums: [
        {
          id: "",
          name: "",
        },
      ]
    });
    const[filteredData,setFilteredData]=useState({
      filteredAuditoriums: [
        {
          id: "",
          name: "",
        },
      ],
      filteredMovies: [
        {
          id: "",
          coverPicture: "",
          title: "",
          rating: 0,
          year: "",
        },
      ]
    });

    useEffect(() => {
      Service.getCurrentMoviesAndProjections(props.setInfo,props.setMovies);
      Service.getAllCinemas(props.setInfo,setCinemas);
      Service.getAllAuditoriums(props.setInfo,setAuditoriums);
    }, []);
    
    const infoCinema = useMemo(()=>props.info,[props.info.selectedCinema,props.info.selectedAuditorium]);

    console.log("render FILTER");
    return(
    
        <form
        id="name"
        name={props.info.name}
        onSubmit={props.handleSubmit}
        className="filter"
      >
        <span className="filter-heading">Filter by:</span>
        
        <SelectCinenma cinemas={cinemas.cinemas} setInfo={props.setInfo} setFilteredData={setFilteredData}/>

        <SelectAuditoriums selectedCinema={props.info.selectedCinema} selectedAuditoriumId={props.info.auditoriumId} filteredAuditoriums={filteredData.filteredAuditoriums} auditoriums={auditoriums.auditoriums} setInfo={props.setInfo} setFilteredData={setFilteredData}/>
        
        <SelectMovies info={infoCinema} setInfo={props.setInfo} filteredMovies={filteredData.filteredMovies} movies={props.movies}/>
        
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
          onClick={() => props.setInfo((prev)=>({ ...prev, submitted: true }))}
        >
          Submit
        </button>
      </form>
     
    );
});

export default FilterProjections;
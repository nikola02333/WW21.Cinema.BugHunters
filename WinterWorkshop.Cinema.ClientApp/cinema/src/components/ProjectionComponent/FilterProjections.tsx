import React, { useEffect, useState , memo, useMemo,Dispatch,SetStateAction} from "react";
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
import {IInfoState,IStateMovies} from "./Projections"
import { classicNameResolver } from "typescript";
interface IProps{
  movies: IMovie[];
  setMovies: Dispatch<SetStateAction<IStateMovies>>;
  info: IInfoState;
  setInfo: Dispatch<SetStateAction<IInfoState>>;
  handleSubmit : (event: React.FormEvent<HTMLFormElement>) => void;
}

interface IFilteredData{
  filteredAuditoriums:IAuditorium[];
  filteredMovies:IMovie[];
}

const FilterProjections:React.FC<IProps> = memo(({movies,setMovies,info,setInfo,handleSubmit}) =>{
    
    const[cinemas,setCinemas]=useState<{cinemas: ICinema[]}>({
      cinemas: [
        { id: "",
         name: "",
         address:"",
        cityName:"",
        }]
    });
    const[auditoriums,setAuditoriums]=useState<{auditoriums: IAuditorium[]}>({
      auditoriums: [
        {
          id: "",
          name: "",
          cinemaId: "",
          numberOfSeats: 0,
          seatRows: 0
        },
      ]
    });
    const[filteredData,setFilteredData]=useState<IFilteredData>({
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
          hasOscar:false,
        },
      ]
    });

    useEffect(() => {
      Service.getCurrentMoviesAndProjections(setInfo,setMovies);
      Service.getAllCinemas(setInfo,setCinemas);
      Service.getAllAuditoriums(setInfo,setAuditoriums);
    }, []);
    
    const infoCinema = useMemo(()=>info,[info.selectedCinema,info.selectedAuditorium]);

    console.log("render FILTER");
    return(
    
        <form
        id="name"
        name={info.name}
        onSubmit={handleSubmit}
        className="filter"
      >
        <span className="filter-heading">Filter by:</span>
        
        <SelectCinenma cinemas={cinemas.cinemas} setInfo={setInfo} setFilteredData={setFilteredData}/>

        <SelectAuditoriums selectedCinema={info.selectedCinema} selectedAuditoriumId={info.auditoriumId} filteredAuditoriums={filteredData.filteredAuditoriums} auditoriums={auditoriums.auditoriums} setInfo={setInfo} setFilteredData={setFilteredData}/>
        
        <SelectMovies info={infoCinema} setInfo={setInfo} filteredMovies={filteredData.filteredMovies} movies={movies}/>
        
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
          onClick={() => setInfo((prev)=>({ ...prev, submitted: true }))}
        >
          Submit
        </button>
      </form>
     
    );
});

export default FilterProjections;
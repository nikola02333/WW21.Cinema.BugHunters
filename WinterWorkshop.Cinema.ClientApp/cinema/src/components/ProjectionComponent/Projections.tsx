import React, { useEffect, useState,useMemo } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import FilterProjections from "./FilterProjections";
import MovieProjectCard from "./MovieProjectionCard";
import * as Service from "./ProjectionService"
export interface IStateMovies{
  movies:IMovie[];
}
interface IProjectionState{
  filteredProjections:IProjection[];
}

export interface IInfoState{
  cinemaId: string;
  auditoriumId: string;
  movieId: string;
  dateTime: Date;
  id: string;
  name: string;
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
}

const Projections : React.FC = (props: any) => {

  const [movies,setMovies]=useState<IStateMovies>({
    movies: []
  });

  const [projection,setProjection]=useState<IProjectionState>({
    filteredProjections: [],
  });

  const [info, setInfo] = useState<IInfoState>({
    cinemaId: "",
    auditoriumId: "",
    movieId: "",
    dateTime: new Date(0),
    id: "",
    name: "",
    current: false,
    tag: "",
    titleError: "",
    yearError: "",
    submitted: false,
    isLoading: true,
    selectedCinema: false,
    selectedAuditorium: false,
    selectedMovie: false,
    selectedDate: false,
  });

    useEffect(()=>{
      Service.getCurrentMoviesAndProjections(setInfo,setMovies);
    },[]);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
      
      event.preventDefault();
      const { cinemaId, auditoriumId, movieId, dateTime } = info;

      if (cinemaId || auditoriumId || movieId || ( dateTime && dateTime.getFullYear()!==1970) ) {
        Service.getCurrentFilteredMoviesAndProjections(info,setInfo,movies,setProjection);
      } else {
        if(!cinemaId && !auditoriumId && !movieId && ( !dateTime || dateTime.getFullYear()===1970) ){
          NotificationManager.info("All movies");
        }else{
          NotificationManager.error("Not found.");
        }
        
        setInfo((prev)=>({ ...prev, submitted: false }));
        setProjection((prev)=>({ ...prev,setProjection:[]  }));
      }
    };

    console.log("PROJECTIONS")

    const infoMemo = useMemo(()=>info.submitted,[info.submitted,projection.filteredProjections]);
    const moviesMemo = useMemo(()=>movies.movies,[movies.movies]);
    const filterProjectionsMemo = useMemo(()=>projection.filteredProjections,[projection.filteredProjections]);
   
    return (
        <Container>
          <Row className="justify-content-center">
          <h1 className="projections-title">Current projections</h1>
          </Row>
         
          <Row className="justify-content-center">
          <FilterProjections handleSubmit={handleSubmit} movies={movies.movies} setMovies={setMovies} info={info} setInfo={setInfo}/>
          </Row>
          <MovieProjectCard submitted={infoMemo} movies={moviesMemo} filteredProjections={filterProjectionsMemo}/>
          
        </Container>
      );
};

export default withRouter(Projections);
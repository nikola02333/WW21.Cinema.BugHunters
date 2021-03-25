import React, { useEffect, useState } from "react";
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
  dateTime: string;
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
  date: Date
}

const Projections : React.FC = (props: any) => {

  const [movies,setMovies]=useState<IStateMovies>({
    movies: [
      {
        id: "",
        coverPicture: "",
        title: "",
        rating: 0,
        year: "",
        projections: [
          {
            auditoriumId: 0,
            auditoriumName: "",
            duration: 0,
            id: "",
            movieId: "",
            movieTitle: "",
            price: 0,
            projectionTime: "",
          },
        ],
      },
    ]
  });

  const [projection,setProjection]=useState<IProjectionState>({
    filteredProjections: [
      {
        auditoriumId: 0,
        auditoriumName: "",
        duration: 0,
        id: "",
        movieId: "",
        movieTitle: "",
        price: 0,
        projectionTime: "",
      },
    ],
  });

  const [info, setInfo] = useState<IInfoState>({
    cinemaId: "",
    auditoriumId: "",
    movieId: "",
    dateTime: "",
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
    date: new Date()
  });

    useEffect(()=>{
      Service.getCurrentMoviesAndProjections(setInfo,setMovies);
    },[]);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      setInfo((prev)=>({ ...prev, submitted: true }));
      const { cinemaId, auditoriumId, movieId, dateTime } = info;
  
      if (cinemaId || auditoriumId || movieId || dateTime) {
        Service.getCurrentFilteredMoviesAndProjections(info,setInfo,movies,setProjection);
      } else {
        if(!cinemaId && !auditoriumId && !movieId ){
          NotificationManager.error("All movies");
        }else{
          NotificationManager.error("Not found.");
        }
        
        setInfo((prev)=>({ ...prev, submitted: false }));
        setProjection((prev)=>({ ...prev,setProjection:[]  }));
      }
    };

    console.log("PROJECTIONS")
    
    return (
        <Container>
          <h1 className="projections-title">Current projections</h1>
          <FilterProjections handleSubmit={handleSubmit} movies={movies.movies} setMovies={setMovies} info={info} setInfo={setInfo}/>
          <MovieProjectCard submitted={info.submitted} movies={movies.movies} filteredProjections={projection.filteredProjections}/>
          
        </Container>
      );
};

export default withRouter(Projections);
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



const Projections : React.FC = (props: any) => {

  const [movies,setMovies]=useState({
    movies: [
      {
        id: "",
        coverPicture: "",
        title: "",
        rating: 0,
        year: "",
        projections: [
          {
            id: "",
            movieId: "",
            projectionTime: "",
            auditoriumName: "",
          },
        ],
      },
    ]
  });

  const [projection,setProjection]=useState({
    filteredProjections: [
      {
        id: "",
        movieId: "",
        projectionTime: "",
        coverPicture: "",
        auditoriumName: "",
        movieTitle: "",
        movieRating: 0,
        movieYear: "",
      },
    ],
  });

  const [info, setInfo] = useState({
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
        console.log("PRETRAGA");
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
          <MovieProjectCard props={props} submitted={info.submitted} movies={movies.movies} filteredProjections={projection.filteredProjections}/>
          
        </Container>
      );
};

export default withRouter(Projections);
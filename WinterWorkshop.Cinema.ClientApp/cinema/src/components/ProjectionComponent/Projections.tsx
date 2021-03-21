import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import FilterProjections from "./FilterProjections";
import {getRoundedRating,navigateToProjectionDetails} from "./ProjectionFunctions"



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
        bannerUrl: "",
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
      getCurrentMoviesAndProjections();
    },[]);

    const getCurrentMoviesAndProjections = () => {
      setInfo((prev)=>({ ...prev, submitted: false }));
  
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
      };
  
      setInfo((prev)=>({ ...prev, isLoading: true }));
      fetch(
        `${serviceConfig.baseURL}/api/movies/AllMovies/${true}`,
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
            console.log(data)
            setInfo((prev)=>({ ...prev, movies: data, isLoading: false }));
            setMovies((prev)=>({ ...prev, movies: data }));
          }
          
        })
        .catch((response) => {
          setInfo((prev)=>({ ...prev, isLoading: false }));
          NotificationManager.error(response.message || response.statusText);
        });
    };

    const fillTableWithData = () => {
      return movies.movies.map((movie) => {
        const projectionButton = movie.projections?.map((projection) => {
          return (
            <Button
              key={projection.id}
              onClick={() => navigateToProjectionDetails(projection.id, movie.id,props)}
              className="btn-projection-time"
            >
              {projection.projectionTime.slice(11, 16)}h
            </Button>
          );
        });
  
        return (
          <Container key={movie.id} className="shadow rounded mt-1">
          <Row >
            <Col md={4} className=" rounded mt-3">
            <img className="img-responsive img-fluid" style={{  borderRadius:5}} src={movie.coverPicture} />
            </Col>
            <Col md={8} className=" mt-3">
              <Col md={12}>
              <span className="card-title-font">{movie.title}</span>
               {getRoundedRating(movie.rating)}
              </Col>

              <Col md={12} className="mb-2 text-muted">
              Year of production: {movie.year}
              </Col>
            </Col>
          </Row>
          <Row className="mt-2 mb-5">
            <Col md={12} className="pb-5">
            <span className="mb-2 font-weight-bold mr-2">Projection times:</span>
            {projectionButton}
            </Col>
          </Row>
          </Container>
        );
      });
    };

    const fillTableWithFilteredProjections = () => {
      return projection.filteredProjections.map((filteredProjection) => {
        return (
          <Card.Body key={filteredProjection.movieId}>
            <div className="banner-img">
              <img className="img-style" src={filteredProjection.bannerUrl}></img>
            </div>
            <Card.Title>
              <span className="card-title-font">
                {filteredProjection.movieTitle} -{" "}
                {filteredProjection.auditoriumName}
              </span>
              {filteredProjection.movieRating &&
                getRoundedRating(filteredProjection.movieRating)}
            </Card.Title>
            <hr />
            <Card.Subtitle className="mb-2 text-muted">
              Year of production: {filteredProjection.movieYear}
            </Card.Subtitle>
            <hr />
            <Card.Text>
              <span className="mb-2 font-weight-bold">Projection times:</span>
            </Card.Text>
            <Button
              key={filteredProjection.id}
              // onClick={() =>
              //   navigateToProjectionDetails(
              //     filteredProjection.id,
              //     filteredProjection.movieId
              //   )
              // }
              className="btn-projection-time"
            >
              {filteredProjection.projectionTime.slice(11, 16)}h
            </Button>
          </Card.Body>
        );
      });
    };
    console.log("PROJECTIONS")
    const checkIfFiltered = () => {
      if (info.submitted) {
        return fillTableWithFilteredProjections();
      } else {
        return fillTableWithData();
      }
    };
    return (
        <Container>
          <h1 className="projections-title">Current projections</h1>
          <FilterProjections movies={movies.movies} setMovies={setMovies} info={info} setInfo={setInfo}/>
          {checkIfFiltered()}
        </Container>
      );
};

export default withRouter(Projections);
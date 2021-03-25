import React,{useEffect, useState} from 'react'
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import {getRoundedRating,navigateToProjectionDetails} from "./ProjectionFunctions"
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import {useHistory} from "react-router-dom";


interface IProps{
  submitted: boolean;
  movies: IMovie[];
  filteredProjections:IProjection[];
}

const MovieProjectCard:React.FC<IProps> = ({submitted,movies,filteredProjections}) =>{
  const history =useHistory();

  const [state,setState]=useState({
    projection:[]
  });
  useEffect(() => {
    const objectMap = filteredProjections.reduce(function(o, i) {
      if (!o[i.movieId]) o[i.movieId] = [];
      o[i.movieId].push(i)
  
      return o;
  },{});
  console.log(objectMap);
  }, [filteredProjections])
    
  const fillTableWithData = () => {
        return movies.map((movie) => {
          const projectionButton = movie.projections?.map((projection) => {
            return (
              <Button
                key={projection.id}
                onClick={() => navigateToProjectionDetails(projection.id, movie.id,history)}
                className="btn-projection-time"
              >
                {projection.projectionTime.slice(11, 16)}h
              </Button>
            );
          });
    
          return (
            <Container key={movie.id} className="shadow rounded mt-1">
            <Row >
              <Col md={3} className=" rounded mt-3">
              <img className="img-responsive img-fluid" style={{  borderRadius:5}} src={movie.coverPicture} />
              </Col>
              <Col md={9} className=" mt-3">
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
      return filteredProjections.map((filteredProjection) => {
        return (
          <Container key={filteredProjection.id} className="shadow rounded mt-1">
          <Row >
            <Col md={3} className=" rounded mt-3">
            <img className="img-responsive img-fluid" style={{  borderRadius:5}} src={filteredProjection.coverPicture} />
            </Col>
            <Col md={9} className=" mt-3">
              <Col md={12}>
              <span className="card-title-font">{filteredProjection.movieTitle}
              </span>
              {filteredProjection.movieRating &&
              getRoundedRating(filteredProjection.movieRating)}
              </Col>

              <Col md={12} className="mb-2 text-muted">
              Year of production: {filteredProjection.movieYear}
              </Col>
            </Col>
          </Row>
          <Row className="mt-2 mb-5">
            <Col md={12} className="pb-5">
            <span className="mb-2 font-weight-bold mr-2">{filteredProjection.auditoriumName}</span>
            
            <span className="mb-2 font-weight-bold mr-2">Projection times:</span>
            <Button
              key={filteredProjection.id}
              onClick={() =>
                navigateToProjectionDetails(
                  filteredProjection.id,
                  filteredProjection.movieId,
                  history
                )
              }
              className="btn-projection-time"
            >
              {filteredProjection.projectionTime.slice(11, 16)}h
            </Button>
            </Col>
          </Row>
          </Container>
          
        );
      });
    };

    const checkIfFiltered = () => {
      if (submitted) {
        return fillTableWithFilteredProjections();
      } else {
        return fillTableWithData();
      }
    };

    return(
        <div>{checkIfFiltered()}</div>
    );

}
export default MovieProjectCard;
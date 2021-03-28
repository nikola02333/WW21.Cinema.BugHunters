import React,{useEffect, useState,memo,useMemo,useCallback} from 'react'
import { Container, Row, Col, Card, Button ,Image,Table,OverlayTrigger,Tooltip} from "react-bootstrap";
import {getRoundedRating,navigateToProjectionDetails} from "./ProjectionFunctions"
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import {useHistory} from "react-router-dom";
import _, { map } from 'underscore';
import Projection from '../user/Projection';


interface IProps{
  submitted: boolean;
  movies: IMovie[];
  filteredProjections:IProjection[];
}
export interface IProjectionByCinemaId {
  cinema: IProjection[];
}
// export interface IProjectionByAuditoriumId {
//   auditorium: IProjection[];
// }
interface IState{
  movieId:string;
  projections:IProjectionByCinemaId[];
}

const MovieProjectCard:React.FC<IProps> = memo(({submitted,movies,filteredProjections}) =>{
  const history =useHistory();

  const [state,setState]=useState<IState[]>([]);

  const filter = () => {
    var grouped = _.chain(filteredProjections).groupBy("movieId").map(function(offers, movieId) {
      // Optionally remove product_id from each record
       var cleanOffers = _.map(offers, function(it) {
        return _.omit(it, "movieId");
      });
    var groupedAud=  _.chain(cleanOffers).groupBy("auditoriumId").map(function(offers, auditoriumId) {
       var auditorium = _.map(offers);
          return {auditorium};
       }).value();
    
      return {
        movieId: movieId,
        projections: groupedAud
      };
    }).value();
    return grouped;
  };
    
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
            <Container key={movie.id} className="shadow rounded my-3">
            <Row >
              <Col md={3} className="  my-3">
              <Image  className=" img-responsive img-fluid" style={{  borderRadius:5}} src={movie.coverPicture} />
              </Col>
              <Col md={9} className=" my-3">
                <Col md={12}>
                <span className="card-title-font">{movie.title}</span>
                 {getRoundedRating(movie.rating)}
                </Col>
  
                <Col md={12} className="mb-2 text-muted">
                Year of production: {movie.year}
                </Col>
              </Col>
            </Row>
            </Container>
          );
        });
      };

    

    const fill = () => {
      // if(filteredProjections.length===0){
      //   return(
      //     <></>
      //   );
      // }
      console.log(filteredProjections);
      var grouped = _.chain(filteredProjections).groupBy("movieId").map(function(offers, movieId) {
          var cleanOffers = _.map(offers, function(it) {
          return _.omit(it, "movieId");
        });
        var groupedCinema=  _.chain(cleanOffers).groupBy("cinemaId").map(function(offers, cinemaId) {
          var cinema = _.map(offers);
            return {cinema};
          }).value();
      
      
        return {
          movieId: movieId,
          projections: groupedCinema
        };
      }).value();
      console.log(grouped);
      return grouped.map((filteredProjection: IState)  => {
      var movie = movies.find(x => x.id === filteredProjection.movieId) as IMovie;
      const projectionButton = filteredProjection.projections.map((projectio,index) => {
        
      const bottonsCinema= projectio.cinema.map((cinema,index)=>{
            if(index===0){
              return (<>
                <Col xs={2} className={"mx-1   justify-content-start"}>
              <span className="font-weight-bold">{cinema.cinemaName}</span> 
                </Col>
                <Col xs={1} className={"mx-1  p-0 justify-content-start"}>
                <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">
                  {cinema.projectionTime.slice(0, 10)} {" "+cinema.auditoriumName}</Tooltip>}>
                <Button
                  key={cinema.id}
                  onClick={() => navigateToProjectionDetails(cinema.id, movie.id,history)}
                  className=" btn-sm"
                >
                  {cinema.projectionTime.slice(11, 16)}h
                </Button>
                </OverlayTrigger>
                </Col>
                
                </>
              );
            }
              return (
                <Col xs={1} className={"mx-1  p-0 justify-content-start"}>
                  <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">
                    {cinema.projectionTime.slice(0, 10)} {" "+cinema.auditoriumName}</Tooltip>}>
                    <Button
                    key={cinema.id}
                    onClick={() => navigateToProjectionDetails(cinema.id, movie.id,history)}
                    className="btn-sm">
                      {cinema.projectionTime.slice(11, 16)}h
                    </Button>
                  </OverlayTrigger>
                </Col>
              );
            })
        return (
              <Row key={movie.id+index} className="justify-content-start  my-2 mb-3">{bottonsCinema}</Row>
        );
      });


          return(
          <Container key={filteredProjection.movieId} className="shadow rounded my-3">
            <Row >
              <Col md={3} className="justify-content-start my-3">
              <img className="img-responsive img-fluid" style={{  borderRadius:5}} src={movie.coverPicture} />
              </Col>
              <Col md={9} className=" my-3">
                <Col md={12}>
                <span className="card-title-font">{movie.title}
                </span>
                {movie.rating &&
                getRoundedRating(movie.rating)}
                </Col>

                <Col md={12} className="mb-2 text-muted">
                Year of production: {movie.year}
                </Col>
              </Col>
            </Row>
            <Row >
              <Col md={12} className="mb-2">
              <span className="p-0 font-weight-bold ">Projection times:</span> 
              </Col>
            </Row> 
            <Row>
              <Col xs={12}>
                {projectionButton}
              </Col>
            </Row>
          </Container>
          )
        }
        )};

    const checkIfFiltered = () => {
      console.log(submitted);
      if (submitted) {
        return fill();
      } else {
        return fillTableWithData();
      }
    };

    return(
        <div>{checkIfFiltered()}</div>
    );

});
export default MovieProjectCard;


                
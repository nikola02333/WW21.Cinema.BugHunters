import React,{useEffect, useState,memo,useMemo,useCallback} from 'react'
import { Container, Row, Col, Card, Button ,Image,Table,OverlayTrigger,Tooltip} from "react-bootstrap";
import {getRoundedRating,navigateToProjectionDetails} from "./ProjectionFunctions"
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";
import {useHistory} from "react-router-dom";
import _, { map } from 'underscore';


interface IProps{
  submitted: boolean;
  movies: IMovie[];
  filteredProjections:IProjection[];
}
export interface IProjectionByAuditoriumId {
  auditorium: IProjection[];
}
interface IState{
  movieId:string;
  projections:IProjectionByAuditoriumId[];
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
  const filterCallBack = useCallback(() => filter(), [state]);
    
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
            {/* <Row className="mt-2 mb-5">
              <Col md={12} className="pb-5">
              <span className="mb-2 font-weight-bold mr-2">Projection times:</span>
              {projectionButton}
              </Col>
            </Row> */}
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
      
      return grouped.map((filteredProjection: IState)  => {
      var movie = movies.find(x => x.id === filteredProjection.movieId) as IMovie;
      console.log(movie.title);
      
      const projectionButton = filteredProjection.projections.map((projectio) => {
      
      const bottonsAudit= projectio.auditorium.map((audit,index)=>{
            if(index===0){
              return (<>
                <Col xs={1} className={"mx-3"}>
              <span className="font-weight-bold">{audit.auditoriumName}</span> 
                </Col>
                <Col xs={1} className={"mx-3"}>
                <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">{audit.projectionTime.slice(0, 10)}</Tooltip>}>
                <Button
                  key={audit.id}
                  onClick={() => navigateToProjectionDetails(audit.id, movie.id,history)}
                  className="btn-projection-time my-2"
                >
                  {audit.projectionTime.slice(11, 16)}h
                </Button>
                </OverlayTrigger>
                </Col>
                
                </>
              );
            }
              return (
                <Col xs={1} className={"mx-3"}>
                  <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">{audit.projectionTime.slice(0, 10)}</Tooltip>}>
                    <Button
                    key={audit.id}
                    onClick={() => navigateToProjectionDetails(audit.id, movie.id,history)}
                    className="btn-projection-time my-2">
                      {audit.projectionTime.slice(11, 16)}h
                    </Button>
                  </OverlayTrigger>
                </Col>
              );
            })
        return (
              <Row className="justify-content-start my-2">{bottonsAudit}</Row>
        );
      });


          return(
          <Container key={filteredProjection.movieId} className="shadow rounded my-3">
            <Row >
              <Col md={3} className="justify-content-start mt-3">
              <img className="img-responsive img-fluid" style={{  borderRadius:5}} src={movie.coverPicture} />
              </Col>
              <Col md={9} className=" mt-3">
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
              <Row className=" ">
              <Col md={12} className="py-3">
              <span className="my-3 font-weight-bold ">Projection times:</span> 
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


                
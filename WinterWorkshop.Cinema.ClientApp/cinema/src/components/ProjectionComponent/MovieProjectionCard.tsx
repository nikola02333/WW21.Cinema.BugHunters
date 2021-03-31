import React,{memo} from 'react'
import { Container, Row, Col, Button ,Image,Table,OverlayTrigger,Tooltip} from "react-bootstrap";
import {getRoundedRating,navigateToProjectionDetails} from "./ProjectionFunctions"
import {  IProjection, IMovie } from "../../models";
import {useHistory} from "react-router-dom";
import _, { map } from 'underscore';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faStar } from '@fortawesome/free-solid-svg-icons'
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons'




interface IProps{
  submitted: boolean;
  movies: IMovie[];
  currantProjections:IProjection[];
  filteredProjections:IProjection[];
}
export interface IProjectionByCinemaId {
  cinema: IProjection[];
}

interface IState{
  movieId:string;
  projections:IProjectionByCinemaId[];
}

const MovieProjectCard:React.FC<IProps> = memo(({submitted,movies,filteredProjections,currantProjections}) =>{
  const history =useHistory();

    const showProjections = (projections:IProjection[]) => {
     
      var grouped = _.chain(projections).groupBy("movieId").map(function(offers, movieId) {
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
      return grouped.map((filteredProjection: IState,indexTop)  => {
      var movie = movies.find(x => x.id === filteredProjection.movieId) as IMovie;
      if(movie===undefined){
        return;
      }

      const projectionButton = filteredProjection.projections.map((projection,index) => {
        
      const bottonsCinema= projection.cinema.map((cinema,index)=>{
            if(index===0){
              return (<React.Fragment key={Math.floor(Math.random() * 10000)}>
                <Col xs={2} className={"mx-1   justify-content-start"}>
              <span className="font-weight-bold">{cinema.cinemaName}</span> 
                </Col>
                <Col xs={1} className={"mx-1  p-0 justify-content-start"}>
                <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">
                  {cinema.price+"rsd "+cinema.auditoriumName}</Tooltip>}>
                <Button
                  key={cinema.id}
                  onClick={() => navigateToProjectionDetails(cinema.id, movie.id,history)}
                  className=" btn-sm"
                >
                  {cinema.projectionTime.slice(5, 10)+"  "+cinema.projectionTime.slice(11, 16)}h
                </Button>
                </OverlayTrigger>
                </Col>
                
                </React.Fragment>
              );
            }
              return (
                <Col key={Math.floor(Math.random() * 10000)} xs={1} className={"mx-1  p-0 justify-content-start"}>
                  <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">
                  {cinema.price+"rsd "+cinema.auditoriumName}</Tooltip>}>
                    <Button
                    key={cinema.id}
                    onClick={() => navigateToProjectionDetails(cinema.id, movie.id,history)}
                    className="btn-sm">
                      { cinema.projectionTime.slice(5, 10)+"  "+cinema.projectionTime.slice(11, 16)}h
                    </Button>
                  </OverlayTrigger>
                </Col>
              );
            })
        return (<React.Fragment key={Math.floor(Math.random() * 10000)}>
              <Row key={Math.floor(Math.random() * 10000)} className="justify-content-start  my-2 mb-3">{bottonsCinema}</Row>
              <hr/></React.Fragment>
        );
      });
     
          return(
          <Container key={filteredProjection.movieId} className="shadow rounded my-3">
            <Row className="">
              <Col md={4} className="justify-content-center my-3">
                
              <img onClick={() => history.push(`MovieDeatails/${movie.id}`)} className=" img-fluid" style={{  borderRadius:5}} src={movie.coverPicture} />
              </Col>
              <Col  className=" my-3">
                  <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Name:</span>
                    </Col>
                    <Col >
                    <span className="movie-details">{movie.title}</span>
                    </Col>
                </Row >
               
                <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Genre:</span>
                    </Col>
                    <Col>
                    <span className="movie-details">{movie.tagsModel?.map(tag=> { if(tag.tagName==="Genre") return tag.tagValue+" "}).join('  ')}</span>
                    </Col>
                </Row>

                <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Actors:</span>
                    </Col>
                    <Col>
                    <span className="movie-details-text pt-auto">{movie.tagsModel?.map(tag=> { if(tag.tagName==="Actor") return tag.tagValue+","}).join('  ')}</span>
                    </Col>
                </Row >
                <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Runtime:</span>
                    </Col>
                    <Col>
                    <span className="movie-details-text">{projections[0].duration+" min"}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Year:</span>
                    </Col>
                    <Col>
                    <span className="movie-details-text">{movie.year}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Awards:</span>
                    </Col>
                    <Col>
                    <span className="movie-details-text">{movie.hasOscar?"Oscar":"No awards"}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={2}>
                    <span className="font-weight-bold movie-details">Rating:</span>
                    </Col>
                    <Col>
                    <span className=""><FontAwesomeIcon icon={faStar} className="text-warning"/>{movie.rating+"/10"}</span>
                    </Col>
                    
                </Row>
                <Row className="mt-3">
                  <Col xs={2} className="d-flex justify-content-start ">
                      <Button variant="outline-info" onClick={() => history.push(`MovieDeatails/${movie.id}`)}>More</Button>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row >
              <Col xs={12} >
              <span className="p-0 font-weight-bold ">Projection times:</span> 
              </Col>
            </Row>
            <hr/> 
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
      if (submitted) {
        return showProjections(filteredProjections);
      } else {
        return showProjections(currantProjections);
      }
    };

    return(
        <div>{checkIfFiltered()}</div>
    );

});
export default MovieProjectCard;


                
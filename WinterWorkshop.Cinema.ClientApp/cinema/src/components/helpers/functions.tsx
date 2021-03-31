import React from 'react'
import {IMovie, IProjection} from "../../models/index"
import _, { map } from 'underscore';
import { Row, Col,  Button ,OverlayTrigger,Tooltip} from "react-bootstrap";

export interface IProjectionByCinemaId {
  cinema: IProjection[];
}
export interface IGroupedProjections{
  movieId:string;
  projections:IProjectionByCinemaId[];
}

export const getRoundedRating = (rating: number) => {
    const result = rating;
    return <span className="float-right" >Rating: {result}/10</span>;
  };

export const groupProjectionButton = (projections:IProjection[]) => {
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
    return grouped;
  };

  export const navigateToProjectionDetails = (id: string, movieId: string,history) => {
    history.push(`TicketReservation/${id}/${movieId}`);
  };

  export const groupedProjectionButtons=(filteredProjection :IGroupedProjections, movie: IMovie, history)=>{
    
    if(_.isEmpty(filteredProjection)){
      return(<></>);
    }

    debugger
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
                onClick={() => navigateToProjectionDetails(cinema.id, movie.id ,history)}
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
                  onClick={() => navigateToProjectionDetails(cinema.id, movie.id ,history)}
                  className="btn-sm">
                    {cinema.projectionTime.slice(11, 16)}h
                  </Button>
                </OverlayTrigger>
              </Col>
            );
          })
      return (<>
            <Row className="justify-content-start  my-2 mb-3">{bottonsCinema}</Row>
            </>
      );
    
  });
};
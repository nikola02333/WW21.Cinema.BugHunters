import React from 'react'
import {groupProjectionButton,groupedProjectionButtons} from "../helpers/functions"
import {Col,Row, OverlayTrigger,Button, Tooltip} from "react-bootstrap";
import {IMovie, IProjection,IGroupedProjections} from "../../models/index"
import _, { map } from 'underscore';


interface IProps{
    groupedProjections:IGroupedProjections;
    movie:IMovie;
    history
}


const MovieProjectionButtons : React.FC<IProps> =({groupedProjections,movie,history})=>{
    
    const buttons=(groupedProjections, movie, history)=>{
        if(groupedProjections!=={} && movie!=={} ){
            return(
    <React.Fragment>
            {groupedProjectionButtons(groupedProjections, movie, history)}
            </React.Fragment>
            );
            
        }else{
            return (
                <React.Fragment></React.Fragment>
            );
        }
    }
    const navigateToProjectionDetails = (id: string, movieId: string,history) => {
        history.push(`TicketReservation/${id}/${movieId}`);
      };
    

    const groupedProjectionButtons=(filteredProjection :IGroupedProjections, movie: IMovie, history)=>{
    
        if(_.isEmpty(filteredProjection)){
          return(<></>);
        }
    
       
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
        return(
            <React.Fragment>{projectionButton}</React.Fragment>
        )
      });
    };

    return(
        <Col xs={12}>
                    {buttons(groupedProjections, movie, history)}
        </Col>

    );
    
}
export default MovieProjectionButtons;
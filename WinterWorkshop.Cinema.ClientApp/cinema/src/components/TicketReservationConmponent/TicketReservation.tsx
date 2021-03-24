import React, {useMemo, useState,useEffect} from 'react'
import { Container ,Row,Col} from 'react-bootstrap';
import {
    ICurrentReservationSeats,
    IMovie,
    IProjection,
    IReservedSeats,
    ISeats,
    IProjectionNEW,
  } from "../../models";
import ProjectionDetails from "./ProjectionDetails";
import * as Service from "../Services/SeatsService";
import ShowAuditorium from "./ShowAuditorium";
import {getUserByUsername} from "../Services/userService";
import InfoTable from "./InfoTable";
import {tryReservation} from "../Services/ReservationService";
interface IcurrentReservationSeats{
  currentSeatId:string
}

const TicketReservation:React.FC = (props) =>{
    const [seat,setSeat] = useState({
        maxRow: 0,
        maxNumberOfRow: 0,
        inc: 0,
        seats: [],
        reservedSeats: [
            {
              auditoriumId: 0,
              id: "",
              number: 0,
              row: 0,
            },
        ],
        currentReservationSeats:[],
        clicked: false,
    });
    const [info, setInfo] = useState({
        
        inciD: 0,
        userId: "",
        submitted: false,
        projectionPrice:0
      });

    useEffect(()=>{
      getUserByUsername(setInfo);
    },[]);
 
     function getSeatData(auditoriumId,projectionId){
       Service.getReservedSeats(projectionId,setSeat);
       Service.getSeatsForAuditorium(auditoriumId,setSeat);
       Service.getSeats(auditoriumId,setSeat);
    };

    function tryReservationF(e){
      e.persist();
      tryReservation(e,seat,info);
    };

    return(
        <Container  className="d-flex justify-content-center mt-5 ">
            <Col  xs={12} className="shadow ">
                <Row className="justify-content-center my-2">
                    <Col md={10} className="mt-2">
                        <ProjectionDetails setInfo={setInfo} getSeatData={getSeatData}/>
                    </Col>
                </Row>
                <Row className="justify-content-center ">
                    <h4 className="mt-3 text-center">Choose your seat(s):</h4>
                </Row>
                <Row >
                <InfoTable currentReservationSeats={seat.currentReservationSeats} projectionPrice={info.projectionPrice}/>
                </Row>
                <Row className="justify-content-center my-2">
                <ShowAuditorium tryReservation={tryReservationF} seats={seat} setSeat={setSeat} />
                </Row>
                
            </Col>
        </Container>
    );
}

export default TicketReservation;
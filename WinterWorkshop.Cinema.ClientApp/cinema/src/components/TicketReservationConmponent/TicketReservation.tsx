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
import {getUserByUsernameReservatino} from "../Services/userService";
import InfoTable from "./InfoTable";
import {tryReservation} from "../Services/ReservationService";
import {reservationService} from "../Services/reservationServices";

export interface IStateTicketReservation{
  maxRow:number,
  maxNumberOfRow:number,
  reservedSeats:IReservedSeats[],
  currentReservationSeats: ICurrentReservationSeats[],
  seats:ISeats[]
}
interface IInfo{
  userId: string,
  submitted: boolean,
  projectionPrice:number
}

const TicketReservation:React.FC = () =>{
    const [seat,setSeat] = useState<IStateTicketReservation>({
        maxRow: 0,
        maxNumberOfRow: 0,
        seats: [],
        reservedSeats: [
            {
              auditoriumId: 0,
              id: "",
              number: 0,
              row: 0,
            },
        ],
        currentReservationSeats:[]
    });
    const [info, setInfo] = useState<IInfo>({
        userId: "",
        submitted: false,
        projectionPrice:0
      });

    useEffect(()=>{
      getUserByUsernameReservatino(setInfo);
    },[]);
    
 
     function getSeatData(auditoriumId,projectionId){
       Service.getReservedSeats(projectionId,setSeat);
       Service.getSeatsForAuditorium(auditoriumId,setSeat);
       Service.getSeats(auditoriumId,setSeat);
    };

    const tryReservationF=(e)=>{
      e.persist();
      reservationService.tryReservationn(e,seat,info);

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
                <ShowAuditorium tryReservation={tryReservationF} info={info} seat={seat} setSeat={setSeat} />
                </Row>
                
            </Col>
        </Container>
    );
}

export default TicketReservation;
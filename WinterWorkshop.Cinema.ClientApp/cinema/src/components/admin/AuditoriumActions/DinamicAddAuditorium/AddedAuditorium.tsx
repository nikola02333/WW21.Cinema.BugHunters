import React, { useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup,Button, Row, Col } from "react-bootstrap";
import { ICreateAuditoriumWithCinema} from './../../../../models/AuditoriumModels';


interface IProps{
    removeAuditorium: (auditoriumId) =>void;
  auditoriums :ICreateAuditoriumWithCinema[];
}


const AddedAuditoriums = ({ auditoriums, removeAuditorium }:IProps) => {
 
const toReturn =  auditoriums.map((auditorium, index) => (
   <Row key={index} className="mt-2 justify-content-center">
    <Col xs={9} key={auditorium.id} className="d-flex justify-content-center">
        
        <FormControl
            placeholder="Add tag"
            value={auditorium.auditoriumName}
            name="text"
            disabled
            autoComplete="off"
            className="col-8 mx-1"
        />
       
        <FormControl
            placeholder="Number of seats"
            value={auditorium.numberOfSeats}
            name="text"
            disabled
            className="col-2 mx-1"
            autoComplete="off"
        />
       
        <FormControl
            placeholder="Number of rows"
            value={auditorium.seatRows}
            name="text"
            disabled
            className="col-2 mx-1"
            autoComplete="off"
        />
        
    </Col>
    <Col xs={2} className="icons col-xs-1">
      <Button variant="danger" onClick={() => removeAuditorium(auditorium.id)}>
      <FontAwesomeIcon icon={faTrash} />
      </Button>
    </Col>
  </Row>
  ));

  return(<>{toReturn}</>);
};

export default AddedAuditoriums;

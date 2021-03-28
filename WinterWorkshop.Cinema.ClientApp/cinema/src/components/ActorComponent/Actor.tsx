import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { IActor } from './../../models/IActor';
import { FormControl, FormGroup,Button, Row, Col } from "react-bootstrap";

interface IProps{
  removeActor: (actorId) =>void;
  actors :IActor[];
}


const Actor = ({ actors, removeActor }:IProps) => {

   
const toReturn =  actors.map((actor, index) => (

    <Row key={index} className="mt-2">
    <Col xs={10} key={actor.id} className="d-flex justify-content-center">
    <FormControl
            placeholder="Add tag"
            value={actor.name}
            name="text"
            disabled
            autoComplete="off"
        />
    </Col>
    <Col xs={2} className="icons col-xs-1">
      <Button variant="danger" onClick={() => removeActor(actor.id)}>
      <FontAwesomeIcon icon={faTrash} />
      </Button>
    </Col>
  </Row>
  ));

  return(<>{toReturn}</>);
};

export default Actor;

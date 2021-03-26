import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { IActor } from './../../models/IActor';


interface IProps{
  removeActor: (actorId) =>void;
  actors :IActor[];
}


const Actor = ({ actors, removeActor }:IProps) => {

   
const toReturn =  actors.map((actor, index) => (
   <div key={index}>
    <div key={actor.id}>{actor.name}</div>
      <div   onClick={() => removeActor(actor.id)}>
      <FontAwesomeIcon icon={faTrash} />
      </div>
  </div>
  ));

  return(<>{toReturn}</>);
};

export default Actor;

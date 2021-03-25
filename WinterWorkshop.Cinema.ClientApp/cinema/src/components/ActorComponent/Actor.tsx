import React, { useState } from "react";
import TodoForm from "./ActorForm";
//import { RiCloseCircleLine } from "react-icons/ri";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { IActor } from './../../models/IActor';


interface IProps{
  removeActor: (actorId) =>void;
  actors :IActor[];
}


const Actor = ({ actors, removeActor }:IProps) => {

   
const toReturn =  actors.map((actor, index) => (
   <div className={"todo-row"} key={index}>
    <div key={actor.id}>{actor.name}</div>
    <div className="icons">
      <div>
      <FontAwesomeIcon icon={faTrash} />
      </div>
      <div
        onClick={() => removeActor(actor.id)}
        className="delete-icon"
      />
   
    </div>
  </div>
  ));

  return(<>{toReturn}</>);
};

export default Actor;

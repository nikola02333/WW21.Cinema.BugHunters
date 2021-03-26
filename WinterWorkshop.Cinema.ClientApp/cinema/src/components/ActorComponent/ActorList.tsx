import React, { useState } from "react";
import ActorForm from "./ActorForm";
import Actor from "./Actor";

import { IActor } from './../../models/IActor';


 interface IProps{
   
  //actors
  actors :IActor[];
  setState :any;
 }
const ActorList :React.FC<IProps> = (props) => {
  const [actors, setActors] = useState <IActor[]> (
   props.actors
  );

  const addActor = (actor) => {
    
    if (!actor.name || /^\s*$/.test(actor.name)) {
      return;
    }
    const newActors = [...actors,actor ];
    setActors(newActors);
    props.setState(prevState=> ({...prevState,Actorss: newActors}));

  };

 
  const removeActor = (id) => {
    const removedArr = [...actors].filter((actor) => actor.id !== id);

    setActors(removedArr);
    props.setState(prevState=> ({...prevState, Actorss: removedArr}));
  };

  return (
    <>
      <ActorForm  onSubmit={addActor} />
      <Actor actors={actors} removeActor={removeActor} />
    </>
  );
};

export default ActorList;

import React, { useState } from "react";

import { FormGroup, Row } from "react-bootstrap";
import { ICreateAuditoriumWithCinema } from "../../../../models/AuditoriumModels";
import AuditoriumForm from "./AuditoriumForm"
import AddedAuditorium from "./AddedAuditorium"


 interface IProps{
   
  auditoriums :ICreateAuditoriumWithCinema[];
  setState :any;
 }
const DinamicAddAuditorium :React.FC<IProps> = (props) => {
  const [auditoriums, setAuditoroums] = useState <ICreateAuditoriumWithCinema[]> (
   props.auditoriums
  );

  const addAuditorium = (auditorium:ICreateAuditoriumWithCinema) => {
    
    if (!auditorium.auditoriumName || /^\s*$/.test(auditorium.auditoriumName) || !auditorium.numberOfSeats || auditorium.numberOfSeats<1 || !auditorium.seatRows || auditorium.seatRows<1) {
      return;
    }
    const newAuditoriums = [...auditoriums,auditorium ];
    setAuditoroums(newAuditoriums);
    props.setState(prevState=> ({...prevState,auditoriums: newAuditoriums}));

    console.log(...newAuditoriums);
  };

  const removeAuditorium = (id) => {
    const removedArr = [...auditoriums].filter((todo) => todo.id !== id);
    setAuditoroums(removedArr);
    props.setState(prevState=> ({...prevState, auditoriums: removedArr}));
  };

  return (
    <>
     <FormGroup className="mt-3">
       <Row>
       <AuditoriumForm  onSubmit={addAuditorium} />
       </Row>
     
       <AddedAuditorium  auditoriums={auditoriums} removeAuditorium={removeAuditorium} />
      
      </FormGroup>
    </>
  );
};

export default DinamicAddAuditorium;
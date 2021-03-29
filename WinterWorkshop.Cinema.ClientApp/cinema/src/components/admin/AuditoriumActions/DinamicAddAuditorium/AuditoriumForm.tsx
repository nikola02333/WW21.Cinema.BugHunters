import React, { useState} from "react";
import { FormControl,Button, Row, Col } from "react-bootstrap";

interface IState{
  auditoriumName: string;
  seatRows : number;
  numberOfSeats : number;
}

interface IProps{
  onSubmit(auditorium:any):void;
}

const AuditoriumForm :React.FC<IProps>= ({ onSubmit,...props}) => {
  const [input, setInput] = useState<IState>({
    auditoriumName:"",
    seatRows:1,
    numberOfSeats:1
  });

  const handleChange = (e) => {
      const {id ,value}=e.target;
      if(id==="auditoriumName"){
  
        setInput({ ...input ,[id]:value});
      }else{
        setInput({ ...input ,[id]:+value});
      }
    
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    onSubmit({
      id: Math.floor(Math.random() * 10000),
      auditoriumName:input.auditoriumName,
      numberOfSeats:input.numberOfSeats,
      seatRows:input.seatRows,
    });
    // 
    setInput({
        auditoriumName:"",
        seatRows:1,
        numberOfSeats:1
    });
  };

  return (
    <>
       <Col xs={10}>
       <Row className="d-flex justify-content-end">
        <FormControl
            type="text"
            placeholder="Auditorium name"
            value={input.auditoriumName}
            onChange={handleChange}
            id="auditoriumName"
            autoComplete="off"
            className="col-11 my-1"
        />
        </Row>
        <Row className="d-flex justify-content-end">
        <FormControl
             min="1"
            type="number"
            placeholder="Number of seats"
            value={input.numberOfSeats}
            onChange={handleChange}
            id="numberOfSeats"
            autoComplete="off"
            className="col-11 my-1"
        />
        </Row>
        <Row className="d-flex justify-content-end">
        <FormControl
            min="1"
            type="number"
            placeholder="Number of rows"
            value={input.seatRows}
            id="seatRows"
            onChange={handleChange}
            autoComplete="off"
            className="col-11 my-1"
        />
        </Row>
        
        </Col>
        <Row className="justify-content-center mt-1">
        <Col xs={2}>
          <Button onClick={handleSubmit} >
            Add
          </Button>
        </Col>
        </Row>
    </>
  );
};

export default AuditoriumForm;
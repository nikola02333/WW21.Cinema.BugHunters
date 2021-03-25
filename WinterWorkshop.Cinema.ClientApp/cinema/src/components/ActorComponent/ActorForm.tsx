import React, { useState, useEffect, useRef } from "react";
import { FormControl, FormGroup } from "react-bootstrap";


interface IState{
  input:string;
}

interface IProps{
  onSubmit(actor:any):void;
}

const ActorForm :React.FC<IProps>= ({ onSubmit,...props}) => {
  const [input, setInput] = useState<IState>({ input:''});


  const handleChange = (e) => {
    setInput({ ...input,input: e.target.value});
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    onSubmit({
      id: Math.floor(Math.random() * 10000),
      name: input.input,
    });
    // 
    setInput({input:""});
  };

  return (
    
        <FormGroup>
          <FormControl
            placeholder="Add Actor"
            value={input.input}
            onChange={handleChange}
            name="text"
           
            autoComplete="off"
          />
          <button onClick={handleSubmit} >
            Add Actor
          </button>
        </FormGroup>
  );
};

export default ActorForm;

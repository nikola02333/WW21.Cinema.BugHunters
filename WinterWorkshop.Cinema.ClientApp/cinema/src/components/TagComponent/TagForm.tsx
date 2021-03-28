import React, { useState, useEffect, useRef } from "react";
import { FormControl, FormGroup,Button, Row, Col } from "react-bootstrap";

interface IState{
  input:string;
  edit?: ITag;
}

interface ITag {
  id: string;
  name: string;
}
interface IProps{
  onSubmit(tag:any):void;
}

const TagForm :React.FC<IProps>= ({ onSubmit,...props}) => {
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
    <>
       <Col xs={10}>
          <FormControl
            
            placeholder="Add tag"
            value={input.input}
            onChange={handleChange}
            name="text"
           
            autoComplete="off"
        />
        </Col>
        <Col xs={2}>
          <Button onClick={handleSubmit} >
            Add
          </Button>
          </Col>
        </>
  );
};

export default TagForm;

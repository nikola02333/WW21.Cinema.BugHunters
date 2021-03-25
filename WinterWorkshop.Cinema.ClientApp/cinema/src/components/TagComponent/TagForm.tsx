import React, { useState, useEffect, useRef } from "react";

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
    <form onSubmit={handleSubmit} className="todo-form">
     
        <>
          <input
            placeholder="Add tag"
            value={input.input}
            onChange={handleChange}
            name="text"
            className="todo-input"
            
          />
          <button onClick={handleSubmit} className="todo-button">
            Add tag
          </button>
        </>
      )
    </form>
  );
};

export default TagForm;

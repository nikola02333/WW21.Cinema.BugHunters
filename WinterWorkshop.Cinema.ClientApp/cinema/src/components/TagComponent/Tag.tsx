import React, { useState } from "react";
import TodoForm from "./TagForm";
//import { RiCloseCircleLine } from "react-icons/ri";

interface ITag {
  id: string;
  name: string;
}
interface IProps{
  removeTag: (tagId) =>void;
  updateTag:(id, value)=> void;
  tags :ITag[];
}


const Tag = ({ tags, removeTag, updateTag }:IProps) => {

  const [edit, setEdit] = useState<ITag>(
    {id: "",
    name: "",}
  );

  const submitUpdate = (value) => {
    updateTag(edit.id, value);
    setEdit({
      id: "",
      name: "",
    });
  };

  if (edit.id) {
    return <TodoForm  onSubmit={submitUpdate} />;
  }
const toReturn =  tags.map((tag, index) => (
   <div className={"todo-row"} key={index}>
    <div key={tag.id}>{tag.name}</div>
    <div className="icons">
      <div>
      <i className="fas fa-trash-alt"></i>
      </div>
      <div
        onClick={() => removeTag(tag.id)}
        className="delete-icon"
      />
   
    </div>
  </div>
  ));

  return(<>{toReturn}</>);
};

export default Tag;

import React, { useState } from "react";
import TodoForm from "./TagForm";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
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
   <div key={index}>
    <div key={tag.id}>{tag.name}</div>
    <div className="icons">
      <div onClick={() => removeTag(tag.id)}>
      <FontAwesomeIcon icon={faTrash} />
      </div>
    </div>
  </div>
  ));

  return(<>{toReturn}</>);
};

export default Tag;

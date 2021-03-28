import React, { useState } from "react";
import TagForm from "./TagForm";
import Tag from "./Tag";
import { FormGroup, Row } from "react-bootstrap";

interface IState {
  tags:ITag [];
}
interface ITag{
  id: string;
  name: string;
}
 interface IProps{
   
  tags :ITag[];
  setState :any;
 }
const TagsList :React.FC<IProps> = (props) => {
  const [tags, setTags] = useState <ITag[]> (
   props.tags
  );

  const addTag = (tag) => {
    
    if (!tag.name || /^\s*$/.test(tag.name)) {
      return;
    }
    const newTags = [...tags,tag ];
    setTags(newTags);
    props.setState(prevState=> ({...prevState,tagss: newTags}));

    console.log(...tags);
  };

  const updateTag = (tagId, newValue) => {
    if (!newValue.name || /^\s*$/.test(newValue.name)) {
      return;
    }

    setTags((prev) =>
      prev.map((item) => (item.id === tagId ? newValue : item))
    );
  };

  const removeTag = (id) => {
    const removedArr = [...tags].filter((todo) => todo.id !== id);
    debugger;
    setTags(removedArr);
    props.setState(prevState=> ({...prevState,tagss: removedArr}));
  };

  return (
    <>
     <FormGroup className="mt-3">
       <Row>
       <TagForm  onSubmit={addTag} />
       </Row>
     
       <Tag tags={tags} removeTag={removeTag} updateTag={updateTag} />
      
      </FormGroup>
    </>
  );
};

export default TagsList;

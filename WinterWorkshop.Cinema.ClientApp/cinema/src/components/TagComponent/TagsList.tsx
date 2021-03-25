import React, { useState } from "react";
import TagForm from "./TagForm";
import Tag from "./Tag";

interface IState {
  tags:ITag [];
}
interface ITag{
  id: string;
  name: string;
}
 interface IProps{
  tags :ITag[];
 }
const TagsList :React.FC<IProps> = (props) => {
  const [tags, setTags] = useState <ITag[]> (
   //[]
   props.tags
  );

  const addTag = (tag) => {
    
    if (!tag.name || /^\s*$/.test(tag.name)) {
      return;
    }
    const newTags = [...tags,tag ];
    setTags(newTags);
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

    setTags(removedArr);
  };

  return (
    <>
      <h1>Add tags:</h1>
      <TagForm  onSubmit={addTag} />
      <Tag tags={tags} removeTag={removeTag} updateTag={updateTag} />
    </>
  );
};

export default TagsList;

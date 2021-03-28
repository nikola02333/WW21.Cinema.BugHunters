import React, { useState } from "react";
import TodoForm from "./TagForm";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup,Button, Row, Col } from "react-bootstrap";

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
   <Row key={index} className="mt-2">
    <Col xs={10} key={tag.id} className="d-flex justify-content-center">
    <FormControl
            placeholder="Add tag"
            value={tag.name}
            name="text"
            disabled
            autoComplete="off"
        />
    </Col>
    <Col xs={2} className="icons col-xs-1">
      <Button variant="danger" onClick={() => removeTag(tag.id)}>
      <FontAwesomeIcon icon={faTrash} />
      </Button>
    </Col>
  </Row>
  ));

  return(<>{toReturn}</>);
};

export default Tag;

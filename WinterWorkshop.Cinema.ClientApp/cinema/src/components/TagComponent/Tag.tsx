import React, { useState, Fragment } from "react";

interface IState{
    tagValue: string;
}
interface IProps {
addTag(): void;
    
   }
  
//const NewMovie: React.FC = (props: any) => {
const Tag :React.FC<IProps> = ({addTag})=> {
  const [tags, setTags] = useState([
    { tagValue: ''}
  ]);

  const handleAddFields = () => {
    const values = [...tags];
    
    values.push({ tagValue: ''});
    setTags(values);
  };

  const handleRemoveFields = index => {
    const values = [...tags];
    values.splice(index, 1);
    setTags(values);
  };

  const handleInputChange = (index, event) => {
    const values = [...tags];
    if (event.target.name === "tagValue") {
      values[index].tagValue = event.target.value;
    } 

    setTags(values);
  };

  const handleSubmit = e => {
    e.preventDefault();
    console.log("inputFields", tags);
  };

  return (
    <>
    <div className="d-flex justify-content-center">
      <form onSubmit={handleSubmit}>
        <div className="form-row">
          {tags.map((inputField, index) => (
            <Fragment key={`${inputField}~${index}`}>
              <div className="form-group col-sm-6">
                <input
                placeholder="Movies tag"
                  type="text"
                  className="form-control"
                  id="tagValue"
                  name="tagValue"
                  value={inputField.tagValue}
                  onChange={event => handleInputChange(index, event)}
                />
              </div>
            
              <div className="form-group col-sm-2">
                <button
                  className="btn btn-link"
                  type="button"
                  onClick={() => handleRemoveFields(index)}
                >
                  -
                </button>
                <button
                  className="btn btn-link"
                  type="button"
                  onClick={() => handleAddFields()}
                >
                  +
                </button>
              </div>
            </Fragment>
          ))}
        </div>
        {/*<div className="submit-button">
          <button
            className="btn btn-primary mr-2"
            type="submit"
            onSubmit={handleSubmit}
          >
            Save
          </button>
          </div>*/}



        <br/>
        <pre>
          {JSON.stringify(tags, null, 2)}
        </pre>
      </form>
      </div>
    </>
  );
};
export default Tag;
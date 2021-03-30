import React, { useEffect, useState } from "react";
import { Row, Table, OverlayTrigger,Tooltip } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEdit,
  faTrash,
  faInfoCircle,
  faLightbulb,
} from "@fortawesome/free-solid-svg-icons";
import { IMovie } from './../../models/index';
import {
    isAdmin,
    isSuperUser,
    isUser,
    isGuest,
  } from "../helpers/authCheck";

 interface IProps {
  movies:IMovie[];
  editMovie? : { (movieId) : void}
  removeMovie? (movieId) : void
  changeCurrent? ( 
      e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>,
    id: string) :void
  
 }


const Movie: React.FC<IProps> =({editMovie,changeCurrent,removeMovie,...props}) => {

    let userShouldSeeWholeTable;
    const shouldUserSeeWholeTable = () => {
      
      if (userShouldSeeWholeTable === undefined) {
        userShouldSeeWholeTable = !isGuest() && !isUser();
      }
      return userShouldSeeWholeTable;
    };
   
  const fillTableWithDaata = () => {
    return props.movies.map((movie) => {
      return (
        <tr key={movie.id}>
          <td
            className="text-center cursor-pointer"
          >
            <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">
              { movie.tagsModel?.map(tag=> tag.tagValue).join(" ")}</Tooltip>}>

            <FontAwesomeIcon
              className="text-info mr-2 fa-1x"
              icon={faInfoCircle}
            />
             </OverlayTrigger>
          </td>
          
          <td>{movie.title}</td>
          <td>{movie.year}</td>
          <td>{Math.round(movie.rating)}/10</td>
          <td>{ movie.hasOscar? "true" : "fase"}</td>
          {shouldUserSeeWholeTable() && <td>{movie.current ? "Yes" : "No"}</td>}
          {shouldUserSeeWholeTable() && (
            <td
              className="text-center cursor-pointer"
              onClick={() => editMovie?.(movie.id)}
            >
              <FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} />
            </td>
          )}
          {shouldUserSeeWholeTable() && (
            <td
              className="text-center cursor-pointer"
              onClick={() => removeMovie?.(movie.id)}
            >
              <FontAwesomeIcon
                className="text-danger mr-2 fa-1x"
                icon={faTrash}
              />
            </td>
          )}
          {shouldUserSeeWholeTable() && (
            <td
              className="text-center cursor-pointer"
              onClick={(
                e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>
              ) => changeCurrent?.(e, movie.id)}
            >
              <FontAwesomeIcon
                className={
                  movie.current
                    ? "text-warning mr-2 fa-1x"
                    : "text-secondary mr-2 fa-1x"
                }
                icon={faLightbulb}
              />
            </td>
          )}
        </tr>
      );
    });
  };

  let inputValue;
  const rowsData = fillTableWithDaata();
  const table = (
    <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Tags</th>
          <th>Title</th>
          <th>Year</th>
          <th>Rating</th>
          <th>Has oscar</th>
          {shouldUserSeeWholeTable() && <th>Is Current</th>}
          {shouldUserSeeWholeTable() && <th></th>}
          {shouldUserSeeWholeTable() && <th></th>}
        </tr>
      </thead>
      <tbody>{rowsData}</tbody>
    </Table>
  );
  //const showTable = props.isLoading ? <Spinner></Spinner> : table;
  const showTable = table;


return(<>
<Row className="no-gutters pr-5 pl-5">{showTable}</Row>
    </>);
};
export default Movie;
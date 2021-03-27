import React from 'react'
import { IProjection } from "../../../models";
import {projectionService} from "../../Services/projectionService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import {useHistory} from "react-router-dom"
import { Table } from "react-bootstrap";
import {IState} from "./ShowAllProjections"

interface IProps{
    projections:IProjection[];
    setState:React.Dispatch<React.SetStateAction<IState>>
}

const ProjectionsTable : React.FC<IProps> = ({projections,setState})=>{
    
    const history=useHistory();

    const editProjection = (id: string) => {
        history.push(`editProjection/${id}`);
      };

    const removeProjection = async(id: string) => {
        setState(prev=>({ ...prev, isLoading: true }));
        await projectionService.deleteProjectino(id);
        setState(prev=>({ ...prev, isLoading: false }));
      };


    const fillTableWithData = () => {
        console.log("object");
        return projections.map((projection) => {
          return (
            <tr key={projection.id}>
              <td width="18%">{projection.movieTitle}</td>
              <td width="18%">{projection.auditoriumName}</td>
              <td width="18%">{projection.projectionTime}</td>
              <td
                width="5%"
                className="text-center cursor-pointer"
                onClick={() => editProjection(projection.id)}
              >
                <FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} />
              </td>
              <td
                width="5%"
                className="text-center cursor-pointer"
                onClick={() => removeProjection(projection.id)}
              >
                <FontAwesomeIcon
                  className="text-danger mr-2 fa-1x"
                  icon={faTrash}
                />
              </td>
            </tr>
          );
        });
      };

    return(
        <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Movie Title</th>
          <th>Auditorium Name</th>
          <th>Projection Time</th>
          <th></th>
          <th></th>
        </tr>
      </thead>
      <tbody>{fillTableWithData()}</tbody>
    </Table>
    );
};

export default ProjectionsTable;
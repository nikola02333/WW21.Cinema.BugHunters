import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import { IProjection } from "../../../models";
import {projectionService} from "../../Services/projectionService";
import ProjectionsTable from "./ProjectionsTabel";

export interface IState {
  projections: IProjection[];
  isLoading: boolean;
}

const ShowAllProjections: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    projections: [
      {
        auditoriumId: 0,
        auditoriumName: "",
        duration: 0,
        id: "",
        movieId: "",
        movieTitle: "",
        price: 0,
        projectionTime: "",
      },
    ],
    isLoading: true,
  });

  useEffect(() => {
    getProjections();
  }, []);

  const getProjections = async() => {
    console.log("projection");
    setState(prev=>({ ...prev, isLoading: true }));
    var projections = await projectionService.getAllProjections();
    if(projections === undefined){
      return;
    }
    setState(prev=>({ ...prev,isLoading: false,  projections: projections }));
  };

  const removeProjection = async(id: string) => {
    setState(prev=>({ ...prev, isLoading: true }));
   const deleted= await projectionService.deleteProjectino(id);
   if(deleted===undefined){
    return;
   }
    setState(prev=>({ ...prev, isLoading: false }));
    setTimeout(() => window.location.reload(), 1000);
  };

  const fillTableWithDaata = () => {
    return state.projections.map((projection) => {
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

  const editProjection = (id: string) => {
    props.history.push(`editProjection/${id}`);
  };

  
  const showTable = state.isLoading ? <Spinner></Spinner> : <ProjectionsTable setState={setState} projections={state.projections}/>;
  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">All Projections</h1>
      </Row>
      <Row className="no-gutters pr-5 pl-5">{showTable}</Row>
    </React.Fragment>
  );
};

export default ShowAllProjections;

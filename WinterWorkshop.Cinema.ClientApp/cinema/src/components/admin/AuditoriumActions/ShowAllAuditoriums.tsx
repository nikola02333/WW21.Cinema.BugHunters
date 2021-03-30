import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import { withRouter } from "react-router";
import { IAuditorium, ISeats } from "../../../models";
import {auditoriumService} from './../../Services/auditoriumService';
interface IState {
  auditoriums: IAuditorium[];
  isLoading: boolean;
}

const ShowAllAuditoriums: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    auditoriums: [
      {
        id: "",
        cinemaId: "",
        name: "", 
        numberOfSeats:0,
        seatRows: 0
      },   
    ],
    isLoading: true,
  });

  useEffect(() => {
    getAuditoriums();
  }, []);

  const getAuditoriums = async () => {
    setState({ ...state, isLoading: true });
     
    var auditoriums = await auditoriumService.getAllAuditoriums();
    
    if(auditoriums=== undefined)
    {
      return;
    }
    setState({ ...state, auditoriums: auditoriums, isLoading: false });
  };

 

  const removeAuditorium = async (auditoriumId: string) => {
   
    var result = await auditoriumService.deleteAuditorium(auditoriumId);
    if(result === undefined)
     {
      return;
     }
     else{
      NotificationManager.success("Auditorium successfully deleted!");
      var auditoriumDeleted= state.auditoriums.filter( auditorium=> auditorium.id != auditoriumId );
      setState({...state, auditoriums: auditoriumDeleted});
     }
  };

   const size= (obj)=> {
    var size = 0, key;
    for (key in obj) {
        if (obj.hasOwnProperty(key)) size++;
    }
    return size;
};

  const fillTableWithData = () => {
    return state.auditoriums.map((auditorium) => {
      return (
        <tr key={auditorium.id}>
          <td width="25%">{auditorium.cinemaId}</td>
          <td width="25%">{auditorium.name}</td> 
          <td
            width="5%"
            className="text-center cursor-pointer"
            onClick={() => editAuditorium(auditorium.id)}
          >
            <FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} />
          </td>
          <td
            width="5%"
            className="text-center cursor-pointer"
            onClick={() => removeAuditorium(auditorium.id)}
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

  const editAuditorium = (id: string) => {
    props.history.push(`EditAuditorium/${id}`);
  };

  const rowsData = fillTableWithData();
  const table = (
    <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Cinema Id</th>
          <th>Name</th>       
        </tr>
      </thead>
      <tbody>{rowsData}</tbody>
    </Table>
  );
  const showTable = state.isLoading ? <Spinner></Spinner> : table;

  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">All Auditoriums</h1>
      </Row>
      <Row className="no-gutters pr-5 pl-5">{showTable}</Row>
    </React.Fragment>
  );
};

export default withRouter(ShowAllAuditoriums);

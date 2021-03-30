import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import { ICinema } from "../../../models";
import {cinemaService} from './../../Services/cinemaService';

interface IState {
  cinemas: ICinema[];
  isLoading: boolean;
}

const ShowAllCinemas: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    cinemas: [
      {
        id: "",
        name: "",
        address:"",
        cityName:""
      },
    ],
    isLoading: true,
  });

  
  useEffect(() => {
    getCinemas();
  },[]);

  const getCinemas = async () => {

    setState({ ...state, isLoading: true });
     
    var cinemas= await cinemaService.getCinemas();
    if(cinemas=== undefined)
    {
      return;
    }
    setState({ ...state, cinemas: cinemas, isLoading: false });
  };


  const removeCinema = async (id: string) => {
    var result = await cinemaService.removeCinema(id);
    if(result === undefined)
     {
      return;
     }
     else{
      var cinemaDeleted= state.cinemas.filter( cinema=> cinema.id != id );
      setState({...state, cinemas: cinemaDeleted});
     }
  };

  const fillTableWithDaata = () => {
    return state.cinemas.map((cinema) => {
      return (
        <tr key={cinema.id}>
          <td width="25%">{cinema.id}</td>
          <td width="25%">{cinema.name}</td>
          <td width="25%">{cinema.address}</td>
          <td width="25%">{cinema.cityName}</td>
          <td
            width="5%"
            className="text-center cursor-pointer"
            onClick={() => editCinema(cinema.id)}
          >
            <FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} />
          </td>
          <td
            width="5%"
            className="text-center cursor-pointer"
            onClick={() => removeCinema(cinema.id)}
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

  const editCinema = (id: string) => {
    // to be implemented
    props.history.push(`editcinema/${id}`);
  };

  const rowsData = fillTableWithDaata();
  const table = (
    <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Address</th>
          <th>City name</th>
          <th></th>
          <th></th>
        </tr>
      </thead>
      <tbody>{rowsData}</tbody>
    </Table>
  );
  const showTable = state.isLoading ? <Spinner></Spinner> : table;

  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">All Cinemas</h1>
      </Row>
      <Row className="no-gutters pr-5 pl-5">{showTable}</Row>
    </React.Fragment>
  );
};

export default ShowAllCinemas;

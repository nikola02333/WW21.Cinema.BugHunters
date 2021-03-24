import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import { withRouter } from "react-router";
import { IAuditorium, ISeats } from "../../../models";

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

  const getAuditoriums = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState((prev)=>({...prev, isLoading: true }));
    fetch(`${serviceConfig.baseURL}/api/auditoriums/all`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState((prev)=>({...prev, auditoriums: data, isLoading: false }));
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState((prev)=>({...prev, isLoading: false }));
      });
  };

 

  const removeAuditorium = (auditoriumId: string) => {
    const requestOptions = {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState((prev)=>({...prev, isLoading: true }));
    fetch(
      `${serviceConfig.baseURL}/api/Auditoriums/${auditoriumId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        NotificationManager.success("Successfully deleted auditorium.");
        return response.json();
      })

      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState((prev)=>({...prev, isLoading: false }));
      });

    setTimeout(() => window.location.reload(), 1000);
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

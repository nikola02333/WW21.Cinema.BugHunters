import React, { useEffect, useState } from "react";
import { useHistory,useParams,withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Typeahead } from "react-bootstrap-typeahead";
import { ICinema } from "../../../models";
import {cinemaService} from '../../Services/cinemaService'
import {auditoriumService} from '../../Services/auditoriumService'
interface IParams {
  id: string;
}
interface IState {
  id:string;
  auditoriumName: string;
  auditoriumNameError: string;
  submitted: boolean;
  canSubmit: boolean;
  isLoading: boolean;
}

const EditAuditorium: React.FC = (props: any)=> {
  const { id } = useParams<IParams>();
  const history = useHistory();
  
  const [state, setState] = useState<IState>({
    id:"",  
    auditoriumName: "", 
    auditoriumNameError: "", 
    submitted: false,
    canSubmit: true,
    isLoading: true
  });

  useEffect(() => {
    getAuditoriumById(id);
  }, [id]);
  
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    validate(id, value);
    setState((prev)=>({ ...prev, [id]: value }));
  
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState((prev)=>({ ...prev, submitted: true }));
    if (state.auditoriumName)
     {
      editAuditorium();
    } 
    else{
      NotificationManager.error("Please fill form with data.");
      setState((prev)=>({ ...prev, submitted: false }));
    }
  };

  const validate = (id: string, value: string | null) => {
    if (id === "auditoriumName") {
      if (value === "") {
        setState((prev)=>({
          ...prev,
          auditoriumNameError: "Fill in auditorium name",
          canSubmit: false,
        }));
      }
     }
  };

  const editAuditorium = async () => {
    const data = { 
      auditoriumName: state.auditoriumName
    };
    await auditoriumService.updateAuditorium(id,data);
    history.push('/dashboard/AllAuditoriums'); 
  };

  const getAuditoriumById = async (auditoriumId: string) => {

    var auditorium = await auditoriumService.getAuditoriumById(auditoriumId);
    if(auditorium  != undefined)
    {
      setState((prev)=>({
        ...prev,
        auditoriumName: auditorium.auditoriumName,
        id: auditorium.id + ""
      }));
    }
  };

  return (
    <Container>
      <Row className="d-flex justify-content-center mt-3">
        <Col xs={12}>
          <h1 className="form-header">Edit Auditorium</h1>
        </Col>
        <Col xs={11} md={9} lg={7} xl={5}>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="auditoriumName"
                type="text"
                placeholder="Auditorium Name"
                value={state.auditoriumName}
                onChange={handleChange}
              />
              <FormText className="text-danger">
                {state.auditoriumNameError}
              </FormText>
            </FormGroup>       
            <Button
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Edit Auditorium
            </Button>
          </form>
       
       </Col>
      </Row>
    </Container>
  );
  };
export default withRouter(EditAuditorium);

import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import {FormGroup,Button,Container,Row,Col,FormText,FormControl,Form} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { IAuditorium, IMovie } from "../../../models";
import {projectionService} from "../../Services/projectionService";
import {ICreateProjection} from "../../../models/ProjectionModels";
import {movieService} from "../../Services/movieService"
import {auditoriumService} from "../../Services/auditoriumService"
import {IProjection} from "../../../models/index" 
import {useHistory} from "react-router-dom"



interface IState {
  projectionTime: string;
  movieId: string;
  auditoriumId: number;
  price:number;
  duration:number;

  priceError:string;
  durationError:string;
  projectionTimeError: string;
  movieIdError: string;
  auditoriumIdError: string;

  movies: IMovie[];
  auditoriums: IAuditorium[];
  canSubmit: boolean;
  submitted: boolean;
  
  projectionTimeSubmit: boolean;
  movieIdSubmit: boolean;
  auditoriumIdSubmit: boolean;
  priceSubmit:boolean;
  durationSubmit:boolean;
  edit:boolean;
}


const NewProjection: React.FC = (props: any) => {

  var id = window.location.pathname.split("/")[3];

  const [state, setState] = useState<IState>({
    projectionTime: "",
    movieId: "",
    auditoriumId: 0,
    price:0,
    duration:0,
    priceError:"",
    durationError:"",
    projectionTimeError: "",
    movieIdError: "",
    auditoriumIdError: "",
    movies: [
      {
        id: "",
        coverPicture: "",
        rating: 0,
        title: "",
        year: "",
      },
    ],
    auditoriums: [
      {
        id: "",
        name: "",
        cinemaId:""
      },
    ],
    submitted: false,
    canSubmit: false,
    projectionTimeSubmit: false,
    movieIdSubmit: false,
    auditoriumIdSubmit: false,
    priceSubmit:false,
    durationSubmit:false,
    edit:false
  });

  useEffect(() => {
    getProjection();
    getMovies();
    getAuditoriums();
  }, []);
  useEffect(()=>{
    console.log(state);
  },[state]);

  const getProjection = async()=>{
    
    if(id===undefined){
      return;
    }
    var projection =await projectionService.getProjectionById(id);
    if(projection === undefined){
      return;
    }
    const {movieId , auditoriumId,projectionTime,price,duration}=projection;

    setState((prev)=>({...prev,movieId:movieId ,auditoriumId:auditoriumId,
      projectionTime:projectionTime,price:price,duration:duration,edit:true}));
      console.log(state.edit);
      validate("movieId",movieId);
      validate("auditoriumId",auditoriumId);
      validate("projectionTime",projectionTime);
      validate("price",price);
      validate("duration",duration);
  }

  const getMovies = async() => {
    const movies =await movieService.getAllMovies();
    if(movies===undefined){
      setState((prev)=>({ ...prev, submitted: false }));
      return;
    }
    setState((prev)=>({ ...prev, movies: movies }));
  };

  const getAuditoriums = async() => {
    const auditoriums = await auditoriumService.getAllAuditoriums();
    if(auditoriums===undefined){
      setState((prev)=>({ ...prev, submitted: false }));
      return;
    }
    setState((prev)=>({ ...prev, auditoriums: auditoriums }));
  };


  const handleChange = (e) => {
    const { id, value } = e;
    console.log(id);
    if (value!==null) {
      if(id==="auditoriumId" ||id==="price" || id==="duration"){
        setState((prev)=>({ ...prev, [id]: +value}));
      }else{
        setState((prev)=>({ ...prev, [id]: value}));
      }
      
      validate(id, value);
    } else {
      //validate(id, null);
    }
  };

  const onDateChange = (e) =>{
    var date = e.target.value;
    const datetime = new Date(date);
    if(datetime < new Date()){
      validate("projectionTime",null);
    }else{
      validate("projectionTime",date);
      setState((prev)=>({ ...prev, projectionTime: date }));
    }
  };

  const validate = (id, value) => {
    if (id === "projectionTime") {
      if (!value) {
        console.log("kk");
        setState((prev)=>({ ...prev,
          projectionTimeError: "Projection time cannot be in past",
          canSubmit: false,
          projectionTimeSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, projectionTimeError: "", canSubmit: true ,projectionTimeSubmit:true}));
      }
    } else if (id === "movieId") {
      if (!value||value==="") {
        setState((prev)=>({ ...prev,
          movieIdError: "Please chose movie from dropdown",
          canSubmit: false,
          movieIdSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, movieIdError: "", canSubmit: true ,movieIdSubmit:true}));
      }
    } else if (id === "auditoriumId") {
      if (!value ||value==="") {
        setState((prev)=>({ ...prev,
          auditoriumIdError: "Please chose auditorium from dropdown",
          canSubmit: false,
          auditoriumIdSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, auditoriumIdError: "", canSubmit: true ,auditoriumIdSubmit:true}));
      }
    }else if (id === "price") {
      if (!value || value<1) {
        setState((prev)=>({ ...prev,
          priceError: "Please insert price higher then 0",
          canSubmit: false,
          priceSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, priceError: "", canSubmit: true,priceSubmit:true }));
      }
    }else if (id === "duration") {
      if (!value || value<1) {
        setState((prev)=>({ ...prev,
          durationError: "Please insert duration higher then 0",
          canSubmit: false,
          durationSubmit:false
        }));
      } else {
        setState((prev)=>({ ...prev, durationError: "", canSubmit: true,durationSubmit:true }));
      }
    }
  };


  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState((prev)=>({ ...prev, submitted: true }));
    console.log(state);

    if (state.durationSubmit && state.movieIdSubmit && state.auditoriumIdSubmit && state.priceSubmit && state.projectionTimeSubmit) {
      if(state.edit){
        editProjection();
      }else{
        addProjection();
      }
      
    } else {
      NotificationManager.error("Please fill in data");
      setState((prev)=>({ ...prev, submitted: false }));
    }
  };
  const editProjection= async()=>{
    const data:ICreateProjection = {
      MovieId: state.movieId,
      AuditoriumId: state.auditoriumId,
      ProjectionTime: new Date(state.projectionTime) ,
      Duration: state.duration,
      Price:state.price
    };
    const updated= await projectionService.updateProjection(id,data);
    if(updated===undefined){
    setState((prev)=>({ ...prev, submitted: false }));
    return;
    }
    NotificationManager.success("Projection updated!");
    props.history.push(`AllProjections`); 
  }

  const addProjection = async() => {
    const data:ICreateProjection = {
      MovieId: state.movieId,
      AuditoriumId: state.auditoriumId,
      ProjectionTime: new Date(state.projectionTime) ,
      Duration: state.duration,
      Price:state.price
    };
    
    const created= await projectionService.createProjection(data)
    if(created===undefined){
    setState((prev)=>({ ...prev, submitted: false }));
    return;
    }
    NotificationManager.success("New projection added!");
    props.history.push(`AllProjections`); 
  };

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">{state.edit? "Edit Projection":"Add Projection"}</h1>
          <Form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                value={state.movieId}
                as="select"
                placeholder="Choose a movie..."
                id="movieId"
                className="add-new-form"
                onChange={(e) => {
                  handleChange(e.target);
                 }}
              >
                <option value={""} >Choose a movie...</option>
                {state.movies.map((movie)=>{
                return(
                  <option key={movie.id} value={movie.id}>{movie.title}</option>
                );
              })}
              </FormControl>
              <FormText className="text-danger text-center">{state.movieIdError}</FormText>
            </FormGroup>
            <FormGroup>
              <Form.Control
                value={state.auditoriumId}
                as="select"
                className="add-new-form"
                placeholder="Choose auditorium..."
                id="auditoriumId"
                onChange={(e) => {
                  handleChange(e.target);
                 }}
              ><option value={""}>Choose a auditorium...</option>
                {state.auditoriums.map((auditorium)=>{
                return(
                  <option key={auditorium.id} value={auditorium.id}>{auditorium.name}</option>
                );
              })}
              </Form.Control>
              <FormText className="text-danger text-center">
                {state.auditoriumIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
            <FormControl
              type="number"
              min="0"
              className="add-new-form"
              placeholder={state.edit?"Price: "+state.price : "Projection price"}
              id="price"
              onChange={(e) => {
                handleChange(e.target);
              }}
              />
              <FormText className="text-danger text-center">
                {state.priceError}
              </FormText>
            </FormGroup>
            
            <FormGroup >
              <Form.Control
              min="0"
              type="number"
              className="add-new-form"
              placeholder={state.edit?"Duration: "+state.duration+" min":"Duration in minutes"}
              id="duration"
              onChange={(e) => {
                handleChange(e.target);
              }}
              />
              <FormText className="text-danger text-center">
                {state.durationError}
              </FormText>
            </FormGroup>

            <FormGroup>
              <FormControl
                value={state.projectionTime}
                onChange={(e) =>
                  onDateChange(e)
                }
                name="dateTime"
                type="datetime-local"
                id="date"
                className="add-new-form"
                
              />
              <FormText className="text-danger text-center">
                {state.projectionTimeError}
              </FormText>
            </FormGroup>
            <FormGroup className="d-flex justify-content-center">
            <Button
             
              type="submit"
              disabled={(state.durationSubmit && state.movieIdSubmit && state.auditoriumIdSubmit && state.priceSubmit && state.projectionTimeSubmit ) ? false : true}
            >
              {state.edit?"Edit":"Add"}
            </Button>
            </FormGroup>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewProjection);

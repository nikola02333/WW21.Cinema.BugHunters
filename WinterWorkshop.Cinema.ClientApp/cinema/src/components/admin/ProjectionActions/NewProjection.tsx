import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  Button,
  Container,
  Row,
  Col,
  FormText,FormControl
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Typeahead } from "react-bootstrap-typeahead";
import DateTimePicker from "react-datetime-picker";
import { IAuditorium, IMovie } from "../../../models";
import {projectionService} from "../../Services/projectionService";
import {ICreateProjection} from "../../../models/ProjectionModels";
import {movieService} from "../../Services/movieService"



interface IState {
  projectionTime: string;
  movieId: string;
  auditoriumId: number;
  submitted: boolean;
  projectionTimeError: string;
  movieIdError: string;
  auditoriumIdError: string;
  movies: IMovie[];
  auditoriums: IAuditorium[];
  canSubmit: boolean;
  price:number;
  priceError:string;
  duration:number;
  durationError:string;
}

const NewProjection: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    projectionTime: "",
    movieId: "",
    auditoriumId: 0,
    submitted: false,
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
    canSubmit: true,
    price:1,
    priceError:"",
    duration:1,
    durationError:""
  });

  useEffect(() => {
    getMovies();
    getAuditoriums();
  }, []);

  const handleChange = (e) => {
    const { id, value } = e.target;
    setState((prev)=>({ ...prev, [id]: value }));
  };

  const validate = (id, value) => {
    if (id === "projectionTime") {
      if (!value) {
        setState((prev)=>({ ...prev,
          projectionTimeError: "Chose projection time",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, projectionTimeError: "", canSubmit: true }));
      }
    } else if (id === "movieId") {
      if (!value) {
        setState((prev)=>({ ...prev,
          movieIdError: "Please chose movie from dropdown",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, movieIdError: "", canSubmit: true }));
      }
    } else if (id === "auditoriumId") {
      if (!value) {
        setState((prev)=>({ ...prev,
          auditoriumIdError: "Please chose auditorium from dropdown",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, auditoriumIdError: "", canSubmit: true }));
      }
    }else if (id === "price") {
      if (!value) {
        setState((prev)=>({ ...prev,
          priceError: "Please insert price",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, priceError: "", canSubmit: true }));
      }
    }else if (id === "duration") {
      if (!value) {
        setState((prev)=>({ ...prev,
          durationError: "Please insert duration",
          canSubmit: false,
        }));
      } else {
        setState((prev)=>({ ...prev, durationError: "", canSubmit: true }));
      }
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState((prev)=>({ ...prev, submitted: true }));
    console.log(state);

    if (state.movieId && state.auditoriumId && state.projectionTime && state.price && state.duration) {
      addProjection();
    } else {
      NotificationManager.error("Please fill in data");
      setState((prev)=>({ ...prev, submitted: false }));
    }
  };

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
        return;
       }
        NotificationManager.success("New projection added!");
        props.history.push(`AllProjections`);
        setState((prev)=>({ ...prev, submitted: false }));
  };

  const getMovies = async() => {
    const movies =await movieService.getAllMovies();
    if(movies===undefined){
      setState((prev)=>({ ...prev, submitted: false }));
      return;
    }
    setState((prev)=>({ ...prev, movies: movies }));
  };

  const getAuditoriums = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/auditoriums/all`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState((prev)=>({ ...prev, auditoriums: data }));
        }
        console.log(data);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState((prev)=>({ ...prev, submitted: false }));
      });
  };

  const onMovieChange = (id: string) => {
    if (id) {
      setState((prev)=>({ ...prev, movieId: id }));
      validate("movieId", id);
    } else {
      validate("movieId", null);
    }
  };

  const onAuditoriumChange = (id: number) => {
    if (id!==null) {
      setState((prev)=>({ ...prev, auditoriumId: id}));
      validate("auditoriumId", id);
    } else {
      validate("auditoriumId", null);
    }
  };
  const onPriceChange = (price:number) => {
    if (price>1) {
      setState((prev)=>({ ...prev, price: price}));
      validate("price", price);
    } else {
      validate("price", null);
    }
  };
  const onDurationChange = (duration:number) => {
    if (duration>1) {
      setState((prev)=>({ ...prev, duration:duration }));
      validate("duration", duration);
    } else {
      validate("duration", null);
    }
  };

  const onDateChange = (e) =>{
    var date = e.target.value;
    setState((prev)=>({ ...prev, projectionTime: date }));
    console.log(date)
  };
  
  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add Projection</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                required={true}
                as="select"
                placeholder="Choose a movie..."
                id="movie"
                className="add-new-form"
                onChange={(e) => {
                  onMovieChange(e.target.value);
                 }}
              ><option>Choose a movie...</option>
                {state.movies.map((movie)=>{
                return(
                  <option value={movie.id}>{movie.title}</option>
                );
              })}
              </FormControl>
              <FormText className="text-danger">{state.movieIdError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                as="select"
                className="add-new-form"
                placeholder="Choose auditorium..."
                id="auditorium"
                onChange={(e) => {
                  onAuditoriumChange(+e.target.value);
                 }}
              ><option>Choose a auditorium...</option>
                {state.auditoriums.map((auditorium)=>{
                return(
                  <option value={auditorium.id}>{auditorium.name}</option>
                );
              })}
              </FormControl>
              <FormText className="text-danger">
                {state.auditoriumIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
            <FormControl
              type="number"
              min="1"
              className="add-new-form"
              placeholder="Projection price"
              id="price"
              onChange={(e) => {
                onPriceChange(+e.target.value);
              }}
              />
              <FormText className="text-danger">
                {state.auditoriumIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
              min="1"
              type="number"
              className="add-new-form"
              placeholder="Duration in minutes"
              id="duration"
              onChange={(e) => {
                onDurationChange(+e.target.value);
              }}
              />
              <FormText className="text-danger">
                {state.auditoriumIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                
                onChange={(e) =>
                  onDateChange(e)
                }
                name="dateTime"
                type="datetime-local"
                id="date"
                className="add-new-form"
                
              />
              <FormText className="text-danger">
                {state.projectionTimeError}
              </FormText>
            </FormGroup>
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              
            >
              Add
            </Button>
          </form>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewProjection);

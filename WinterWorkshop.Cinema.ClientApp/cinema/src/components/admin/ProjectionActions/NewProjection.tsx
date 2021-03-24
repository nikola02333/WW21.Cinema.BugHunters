import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Typeahead } from "react-bootstrap-typeahead";
import DateTimePicker from "react-datetime-picker";
import { IAuditorium, IMovie } from "../../../models";

interface IState {
  projectionTime: string;
  movieId: string;
  auditoriumId: string;
  submitted: boolean;
  projectionTimeError: string;
  movieIdError: string;
  auditoriumIdError: string;
  movies: IMovie[];
  auditoriums: IAuditorium[];
  canSubmit: boolean;
}

const NewProjection: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    projectionTime: "",
    movieId: "",
    auditoriumId: "",
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
  });

  useEffect(() => {
    getProjections();
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
        setState({ ...state, movieIdError: "", canSubmit: true });
      }
    } else if (id === "auditoriumId") {
      if (!value) {
        setState((prev)=>({ ...prev,
          auditoriumIdError: "Please chose auditorium from dropdown",
          canSubmit: false,
        }));
      } else {
        setState({ ...state, auditoriumIdError: "", canSubmit: true });
      }
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });

    if (state.movieId && state.auditoriumId && state.projectionTime) {
      addProjection();
    } else {
      NotificationManager.error("Please fill in data");
      setState({ ...state, submitted: false });
    }
  };

  const addProjection = () => {
    const data = {
      movieId: state.movieId,
      auditoriumId: state.auditoriumId,
      projectionTime: state.projectionTime,
    };

    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/projections`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("New projection added!");
        props.history.push(`AllProjections`);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState((prev)=>({ ...prev, submitted: false }));
      });
  };

  const getProjections = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/Movies/AllMovies/${false}`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState((prev)=>({ ...prev, movies: data }));
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState((prev)=>({ ...prev, submitted: false }));
      });
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
        setState({ ...state, submitted: false });
      });
  };

  const onMovieChange = (movies: IMovie[]) => {
    if (movies[0]) {
      setState((prev)=>({ ...prev, movieId: movies[0].id }));
      validate("movieId", movies[0]);
    } else {
      validate("movieId", null);
    }
  };

  const onAuditoriumChange = (auditoriums: IAuditorium[]) => {
    if (auditoriums[0]) {
      setState((prev)=>({ ...prev, auditoriumId: auditoriums[0].id }));
      validate("auditoriumId", auditoriums[0]);
    } else {
      validate("auditoriumId", null);
    }
  };

  const onDateChange = (date: Date) =>{
    setState((prev)=>({ ...prev, projectionTime: date.toLocaleTimeString() }));
  };
  
  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add Projection</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <Typeahead
                labelKey="title"
                options={state.movies}
                placeholder="Choose a movie..."
                id="movie"
                className="add-new-form"
                onChange={(e) => {
                  onMovieChange(e);
                }}
              />
              <FormText className="text-danger">{state.movieIdError}</FormText>
            </FormGroup>
            <FormGroup>
              <Typeahead
                labelKey="name"
                className="add-new-form"
                options={state.auditoriums}
                placeholder="Choose auditorium..."
                id="auditorium"
                onChange={(e) => {
                  onAuditoriumChange(e);
                }}
              />
              <FormText className="text-danger">
                {state.auditoriumIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <DateTimePicker
                className="form-control add-new-form"
                onChange={onDateChange}
                value={state.projectionTime}
              />
              <FormText className="text-danger">
                {state.projectionTimeError}
              </FormText>
            </FormGroup>
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
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

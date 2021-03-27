import React, { useState } from "react";
import { withRouter } from "react-router-dom";
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
import { YearPicker } from "react-dropdown-date";

import { movieService } from './../Services/movieService';
import { IMovieToCreateModel } from './../../models/IMovieToCreateModel';
import TagsList from '../TagComponent/TagsList';
import {ITag} from '../../models/ITag';
import ActorList from './../ActorComponent/ActorList';
import { IActor } from './../../models/IActor';
interface IState {
  title: string;
  year: string;
  rating: string;
  current: boolean;
  titleError: string;
  submitted: boolean;
  canSubmit: boolean;
  tags: string;
  coverPicture: string;
  yearError: string;
  genre: string;
  genreError: string;
  tagss: ITag[];
  Actors:string;
  Actorss:IActor[];
  ActorsError:string;
  description:string;
  descriptionError:string;
}

const NewMovie: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    title: "",
    year: "",
    rating: "",
    current: false,
    titleError: "",
    submitted: false,
    canSubmit: true,
    tags: "",
    coverPicture: "",
    yearError: "",
    genre: "",
  genreError: "",
  tagss:[],
  description:"",
  descriptionError:"",
  Actors:"",
  ActorsError:"",
  Actorss:[]
  });

  

  console.log('tagovi: '+JSON.stringify(state.tagss));
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    validate(id, value);
    setState({ ...state, [id]: value });
  
  };

  const handleTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setState({ ...state, tags: e.target.value });
  };

  const handleBannerUrlChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setState({ ...state, coverPicture: e.target.value });
  };

  const validate = (id: string, value: string) => {
    if (id === "title") {
      
      if (value === "") {
        setState({
          ...state,
          titleError: "Fill in movie title",
          canSubmit: false,
        });
      } else {
        setState({ ...state, titleError: "", canSubmit: true });
      }
    }
    if (id === "genre") {
      if (value === "") {
        setState({
          ...state,
          genreError: "Fill in movie genre",
          canSubmit: false,
        });
      } else {
        setState({ ...state, genreError: "", canSubmit: true });
      }
    }

    if (id === "year") {
      const yearNum = +value;
      if (!value || value === "" || yearNum < 1895 || yearNum > 2100) {
        setState({ ...state, yearError: "Please chose valid year" });
      } else {
        setState({ ...state, yearError: "" });
      }
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // ovde sad treba tagove da izvucem iz statea
    setState({ ...state, submitted: true });
    const { title, year, rating } = state;
    if (title && year && rating ) {
      addMovie();
    } else {
     
      NotificationManager.error("Please fill in data");
      setState({ ...state, submitted: false });
    }
  };

  const handleYearChange = (year: string) => {
    validate("year", year);
    setState({ ...state, year: year });
   
  };

 
  const addMovie = async() => {
   
    var movieToCreate : IMovieToCreateModel = {
 
      Title: state.title,
      Year: +state.year,
      Current: ( (state.current.toString() === 'true') ? true: false),
      Rating: +state.rating,
      Tags: state.tagss.map(tag=> tag.name).join(','),
      CoverPicture: state.coverPicture,
      Genre: state.genre,
      Actors: state.Actorss.map(actor=> actor.name).join(','),
      Description:''
    };
    
   await movieService.createMovie(movieToCreate);
  };

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add New Movie</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="title"
                type="text"
                placeholder="Movie Title"
                value={state.title}
                onChange={handleChange}
                className="add-new-form"
              />
              <FormText className="text-danger">{state.titleError}</FormText>
            </FormGroup>
            <FormGroup>
              <YearPicker
                defaultValue={"Select Movie Year"}
                start={1895}
                end={2022}
                reverse
                required={true}
                disabled={false}
                value={state.year}
                onChange={(year: string) => {
                  handleYearChange(year);
                }}
                id={"year"}
                name={"year"}
                classes={"form-control add-new-form"}
                optionClasses={"option classes"}
              />
              <FormText className="text-danger">{state.yearError}</FormText>
            </FormGroup>
            <FormGroup>
            
              <FormControl
                as="select"
                className="add-new-form"
                placeholder="Rating"
                id="rating"
                value={state.rating}
                onChange={handleChange}
              >
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
                <option value="5">5</option>
                <option value="6">6</option>
                <option value="7">7</option>
                <option value="8">8</option>
                <option value="9">9</option>
                <option value="10">10</option>
              </FormControl>
            </FormGroup>
            <FormGroup>
              <FormControl
                className="add-new-form"
                as="select"
                placeholder="Current"
                id="current"
                value={state.current.toString()}
                onChange={handleChange}
              >
                <option value="true">Current</option>
                <option value="false">Not Current</option>
              </FormControl>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="genre"
                type="text"
                placeholder="Genre"
                value={state.genre}
                onChange={handleChange}
                className="add-new-form"
              />
              <FormText className="text-danger">{state.genreError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="description"
                type="text"
                placeholder="About Movie"
                value={state.description}
                onChange={handleChange}
                className="add-new-form"
              />
              <FormText className="text-danger">{state.description}</FormText>
            </FormGroup>

            <FormControl
              id="tags"
              type="text"
              placeholder="Movie Tags"
              value={state.tags}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                handleTagsChange(e);
              }}
              className="add-new-form"
            />

           
            <div className="d-flex justify-content-center">
            <TagsList setState={setState} tags={state.tagss}/>
            </div>
         
         
            <div className="d-flex justify-content-center">
            <ActorList setState={setState} actors={state.Actorss}/>
            </div>
            <FormControl
              id="coverPicture"
              type="text"
              placeholder="Banner Url"
              value={state.coverPicture}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                handleBannerUrlChange(e);
              }}
              className="add-new-form"
            />
            <FormText className="text-danger">{state.titleError}</FormText>
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Add Movie
            </Button>
          </form>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewMovie);

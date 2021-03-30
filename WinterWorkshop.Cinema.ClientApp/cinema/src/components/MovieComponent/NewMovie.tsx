import React, { useState } from "react";
import { withRouter ,useHistory} from "react-router-dom";

import {FormGroup,Button,Container,Row,Col,FormText,FormControl,Form} from "react-bootstrap";
import { YearPicker } from "react-dropdown-date";

import { movieService } from './../Services/movieService';
import { IMovieToCreateModel } from './../../models/IMovieToCreateModel';
import TagsList from '../TagComponent/TagsList';
import {ITag} from '../../models/ITag';
import ActorList from './../ActorComponent/ActorList';
import { IActor } from './../../models/IActor';


import { imdbService } from './../Services/imdbService';
interface IState {
  title: string;
  year: string;
  rating: string;
  current: boolean;
  movieTitleId: string;
  movieTitleIdError:string;
  titleError: string;
  titleSubmit: boolean;
  submitted: boolean;
  canSubmit: boolean;
  tags: string;
  coverPicture: string;
  yearError: string;
  genre: string;
  genreError: string;
  genreSubmit: boolean;
  tagss: ITag[];
  Actors:string;
  Actorss:IActor[];
  ActorsError:string;
  description:string;
  yearSubmit: boolean;
  descriptionSubmit: boolean;
  imdbid: string;
  hasOscar: boolean;
  imdbidError:string;
  descriptionError:string;
}
const NewMovie: React.FC = (props: any) => {
  const history = useHistory();
  const [state, setState] = useState<IState>({
    title: "",
    year: "",
    yearSubmit: false,
    rating: "",
    current: false,
    titleError: "",
    movieTitleId: "",
    movieTitleIdError: "",
   titleSubmit: false,
    submitted: false,
    canSubmit: true,
    tags: "",
    coverPicture: "",
    hasOscar:false,
    yearError: "",
    genre: "",
  genreError: "",
  genreSubmit: false,
  tagss:[],
  description:"",
  descriptionError:"",
  descriptionSubmit: false,
  Actors:"",
  ActorsError:"",
  imdbid: "",
  imdbidError:"",
  Actorss:[
   /* {
      id: "str",
      name: 'nikola',
    }*/
  ]
  });

   const handleChange = (e) => {
    const { id, value } = e;
        setState((prev)=>({ ...prev, [id]: value}));
        validate(id, value);
  
  };
  
  const handleBannerUrlChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setState({ ...state, coverPicture: e.target.value });
  };

  const validate = (id: string, value: string) => {
    if (id === "title") {
      if (value === "") {
        setState( (prevState)=> 
        ({...prevState, 
         titleError: "Fill in movie title", 
         canSubmit: false,
        titleSubmit: false}));
      } else {
       setState( (prevState)=> 
       ({...prevState, 
        titleError: "", 
        canSubmit: true,
       titleSubmit: true}));
      }
    }


    if (id === "genre") {
      if (value === "") {
        setState( (prevState)=> ({...prevState, genreError:"Fill in movie genre", canSubmit: false}));
      } else {
        setState( (prevState)=> ({...prevState, genreError:"", canSubmit: true, genreSubmit: true}));
      }
    }

    if (id === "year") {
      const yearNum = +value;
      if (!value || value === "" || yearNum < 1895 || yearNum > 2100) {
        setState({ ...state, yearError: "Please chose valid year" });
      } else {
        setState( (prevState)=> ({...prevState,  yearError: '', yearSubmit: true}));
      }
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

  
    setState({ ...state, submitted: true });
    const { title, year, genre, description, coverPicture } = state;
    
 
    if (title && year && genre && description && coverPicture  ) {
      addMovie();

    } else {
     
      setState({ ...state, submitted: false });
    }
  };

  const handleYearChange = (year: string) => {
    validate("year", year);
    setState({ ...state, year: year });
   
  };

  const searchImdb= async(id:string)=>{

    const moveiResult = await imdbService.searchImdb(id);

    if(moveiResult !== undefined)
    {

      /**
       *   let myInterfacesArray = countryDealers.map(xx=>{

    return <IDealer>
    {
         dealerName : xx.name,
         dealerCode : xx.code,
         dealerCountry : xx.country
          // and so on
     };

  });
       * let element = document.getElementById(id);
    element.value = valueToSelect;
       */
const actorsToSet= moveiResult.actorList.map( (actor,index) => {
  return  {
    id:  index,
    name: actor.name
    }
  });

    setState((prevState)=>({...prevState,
       title:moveiResult.title,
      genre: moveiResult.genres,
      rating: Math.round(moveiResult.imDbRating).toString(),
      coverPicture: moveiResult.image,
      description : moveiResult.plot,
      
      Actorss :[ ...prevState.Actorss ,...actorsToSet], 
      current: true,
      year: moveiResult.year,
      canSubmit:true,
      titleSubmit: true,
      genreSubmit:true
      }));


    }
  }
 
  const addMovie = async() => {
   
    
    var movieToCreate : IMovieToCreateModel = {
      Title: state.title,
      Year: +state.year,
      ImdbId: state.imdbid,
      Current: ( (state.current.toString() === 'true') ? true: false),
      Rating: +state.rating,
      Tags: state.tagss.map(tag=> tag.name).join(','),
      CoverPicture: state.coverPicture,
      Genre: state.genre,
      Actors: state.Actorss.map(actor=> actor.name).join(','),
      Description: state.description,
      HasOscar:  state.hasOscar.toString()=='true' ? true: false
    };
  const result = await movieService.createMovie(movieToCreate);

  if(result === undefined)
  {
    return;
  }
  history.push('/dashboard/AllMovies');
  };

  const isFormValid =()=>
  {
    return ( ( state.titleSubmit && state.genreSubmit) ? false: true )
  }
  const resetFilds= (e )=> {
    
   //document.getElementById("createMovieForm")?.reset();

  }
  return (
    <Container>
     <Row className="d-flex justify-content-center mt-3">
        
        <Col xs={12}>
          <h1 className="form-header">Add New Movie</h1>
          </Col>
          <Col xs={11} md={9} lg={7} xl={5}>
          <form onSubmit={handleSubmit} id="createMovieForm" >

            <FormGroup>
              <FormControl
                id="imdbid"
                type="text"
                placeholder="movieId for search on Imdb"
                value={state.imdbid}
                onChange={(e) => handleChange(e.target)}
                //  <FormText className="text-danger text-center">{state.imdbidError}</FormText>
              />
              <FormGroup className="d-flex justify-content-center">
              <Button 
              type="button"  
              onClick={()=>searchImdb(state.imdbid)} 
              className="col-4 mt-3"> Seach Imdb</Button>
             </FormGroup>

            </FormGroup>
            <FormGroup>
              <FormControl
                id="title"
                type="text"
                placeholder="Movie Title"
                value={state.title}
                onChange={(e) => handleChange(e.target)}
              />
              <FormText className="text-danger text-center">{state.titleError}</FormText>
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
                classes={"form-control"}
                optionClasses={"option classes"}
              />
              <FormText className="text-danger">{state.yearError}</FormText>
            </FormGroup>
            <FormGroup>
            
              <FormControl
                as="select"
                placeholder="Rating"
                id="rating"
                value={state.rating}
                onChange={(e)=> handleChange(e.target)}
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
                
                as="select"
                placeholder="Current"
                id="current"
                value={state.current.toString()}
                onChange={(e)=>handleChange(e.target)}
              >
                <option value="true">Current</option>
                <option value="false">Not Current</option>
              </FormControl>
            </FormGroup>
            <FormGroup>
              <FormControl
                
                as="select"
                placeholder="Has Oscar"
                id="hasOscar"
                value={state.hasOscar.toString()}
                onChange={(e)=>handleChange(e.target)}
              >
                <option value="false">No Oscar</option>
                <option value="true">Has Oscar</option>
              </FormControl>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="genre"
                type="text"
                placeholder="Genre"
                value={state.genre}
                onChange={(e) =>handleChange(e.target)}
              />
              <FormText className="text-danger text-center">{state.genreError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="description"
                type="text"
                placeholder="Movie description"
                value={state.description}
                onChange={(e) =>handleChange(e.target)}
              />
            </FormGroup>
            <TagsList setState={setState} tags={state.tagss}/>

            <ActorList setState={setState} actors={state.Actorss}/>
            <FormControl
              id="coverPicture"
              type="text"
              placeholder="Banner Url"
              value={state.coverPicture}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                handleBannerUrlChange(e);
              }}
             
            />
                      
            <Row className="d-flex justify-content-center mt-3">
            <Button
              className="col-4"
              type="submit"
              //disabled={state.submitted || !state.canSubmit}
              disabled = {isFormValid()}
              block
            >
              Add Movie
            </Button>
            <Button
            onClick={(e)=>resetFilds(e)}
              className="col-4 mx-3"
              type="reset"
              variant="danger"
              size="sm"
            >
             Reset fileds
            </Button>
            </Row>
          </form>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewMovie);

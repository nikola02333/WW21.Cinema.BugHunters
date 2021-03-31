import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { Row, Table,Form, Col } from "react-bootstrap";
import Spinner from "../Spinner";
import "./../../index.css";
import { IMovie } from "../../models";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from '@fortawesome/free-solid-svg-icons';


import { movieService } from '../Services/movieService';

import Movie from './Movie';
import { imdbService } from './../Services/imdbService';
import { preProcessFile } from "typescript";

interface IState {
  movies: IMovie[];
  filteredMoviesByYear: IMovie[];
  title: string;
  year: string;
  id: string;
  rating: number;
  current: boolean;
  titleError: string;
  yearError: string;
  submitted: boolean;
  isLoading: boolean;
  selectedYear: boolean;
  years: Number[]
  checked:boolean;
  subbmit:boolean;
}

const TopTenMovies: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        coverPicture: "",
        title: "",
        year: "",
        rating: 0,
      },
    ],
    filteredMoviesByYear: [
      {
        id: "",
        coverPicture: "",
        title: "",
        year: "",
        rating: 0,
      },
    ],
    title: "",
    year: "",
    id: "",
    rating: 0,
    current: false,
    titleError: "",
    yearError: "",
    submitted: false,
    isLoading: true,
    selectedYear: false,
    years:[0],
    checked:false,
    subbmit:false
  });

  useEffect( () => {
    getTopTenMovies();
   getAllYears();
  }, []);

 

  const getAllYears = async() => {
    var yearss = await movieService.getAllYears();
    
    if(yearss === undefined)
    {
      return;
    }
    setState( prevState=> ({...prevState, years: yearss}) );

  }

const getTopTenMoviesFomImdb = async()=>{

  
  console.log(state.checked);
  if(!state.checked){
    setState(prevState=> ({ ...prevState, isLoading: true }));
    var moviesImdb = await imdbService.getTopTenMovies();
    debugger;
    if( moviesImdb === undefined)
    {
      setState(prevState=> ({ ...prevState, isLoading: false,checked:!prevState.checked }));
      return;
    }
    setState(prevState=> ({ ...prevState, movies: moviesImdb, isLoading: false }));
  }
  setState((prev)=>({...prev,checked:!prev.checked}));
}

const getTopTenMovies =  async()=>{
    setState((prev)=>({ ...prev,isLoading: true }));
    var movies = await movieService.getTopTen();
    console.log(movies);
    if(movies === undefined)
    {
      return;
    }
    setState(prevState=> ({ ...prevState, movies: movies, isLoading: false }));

  }

  const fillTableWithDaata = () => {
    if (state.movies.length > 0) {
      return state.movies.map((filteredMovie) => {
        return (
          <tr key={filteredMovie.id}>
            <td>{filteredMovie.title}</td>
            <td>{filteredMovie.year}</td>
            <td>{filteredMovie.rating}/10</td>
          </tr>
        );
      });
    } else {
      if (state.selectedYear) {
        setState({ ...state, selectedYear: false });
        NotificationManager.error("Movies with selected year don't exist.");
      }
      return state.movies.map((movie) => {
        return (
          <tr key={movie.id}>
            <td>{movie.title}</td>
            <td>{movie.year}</td>
            <td>{movie.rating}/10</td>
          </tr>
        );
      });
    }
  };
const getMoviesByYear = async(e)=> {

  const { id, value } = e;
  if(value==="0"){
    getTopTenMovies();
    return;
  }else{
    const movies = await movieService.getTopTenMoviesByYear(value); 
  if(movies == undefined)
  {
    return;
  }
    setState(prevState=> ({ ...prevState, movies: movies }));

  }
  
  };

    const rowsData = fillTableWithDaata();
    const table = (
      <Table striped bordered hover size="sm" variant="dark">
        <thead>
          <tr>
            <th>Title</th>
            <th>Year</th>
            <th>Rating</th>
          </tr>
        </thead>
        <tbody>{rowsData}</tbody>
    </Table>
  );


  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">Top 10 Movies</h1>
      </Row>
      <Row className="year-filter justify-content-start">
        <Col  xs={1}>
        <span className="filter-heading ">Filter by:</span>
        </Col>
        <Col  xs={1}>
        <select
         onChange={(e)=> getMoviesByYear(e.target)}
         name="movieYear"
         id="movieYear"
         className="select-dropdown">
          <option value="0">Year</option>
         { state.years.map(year => ( <option key={year.toString()} value={year.toString()}>{year}</option>))}
        </select>
       </Col>

       <Col xs={7}>
           <Form.Check 
           type="checkbox"
           defaultChecked={state.checked}
           onClick={()=>{    
                          getTopTenMoviesFomImdb();}}  
           label="Top10 Movies from IMDb" />
       </Col>
        
      </Row>
      
      {state.isLoading ? <Spinner></Spinner> : (<Movie movies={state.movies}/> )}
    </React.Fragment>
  );
};

export default TopTenMovies;

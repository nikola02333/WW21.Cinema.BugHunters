import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { Row, Table } from "react-bootstrap";
import Spinner from "../../Spinner";
import "./../../../index.css";
import { IMovie } from "../../../models";

import { movieService } from './../../Services/movieService';
import Movie from '../../MovieComponent/Movie';
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
    years:[]
  });

  useEffect( () => {
    getTopTenMovies();
   getAllYears();
  }, []);

  useEffect( ()=>{
    console.log(state.years);
  },[state])

  const getAllYears = async() => {

    
    var yearss = await movieService.getAllYears();
    
    setState( prevState=> ({...prevState, years: yearss}) );

    var x = await showYears();
    
   // console.log(x);
  }
  const getTopTenMovies =  async()=>{

    setState({ ...state,isLoading: true });
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
            <td>{Math.round(filteredMovie.rating)}/10</td>
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
            <td>{Math.round(movie.rating)}/10</td>
          </tr>
        );
      });
    }
  };
const getMoviesByYear = async(e)=> {

  const { id, value } = e;
  
  const movies = await movieService.getTopTenMoviesByYear(value); 
if(movies == undefined)
{
  return;
}
  setState(prevState=> ({ ...prevState, movies: movies }));
};

  const showYears = async () => {
    let yearOptions: JSX.Element[] = [];
    //  
    var yearss = await movieService.getAllYears();

    //console.log(yearss);
    /*
    for (let i = yearss[0]; i <= yearss.lentgh; i++) {
      yearOptions.push(<option value={i}>{i}</option>);
    }
    */
    if(yearss == null)
    {
      return;
    }

    yearOptions=  yearss.map(year => ( <option key={year} value={year}>{year}</option>));
    return yearOptions;
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
      <Row className="year-filter">
        <span className="filter-heading">Filter by:</span>
        <select
        
         onChange={(e)=> getMoviesByYear(e.target)}
          name="movieYear"
          id="movieYear"
          className="select-dropdown"
          //min="1900"
          //max="2100"
        >
          <option value="none">Year</option>
          { state.years.map(year => ( <option key={year.toString()} value={year.toString()}>{year}</option>))}
        </select>
      </Row>

    {state.isLoading ? <Spinner></Spinner> :
    (<Movie 
    movies={state.movies}
    
    /> )}
    </React.Fragment>
  );
};

export default TopTenMovies;

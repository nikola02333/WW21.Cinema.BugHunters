import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import Spinner from "../../Spinner";
import "./../../../index.css";
import { IMovie } from "../../../models";

import { movieService } from './../../Services/movieService';

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
  });

  useEffect(() => {
    getTopTenMovies();
  }, []);

  
// setState({ ...state, movies: data, isLoading: false });
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

  const showYears = () => {
    let yearOptions: JSX.Element[] = [];
    for (let i = 1960; i <= 2100; i++) {
      yearOptions.push(<option value={i}>{i}</option>);
    }
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
  //const showTable = state.isLoading ? <Spinner></Spinner> : table;
  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">Top 10 Movies</h1>
      </Row>
      <Row className="year-filter">
        <span className="filter-heading">Filter by:</span>
        <select
        
          /*/onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
            getTopTenMoviesByYear(e.target.value)
          }
        */
          name="movieYear"
          id="movieYear"
          className="select-dropdown"
          //min="1900"
          //max="2100"
        >
          <option value="none">Year</option>
          {showYears}
        </select>
      </Row>
      <Row className="no-gutters pr-5 pl-5">
      {state.isLoading ? <Spinner></Spinner> : table}
        </Row>


    </React.Fragment>
  );
};

export default TopTenMovies;

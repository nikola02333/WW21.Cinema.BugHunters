import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { Row, Table } from "react-bootstrap";
import Spinner from "../../components/Spinner";
import "../../index.css";
import {
  isAdmin,
  isSuperUser,
  isUser,
  isGuest,
} from "../helpers/authCheck";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { IMovie, ITag } from "../../models/index";

import { movieService } from './../Services/movieService';
import Movie from './Movie';

interface IState {
  movies: IMovie[];
    isLoading: boolean;
}

const Movies: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        coverPicture: "",
        title: "",
        year: "",
        current: false,
        rating: 0,
      },
    ],
    isLoading: true,
  });

  toast.configure();

  
  

  useEffect(() => {
    getMovies();
  }, []);

 
  const changeCurrent = async(
    e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>,
    id: string
  ) => {
    e.preventDefault();

  
   let result = await movieService.changeCurrent(id);
   if(result === undefined)
   {
    NotificationManager.error('Error while changing current state of the movie');
   }
   else{
    const newState = state.movies.filter((movie) => {
      return movie.id !== id;
    });
    setState({ ...state, movies: newState });
    setTimeout(() => {
      window.location.reload();
    }, 100);
   }
  };

  const getMovies = async() => {
    if (isAdmin() === true || isSuperUser() === true || isUser()== true) {
      
      setState({ ...state, isLoading: true });
     
      var movies= await movieService.getAllMovies();
      setState({ ...state, movies: movies, isLoading: false });
    }
  };

  const removeMovie =  async(id: string) => {

     var result = await movieService.removeMovie(id);
     if(result === undefined)
     {

     }
     else{
      var moviesDeleted= state.movies.filter( movie => movie.id != id );
      setState({...state, movies: moviesDeleted});
     }
  };


  // dovde je za movie!!!
  const editMovie = (id: string) => {
    props.history.push(`editmovie/${id}`);
  };

  const searchMovie = async(tag: string) => {

    var moviesSearch = await movieService.searcMovie(tag);

    if(moviesSearch != undefined)
    {
      setState({ ...state, movies: moviesSearch, isLoading: false });
    }
   
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    var data = new FormData(e.currentTarget);

    var queryParts: string[] = [];
    var entries = data.entries();

    for (var pair of entries) {
      queryParts.push(
        `${encodeURIComponent(pair[0])}=${encodeURIComponent(
          pair[1].toString()
        )}`
      );
    }
    var query = queryParts.join("&");
    var loc = window.location;
    var url = `${loc.protocol}//${loc.host}${loc.pathname}?${query}`;

    let tag = url.split("=")[1];

    if (tag) {
      searchMovie(tag);
    } else {
      NotificationManager.error(
        "Please type type something in search bar to search for movies."
      );
    }
  };
  const handleChange = (e) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
  };


  // ovo je za movie!!!
  let inputValue;

  // dovde !!!
  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">Search all Movies by tags or title</h1>
      </Row>
      <Row>
        <form
          onSubmit={(e: React.FormEvent<HTMLFormElement>) => handleSubmit(e)}
          className="form-inline search-field md-form mr-auto mb-4 search-form search-form-second"
        >
          <input
            className="mr-sm-2 search-bar"
            id="tag"
            type="text"
            placeholder="Search"
            name="inputValue"
            value={inputValue}
            aria-label="Search"
          />
          <button className="btn-search" type="submit">
            Search
          </button>
        </form>
      </Row>
  
    {state.isLoading ? <Spinner></Spinner> :
    (<Movie 
    movies={state.movies}
    editMovie={editMovie}
    removeMovie={removeMovie} 
    changeCurrent={changeCurrent}
    
    /> )}
    </React.Fragment>
  );
};

export default Movies;

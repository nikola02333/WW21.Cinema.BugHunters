import React, { useEffect, useState} from "react";
import { NotificationManager } from "react-notifications";
import { Row } from "react-bootstrap";
import Spinner from "../../components/Spinner";
import "../../index.css";

import {  toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { IMovie } from "../../models/index";

import { movieService } from './../Services/movieService';
import Movie from './Movie';

interface IState {
  movies: IMovie[];
  isLoading: boolean;
}
interface IProps{
  showTopTenMovies: boolean;
}

const Movies: React.FC<IProps> =  (props: any) => {
  
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        coverPicture: "",
        title: "",
        year: "",
        current: false,
        rating: 0,
        tagsModel:[{

          tagId:0,
  
          tagName:"",
  
          tagValue:""
  
        }]
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
      setState({ ...state, isLoading: true });
     
      var movies= await movieService.getAllMovies();
      if(movies === undefined)
      {
        return;
      }
      setState({ ...state, movies: movies, isLoading: false });
   
  };

  const removeMovie =  async(id: string) => {

     var result = await movieService.removeMovie(id);
     if(result === undefined)
     {
      return;
     }
     else{
      var moviesDeleted= state.movies.filter( movie => movie.id !== id );
      setState({...state, movies: moviesDeleted});
     }
  };


  // dovde je za movie!!!
  const editMovie = (id: string) => {
    props.history.push(`editmovie/${id}`);
  };

  const searchMovie = async(tag: string) => {

   
    var moviesSearch = await movieService.searcMovie(tag);
    if(moviesSearch !== undefined)
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
      getMovies();
    }
  };

  let inputValue;


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
<Row className="no-gutters pr-5 pl-5">
  
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

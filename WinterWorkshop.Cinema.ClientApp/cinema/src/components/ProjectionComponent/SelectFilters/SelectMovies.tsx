import React,{memo,useMemo,useEffect} from 'react'
import { idText } from 'typescript';

const SelectMovies = memo((props:{info,setInfo,filteredMovies,movies}) => {

    var movieIsSame = false;
    const fillFilterWithMovies = () => {
        if (props.info.selectedAuditorium || props.info.selectedCinema) {
         const filterMovies = props.filteredMovies.map((movie) => {
           if(movie.id==props.info.movieId){
             movieIsSame=true;
           }
            return (
              <option value={movie.id} key={movie.id}>
                {movie.title}
              </option>
            );
          });
        if(!movieIsSame){
          props.setInfo((prev)=>({ ...prev, selectedMovie: false, movieId:""}));
        }  
        console.log(movieIsSame);
        return filterMovies;
        } else {
          return props.movies.map((movie) => {
            return (
              <option value={movie.id} key={movie.id}>
                {movie.title}
              </option>
            );
          });
        }
      };
      const onChange = (e)=> { 
        if(e.target.value!=="none"){
          e.persist();
          props.setInfo((prev)=>({ ...prev, selectedMovie: true, movieId: e.target.value }));
        }else{
          props.setInfo((prev)=>({ ...prev, selectedMovie: false, movieId:""}));
        }
       
    };
    return(
        <select
          name="movieId"
          id="movie"
          className="select-dropdown"
          onChange={onChange}
        >
          <option value="none">Movie</option>
          {fillFilterWithMovies()}
        </select>
    );
});

export default SelectMovies;
import React,{memo} from 'react'

const SelectMovies = memo((props:{info,filteredMovies,movies}) => {
console.log("FilterMovies")
    const fillFilterWithMovies = () => {
        if (props.info.selectedAuditorium || props.info.selectedCinema) {
          return props.filteredMovies.map((movie) => {
            return (
              <option value={movie.id} key={movie.id}>
                {movie.title}
              </option>
            );
          });
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
    return(
        <select
          name="movieId"
          id="movie"
          className="select-dropdown"
        >
           {/* <option value="" selected disabled hidden>Movie</option>  */}
          <option value="none">Movie</option>
          {fillFilterWithMovies()}
        </select>
    );
});

export default SelectMovies;
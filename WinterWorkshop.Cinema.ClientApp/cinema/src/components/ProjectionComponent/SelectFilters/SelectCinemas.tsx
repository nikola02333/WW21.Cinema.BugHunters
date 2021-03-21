import React,{memo} from 'react'
import {getAuditoriumsBySelectedCinema,getMoviesBySelectedCinema} from "../ProjectionService"

const SelectCinenma  = memo((props:{setInfo,cinemas,setFilteredData}) =>{
  console.log("FilterCinenma")
    const fillFilterWithCinemas = () => {
        return props.cinemas.map((cinema) => {
          return (
            <option value={cinema.id} key={cinema.id}>
              {cinema.name}
            </option>
          );
        });
      };
    

    return(
        <select
          onChange={(e) => {getAuditoriumsBySelectedCinema(e.target.value,props.setInfo, props.setFilteredData);
                            getMoviesBySelectedCinema(e.target.value,props.setInfo, props.setFilteredData);}}
          name="cinemaId"
          id="cinema"
          className="select-dropdown"
        >
          <option value="none">Cinema</option>
          {fillFilterWithCinemas()}
        </select>
    );
});
export default SelectCinenma;
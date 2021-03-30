import React,{memo} from 'react'
import {auditoriumService} from "../../Services/auditoriumService"
import {movieService} from "../../Services/movieService"

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

    const hendleChange = async(e) =>{
      if(e.target.value!=="none"){
      props.setInfo((prev)=>({...prev, cinemaId: e.target.value }));
      getAuditoriumsBySelectedCinema(e.target.value);
      getMoviesBySelectedCinema(e.target.value);
      props.setInfo((prev)=>({
        ...prev,
        isLoading: false,
        selectedCinema: true
      }))
      }else
      {
        props.setInfo((prev)=>({
          ...prev,
          cinemaId: "",
          selectedCinema: false
        }))
      }
    }
    
    const getAuditoriumsBySelectedCinema = async(id:number) =>{
      var data =await auditoriumService.getAuditoriumByCinemaId(id);

      if(data===undefined){
        return;
      }
      props.setFilteredData((prev)=>({
        ...prev,
        filteredAuditoriums: data,
      }));
      
    }

    const getMoviesBySelectedCinema =async(id:number)=>{

      const data =await movieService.getMovieByCinemaId(id);
      if(data===undefined){
        return;
      }
      props.setFilteredData((prev)=>({
        ...prev,
        filteredMovies: data
      }));
      props.setInfo((prev)=>({
        ...prev,
        isLoading: false,
        selectedCinema: true,
      }));
    }

    return(
        <select
          onChange={(e) => {hendleChange(e)}}
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
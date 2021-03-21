import React,{memo} from 'react'
import {getMoviesBySelectedAuditorium} from "../ProjectionService"

const SelectAuditoriums  = memo( (props:{selectedCinema,filteredAuditoriums,auditoriums,setInfo,setFilteredData}) =>{
    console.log("FilterAuditoriums")
    const fillFilterWithAuditoriums = () => {
        if (props.selectedCinema) {
          return props.filteredAuditoriums.map((auditorium) => {
            return <option key={auditorium.id} value={auditorium.id}>{auditorium.name}</option>;
          });
        } else {
          return props.auditoriums.map((auditorium) => {
            return (
              <option   value={auditorium.id} key={auditorium.id}>
                {auditorium.name}
              </option>
            );
          });
        }
      };
    return(
        <select
        onChange={(e) => getMoviesBySelectedAuditorium(e.target.value , props.setInfo, props.setFilteredData)}
        name="auditoriumId"
        id="auditorium"
        className="select-dropdown"
        
      >
        <option value="none">Auditorium</option>
        {fillFilterWithAuditoriums()}
      </select>
    );
});
export default SelectAuditoriums;